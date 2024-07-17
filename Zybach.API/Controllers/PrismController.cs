using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

    public PrismSyncController(ZybachDbContext dbContext, ILogger<PrismSyncController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> rioConfiguration, UserDto callingUser, PrismAPIService prismAPIService) : base(dbContext, logger, keystoneService, rioConfiguration)
    {
        _callingUser = callingUser;
        _prismAPIService = prismAPIService;
    }

    [HttpGet("/prism-monthly-sync/years/{year}/data-types/{prismDataTypeName}")]
    [AdminFeature]
    public async Task<ActionResult<List<PrismMonthlySyncDto>>> GetPrismMonthlySyncs([FromRoute] int year, [FromRoute] string prismDataTypeName)
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

        var syncRecords = await PrismMonthlySync.Get(_dbContext, year, prismDataType);
        return Ok(syncRecords);
    }

    [HttpPut("/prism-monthly-sync/years/{year}/months/{month}/data-types/{prismDataTypeName}/sync")]
    [AdminFeature]
    public async Task<ActionResult<PrismMonthlySyncDto>> Sync([FromRoute] int year, [FromRoute] int month, [FromRoute] string prismDataTypeName)
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

        var prismMonthlySync = await PrismMonthlySync.Get(_dbContext, year, month, prismDataType);
        if (prismMonthlySync == null)
        {
            return NotFound("Record not found.");
        }

        await PrismMonthlySync.UpdateStatus(_dbContext, _callingUser, year, month, prismDataType, PrismSyncStatus.InProgress);

        #region Hangfire this?

        var monthStartDate = new DateTime(year, month, 1);
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var monthEndDate = new DateTime(year, month, daysInMonth);

        var success = await _prismAPIService.GetDataForDateRange(prismDataType, monthStartDate, monthEndDate, _callingUser);

        if (!success)
        {
            await PrismMonthlySync.UpdateStatus(_dbContext, _callingUser, year, month, prismDataType, PrismSyncStatus.Failed);
        }
        else
        {
            await PrismMonthlySync.UpdateStatus(_dbContext, _callingUser, year, month, prismDataType, PrismSyncStatus.Succeeded);
        }

        #endregion

        var record = await PrismMonthlySync.Get(_dbContext, year, month, prismDataType);
        return Ok(record);
    }

    [HttpPut("/prism-monthly-sync/years/{year}/months/{month}/data-types/{prismDataTypeName}/finalize")]
    [AdminFeature]
    public async Task<ActionResult<PrismMonthlySyncDto>> UpdateFinalizedDate([FromRoute] int year, [FromRoute] int month, [FromRoute] string prismDataTypeName, [FromBody] PrismMonthlySyncUpsertDto prismMonthlySyncUpsertDto)
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

        var prismMonthlySync = await PrismMonthlySync.Get(_dbContext, year, month, prismDataType);
        if (prismMonthlySync == null)
        {
            return NotFound("Record not found.");
        }

        var updatedRecord = await PrismMonthlySync.Finalize(_dbContext, _callingUser, year, month, prismDataType, prismMonthlySyncUpsertDto);
        return Ok(updatedRecord);
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