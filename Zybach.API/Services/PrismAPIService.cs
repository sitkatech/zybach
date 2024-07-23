using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSGeo.GDAL;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services;

public class PrismAPIService
{
    private readonly ILogger<PrismAPIService> _logger;
    private readonly IOptions<ZybachConfiguration> _zybachConfiguration;
    private readonly ZybachDbContext _dbContext;
    private readonly HttpClient _httpClient;
    private readonly BlobService _blobService;

    public PrismAPIService(ILogger<PrismAPIService> logger, IOptions<ZybachConfiguration> configuration, ZybachDbContext dbContext, HttpClient httpClient, BlobService blobService)
    {
        _logger = logger;
        _zybachConfiguration = configuration;
        _dbContext = dbContext;
        _httpClient = httpClient;
        _blobService = blobService;
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task SyncPrismData(int year, int month, string prismDataTypeName, UserDto callingUser)
    {
        var monthStartDate = new DateTime(year, month, 1);
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var monthEndDate = new DateTime(year, month, daysInMonth);
        var prismDataType = PrismDataType.All.FirstOrDefault(x => x.PrismDataTypeName == prismDataTypeName);
        var success = await GetZipfilesForDateRange(prismDataType, monthStartDate, monthEndDate, callingUser);
        if (!success)
        {
            await PrismMonthlySyncs.UpdateStatus(_dbContext, callingUser, year, month, prismDataType, PrismSyncStatus.Failed);
        }
        else
        {
            await PrismMonthlySyncs.UpdateStatus(_dbContext, callingUser, year, month, prismDataType, PrismSyncStatus.Succeeded);
        }
    }

    /// <summary>
    /// Retrieves and processes data for a specified date range for a given PRISM data type.
    /// </summary>
    /// <param name="dataType">The data element to retrieve and process.</param>
    /// <param name="start">The start date of the date range.</param>
    /// <param name="end">The end date of the date range.</param>
    /// <param name="callingUser">The user making the request.</param>
    /// <returns>Returns true if the data retrieval and processing is successful.</returns>
    /// <exception cref="ArgumentException">Thrown when the start date is after the end date.</exception>
    public async Task<bool> GetZipfilesForDateRange(PrismDataType dataType, DateTime start, DateTime end, UserDto callingUser)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        var maxDateAllowed = DateTime.UtcNow - TimeSpan.FromDays(1); //Prism Data seems to be delayed by a day, might want this to be configurable?
        var endDate = end < maxDateAllowed ? end : maxDateAllowed;

        var currentDate = start;
        while (currentDate <= endDate)
        {
            var result = await SaveZipFileToBlobStorage(dataType, currentDate, callingUser);
            currentDate = currentDate.AddDays(1);

            if (result.MadePrismRequest)
            {
                Thread.Sleep(2000); //MK 6/26/2024 -- Their example code has a 2 second delay between requests, with a note that says to be kind to their server.
            }
        }

        return true;
    }

    /// <summary>
    /// Downloads climate data from the PRISM API and saves it as a zip file if not already present. Please see https://prism.oregonstate.edu/documents/PRISM_downloads_web_service.pdf for more information.
    /// </summary>
    /// <param name="dataType">The type of climate data to retrieve (e.g., ppt, tmin, tmax).</param>
    /// <param name="date">The date for which to retrieve the data. Format can be YYYYMMDD for daily, YYYYMM for monthly, or YYYY for annual/historical.</param>
    /// <param name="callingUser">The user making the request.</param>
    /// <param name="forceRedownload">A boolean indicating whether to force a redownload of the data even if it already exists.</param>
    /// <returns>A boolean indicating whether the data was successfully downloaded or already exists.</returns>
    public async Task<PrismResult> SaveZipFileToBlobStorage(PrismDataType dataType, DateTime date, UserDto callingUser, bool forceRedownload = false)
    {
        //MK 6/26/2024 -- Check if we have the sync records in place.
        var prismDailySyncRecord = await _dbContext.PrismDailyRecords.FirstOrDefaultAsync(x => x.Year == date.Year && x.Month == date.Month && x.Day == date.Day && x.PrismDataTypeID == dataType.PrismDataTypeID);
        if (prismDailySyncRecord == null)
        {
            var monthlySyncRecord = await _dbContext.PrismMonthlySyncs.FirstOrDefaultAsync(x => x.Year == date.Year && x.Month == date.Month && x.PrismDataTypeID == dataType.PrismDataTypeID);
            if (monthlySyncRecord == null)
            {
                monthlySyncRecord = new PrismMonthlySync
                {
                    PrismDataTypeID = dataType.PrismDataTypeID,
                    Year = date.Year,
                    Month = date.Month,
                    PrismSyncStatusID = PrismSyncStatus.InProgress.PrismSyncStatusID
                };

                await _dbContext.PrismMonthlySyncs.AddAsync(monthlySyncRecord);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Entry(monthlySyncRecord).ReloadAsync();
            }

            prismDailySyncRecord = new PrismDailyRecord
            {
                PrismMonthlySyncID = monthlySyncRecord.PrismMonthlySyncID,
                PrismDataTypeID = dataType.PrismDataTypeID,
                PrismSyncStatusID = PrismSyncStatus.InProgress.PrismSyncStatusID,
                Year = date.Year,
                Month = date.Month,
                Day = date.Day,
            };

            await _dbContext.PrismDailyRecords.AddAsync(prismDailySyncRecord);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(prismDailySyncRecord).ReloadAsync();
        }

        if (prismDailySyncRecord.PrismSyncStatusID == PrismSyncStatus.Succeeded.PrismSyncStatusID && !forceRedownload) 
        {
            var blobResource = await _dbContext.BlobResources.FirstOrDefaultAsync(x => x.BlobResourceID == prismDailySyncRecord.BlobResourceID);
            var blobResourceStream = await _blobService.GetFileStreamFromBlobStorage(blobResource.BlobResourceCanonicalName);
            
            if (blobResource != null && blobResourceStream != null)
            {
                return new PrismResult()
                {
                    Success = true,
                    MadePrismRequest = false,
                    ErrorMessage = null,
                    BlobID = blobResource.BlobResourceID
                };
            }
        }

        var dateAsString = date.ToString("yyyyMMdd");
        var requestURL = $"/prism/data/public/4km/{dataType.PrismDataTypeName}/{dateAsString}";

        try
        {
            var response = await _httpClient.GetAsync(requestURL);
            if (!response.IsSuccessStatusCode)
            {
                prismDailySyncRecord.PrismSyncStatusID = PrismSyncStatus.Failed.PrismSyncStatusID;
                prismDailySyncRecord.ErrorMessage = $"{response.StatusCode}: {response.ReasonPhrase}";
                _dbContext.Update(prismDailySyncRecord);
                await _dbContext.SaveChangesAsync();
                return new PrismResult()
                {
                    Success = false,
                    MadePrismRequest = true,
                    ErrorMessage = $"{response.StatusCode}: {response.ReasonPhrase}"
                };
            }

            await using var streamToReadFrom = await response.Content.ReadAsStreamAsync();

            var blob = await _blobService.CreateBlobResource(_dbContext, streamToReadFrom, GetFileName(dataType, date, "zip"), callingUser.UserID);
            prismDailySyncRecord.BlobResourceID = blob.BlobResourceID;
            prismDailySyncRecord.PrismSyncStatusID = PrismSyncStatus.Succeeded.PrismSyncStatusID;
            _dbContext.Update(prismDailySyncRecord);
            await _dbContext.SaveChangesAsync();

            return new PrismResult()
            {
                Success = true,
                MadePrismRequest = true,
                ErrorMessage = null,
                BlobID = blob.BlobResourceID
            };

        }
        catch(Exception e)
        {
            prismDailySyncRecord.PrismSyncStatusID = PrismSyncStatus.Failed.PrismSyncStatusID;
            prismDailySyncRecord.ErrorMessage = e.Message;
            _dbContext.Update(prismDailySyncRecord);
            await _dbContext.SaveChangesAsync();
            return new PrismResult()
            {
                Success = false,
                MadePrismRequest = true,
                ErrorMessage = $"{e}"
            };
        }
    }

    public async Task<Dataset> GetBilFileAsDataset(int blobID)
    {
        var blob = _dbContext.BlobResources.FirstOrDefault(x => x.BlobResourceID == blobID);
        if (blob == null)
        {
            return null;
        }

        var blobStream = await _blobService.GetFileStreamFromBlobStorage(blob.BlobResourceCanonicalName);
        if (blobStream == null)
        {
            return null;
        }

        using var archive = new ZipArchive(blobStream);
        var bilFileEntry = archive.Entries.FirstOrDefault(e => e.Name.EndsWith(".bil", StringComparison.OrdinalIgnoreCase));
        if (bilFileEntry == null)
        {
            return null;
        }

        var headerFileEntry = archive.Entries.FirstOrDefault(e => e.Name.EndsWith(".hdr", StringComparison.OrdinalIgnoreCase));
        if (headerFileEntry == null)
        {
            return null;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), blob.BlobResourceCanonicalName);
        Directory.CreateDirectory(tempDirectory);

        try
        {
            var tempBilFilePath = Path.Combine(tempDirectory, bilFileEntry.Name);
            var tempHeaderFilePath = Path.Combine(tempDirectory, headerFileEntry.Name);

            await using (var tempBilFileStream = new FileStream(tempBilFilePath, FileMode.Create, FileAccess.Write))
            {
                await bilFileEntry.Open().CopyToAsync(tempBilFileStream);
            }

            await using (var tempHeaderFileStream = new FileStream(tempHeaderFilePath, FileMode.Create, FileAccess.Write))
            {
                await headerFileEntry.Open().CopyToAsync(tempHeaderFileStream);
            }

            var dataset = Gdal.Open(tempBilFilePath, Access.GA_ReadOnly);
            if (dataset == null)
            {
                throw new ApplicationException($"'{tempBilFilePath}' not recognized as a supported file format.");
            }

            return dataset;
        }
        catch(Exception e)
        {
            // Clean up the temporary directory
            Directory.Delete(tempDirectory, true);
            return null;
        }
    }

    public void CleanupTempFiles(int blobID)
    {
        var blob = _dbContext.BlobResources.FirstOrDefault(x => x.BlobResourceID == blobID);
        if (blob == null)
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), blob.BlobResourceCanonicalName);
        if (Directory.Exists(tempDirectory))
        {
            Directory.Delete(tempDirectory, true);
        }
    } 

    public string GetFileName(PrismDataType dataType, DateTime date, string extension)
    {
        var dateAsString = date.ToString("yyyyMMdd");
        return $"PRISM_{dataType.PrismDataTypeName}_stable_4kmD2_{dateAsString}_bil.{extension}";
    }
}

public class PrismResult
{
    public bool Success { get; set; }
    public bool MadePrismRequest { get; set; }
    public string ErrorMessage { get; set; }
    public int? BlobID { get; set; }
}