using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers;

[ApiController]
public class PrismSyncController : SitkaController<PrismSyncController>
{
    private readonly UserDto _callingUser;
    private PrismAPIService _prismAPIService;
    private readonly BlobService _blobService;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public PrismSyncController(ZybachDbContext dbContext, ILogger<PrismSyncController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> rioConfiguration, UserDto callingUser, PrismAPIService prismAPIService, BlobService blobService, IBackgroundJobClient backgroundJobClient) : base(dbContext, logger, keystoneService, rioConfiguration)
    {
        _callingUser = callingUser;
        _prismAPIService = prismAPIService;
        _blobService = blobService;
        _backgroundJobClient = backgroundJobClient;
    }

    [HttpGet("/prism-monthly-sync/years/{year}/data-types/{prismDataTypeName}")]
    [AdminFeature]
    public async Task<ActionResult<List<PrismMonthlySyncSimpleDto>>> GetPrismMonthlySyncs([FromRoute] int year, [FromRoute] string prismDataTypeName)
    {
        var allowedYears = GetAllowedYears();
        if (!allowedYears.Contains(year))
        {
            return NotFound($"Year must be 2020 or later, and before the current year.");
        }

        var prismDataType = PrismDataType.AllAsDto.FirstOrDefault(x => x.PrismDataTypeName == prismDataTypeName);
        if (prismDataType == null)
        {
            return NotFound("Invalid Prism Data Type Name.");
        }

        var syncRecords = await PrismMonthlySyncs.ListSimpleByYearAndDataType(_dbContext, year, prismDataType);
        return Ok(syncRecords);
    }

    [HttpPut("/prism-monthly-sync/years/{year}/months/{month}/data-types/{prismDataTypeName}/sync")]
    [AdminFeature]
    public async Task<ActionResult<PrismMonthlySyncSimpleDto>> Sync([FromRoute] int year, [FromRoute] int month, [FromRoute] string prismDataTypeName)
    {
        var allowedYears = GetAllowedYears();
        if (!allowedYears.Contains(year))
        {
            return NotFound($"Year must be 2020 or later, and before the current year.");
        }

        var allowedMonths = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        if (!allowedMonths.Contains(month))
        {
            return NotFound("Month must be between 1 and 12.");
        }

        var prismDataType = PrismDataType.All.FirstOrDefault(x => x.PrismDataTypeName == prismDataTypeName);
        if (prismDataType == null)
        {
            return NotFound("Invalid Prism Data Type Name.");
        }

        var prismMonthlySync = await PrismMonthlySyncs.GetSimpleByYearMonthAndDataType(_dbContext, year, month, prismDataType);
        if (prismMonthlySync == null)
        {
            return NotFound($"Monthly sync record not found for {year} {month} {prismDataType.PrismDataTypeName}.");
        }

        //Do not allow syncing multiple times in a day.
        if (prismMonthlySync.LastSynchronizedDate.HasValue && prismMonthlySync.LastSynchronizedDate.Value.Date == DateTime.UtcNow.Date)
        {
            return BadRequest("Cannot sync more than once per day to be respectful to PRISM resources and prevent being IP banned.");
        }

        await PrismMonthlySyncs.UpdateStatus(_dbContext, _callingUser, year, month, prismDataType, PrismSyncStatus.InProgress);

        var syncJobID = _backgroundJobClient.Enqueue(() => _prismAPIService.SyncPrismData(year, month, prismDataTypeName, _callingUser));

        var record = await PrismMonthlySyncs.GetSimpleByYearMonthAndDataType(_dbContext, year, month, prismDataType);
        return Ok(record);
    }

    [HttpGet("/prism-monthly-sync/years/{year}/months/{month}/data-types/{prismDataTypeName}/download")]
    [AdminFeature]
    [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Download([FromRoute] int year, [FromRoute] int month, [FromRoute] string prismDataTypeName)
    {
        var allowedYears = GetAllowedYears();
        if (!allowedYears.Contains(year))
        {
            return NotFound($"Year must be 2020 or later, and before the current year.");
        }

        var allowedMonths = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        if (!allowedMonths.Contains(month))
        {
            return NotFound("Month must be between 1 and 12.");
        }

        var prismDataType = PrismDataType.All.FirstOrDefault(x => x.PrismDataTypeName == prismDataTypeName);
        if (prismDataType == null)
        {
            return NotFound("Invalid Prism Data Type Name.");
        }

        var record = await PrismMonthlySyncs.GetSimpleByYearMonthAndDataType(_dbContext, year, month, prismDataType);
        if (record == null)
        {
            return NotFound("Record not found.");
        }

        var dailyRecords = await PrismDailyRecords.ListSimpleByYearMonthAndDataType(_dbContext, year, month, prismDataType);

        var fileStreams = new Dictionary<string, Stream>();

        foreach (var dailyRecord in dailyRecords)
        {
            var fileBytes = await _blobService.GetFileStreamFromBlobStorage(dailyRecord.BlobResourceCanonicalName);
            fileStreams[dailyRecord.BlobFileName] = fileBytes;
        }

        var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var kvp in fileStreams)
            {
                var zipEntry = archive.CreateEntry($"{kvp.Key}.zip", CompressionLevel.Fastest);
                await using var zipStream = zipEntry.Open();
                await kvp.Value.CopyToAsync(zipStream);
            }
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        var contentType = "application/zip";
        var fileName = $"prism-data-{year}-{month}-{prismDataTypeName}";
        return File(memoryStream, contentType, fileName);
    } 

    [HttpPut("/prism-monthly-sync/years/{year}/months/{month}/data-types/{prismDataTypeName}/finalize")]
    [AdminFeature]
    public async Task<ActionResult<PrismMonthlySyncDto>> UpdateFinalizedDate([FromRoute] int year, [FromRoute] int month, [FromRoute] string prismDataTypeName)
    {
        var allowedYears = GetAllowedYears();
        if (!allowedYears.Contains(year))
        {
            return NotFound($"Year must be 2020 or later, and before the current year.");
        }

        var allowedMonths = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        if (!allowedMonths.Contains(month))
        {
            return NotFound("Month must be between 1 and 12.");
        }

        var prismDataType = PrismDataType.All.FirstOrDefault(x => x.PrismDataTypeName == prismDataTypeName);
        if (prismDataType == null)
        {
            return NotFound("Invalid Prism Data Type Name.");
        }

        var prismMonthlySync = await PrismMonthlySyncs.GetSimpleByYearMonthAndDataType(_dbContext, year, month, prismDataType);
        if (prismMonthlySync == null)
        {
            return NotFound("Record not found.");
        }

        if (prismMonthlySync.PrismSyncStatusID != PrismSyncStatus.Succeeded.PrismSyncStatusID)
        {
            return BadRequest("Cannot finalize a record that has not been successfully synchronized.");
        }

        if (prismMonthlySync.RunoffCalculationStatusID != RunoffCalculationStatus.Succeeded.RunoffCalculationStatusID)
        {
            return BadRequest("Cannot finalize a record that has not had the runoff calculated successfully.");
        }

        var updatedRecord = await PrismMonthlySyncs.Finalize(_dbContext, _callingUser, year, month, prismDataType);
        return Ok(updatedRecord);
    }

    [HttpPut("/prism-monthly-sync/years/{year}/months/{month}/calculate-runoff")]
    [AdminFeature]
    public async Task<ActionResult<PrismMonthlySyncSimpleDto>> CalculateRunoff([FromRoute] int year, [FromRoute] int month)
    {
        var allowedYears = GetAllowedYears();
        if (!allowedYears.Contains(year))
        {
            return NotFound($"Year must be 2020 or later, and before the current year.");
        }

        var allowedMonths = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        if (!allowedMonths.Contains(month))
        {
            return NotFound("Month must be between 1 and 12.");
        }

        var prismMonthlySync = await PrismMonthlySyncs.GetSimpleByYearMonthAndDataType(_dbContext, year, month, PrismDataType.ppt);
        if (prismMonthlySync == null)
        {
            return NotFound($"Monthly sync record not found for {year} {month} {PrismDataType.ppt.PrismDataTypeDisplayName}.");
        }

        if (prismMonthlySync.PrismSyncStatusID != PrismSyncStatus.Succeeded.PrismSyncStatusID)
        {
            return BadRequest("Cannot calculate runoff for a record that has not been successfully synchronized.");
        }

        await PrismMonthlySyncs.UpdateRunoffCalculationStatus(_dbContext, _callingUser, year, month, RunoffCalculationStatus.InProgress);

        var runoffCalculationJobID = _backgroundJobClient.Enqueue(() => _prismAPIService.CalculateAndSaveRunoffForAllIrrigationUnitsForYearMonth(_callingUser, year, month));

        var record = await PrismMonthlySyncs.GetSimpleByYearMonthAndDataType(_dbContext, year, month, PrismDataType.ppt);
        return Ok(record);
    }

    [HttpGet("/runoff-data/years/{year}")]
    [ZybachViewFeature]
    public async Task<ActionResult<List<AgHubIrrigationUnitRunoffSimpleDto>>> GetRunoffDataForYear([FromRoute] int year)
    {
        var allowedYears = GetAllowedYears();
        if (!allowedYears.Contains(year))
        {
            return NotFound($"Year must be 2020 or later, and before the current year.");
        }

        var result = await AgHubIrrigationUnitRunoffs.ListSimpleForYear(_dbContext, year);
        return Ok(result);
    }

    private List<int> GetAllowedYears()
    {
        var startYear = 2020;
        var currentYear = DateTime.UtcNow.Year;

        var allowedYears = new List<int>();
        for (var y = startYear; y <= currentYear; y++)
        {
            allowedYears.Add(y);
        }

        return allowedYears;
    }
}