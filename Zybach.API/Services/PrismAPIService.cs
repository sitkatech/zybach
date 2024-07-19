using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services;

//TODO: MK 7/3/2024 -- This service needs to be refactored to:
//  TODO: Use blob storage instead of local storage, however I am not sure that there is blob storage atm in Zybach so punting for now.
//  TODO: Clean up the unzipped files after use.
//  TODO: Provide override to force a redownload and reprocessing if user requests it. 
//  TODO: Inject this service in startup instead of just newing it up. Not the highest priority IMO but good for consistency/correctness. Alternatively it could probably be made static with a little thought.
//  TODO: Possibly should add support for the monthly and yearly data, requested with yyyyMM and yyyy respectively. Going with daily by default by now as that is the most granular, and should be a good example of how to get the other types if we need.
//  TODO: Really not sure with how to deal with the fact that we a) don't know when new data will show up and b) the data changes up to 8 times over 6 months before its considered stable.
//  TODO: Inject dbContext (or values required to new up a dbcontext when we go to hangfire) and add both the blobresource and prismdailyrecord and link FKs up as required

public class PrismAPIService
{
    private readonly ILogger<PrismAPIService> _logger;
    private readonly ZybachConfiguration _zybachConfiguration;
    private readonly ZybachDbContext _dbContext;
    private readonly HttpClient _httpClient;
    private readonly BlobService _blobService;


    public PrismAPIService(ILogger<PrismAPIService> logger, ZybachConfiguration configuration, ZybachDbContext dbContext, HttpClient httpClient, BlobService blobService)
    {
        _logger = logger;
        _zybachConfiguration = configuration;
        _dbContext = dbContext;
        _httpClient = httpClient;
        _blobService = blobService;
    }

    /// <summary>
    /// Retrieves and processes data for a specified date range for a given Prism data element.
    /// </summary>
    /// <param name="dataType">The data element to retrieve and process.</param>
    /// <param name="start">The start date of the date range.</param>
    /// <param name="end">The end date of the date range.</param>
    /// <param name="callingUser">User making the request.</param>
    /// <returns>Returns true if the data retrieval and processing is successful.</returns>
    /// <exception cref="ArgumentException">Thrown when the start date is after the end date.</exception>
    public async Task<bool> GetDataForDateRange(PrismDataType dataType, DateTime start, DateTime end, UserDto callingUser)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        var currentDate = start;
        while (currentDate <= end)
        {
            await GetDataAsZipFolder(dataType, currentDate, callingUser);
            //UnzipFolder(element, date);
            currentDate = currentDate.AddDays(1); 
            Thread.Sleep(2000); //MK 6/26/2024 -- Their example code has a 2 second delay between requests, with a note that says to be kind to their server.
        }

        return true;
    }

    public bool GetDataForDate(PrismDataType dataType, DateTime date, UserDto callingUser)
    {
        var hadOrGotZip = GetDataAsZipFolder(dataType, date, callingUser);
        return hadOrGotZip.Result;
    }

    /// <summary>
    /// Downloads climate data from the PRISM API and saves it as a zip file if not already present locally. Please see https://prism.oregonstate.edu/documents/PRISM_downloads_web_service.pdf for more information.
    /// </summary>
    /// <param name="dataType">The type of climate data to retrieve (e.g., ppt, tmin, tmax).</param>
    /// <param name="date">The date for which to retrieve the data. Format can be YYYYMMDD for daily, YYYYMM for monthly, or YYYY for annual/historical.</param>
    /// <returns>A boolean indicating whether the data was successfully downloaded or already exists locally.</returns>
    public async Task<bool> GetDataAsZipFolder(PrismDataType dataType, DateTime date, UserDto callingUser, bool forceRedownload = false)
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
                return true;
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
                return false;
            }

            await using var streamToReadFrom = await response.Content.ReadAsStreamAsync();

            var blob = await _blobService.CreateBlobResource(_dbContext, streamToReadFrom, GetFileName(dataType, date, "zip"), callingUser.UserID);
            prismDailySyncRecord.BlobResourceID = blob.BlobResourceID;
            prismDailySyncRecord.PrismSyncStatusID = PrismSyncStatus.Succeeded.PrismSyncStatusID;
            _dbContext.Update(prismDailySyncRecord);
            await _dbContext.SaveChangesAsync();

            return true;

        }
        catch(Exception e)
        {
            prismDailySyncRecord.PrismSyncStatusID = PrismSyncStatus.Failed.PrismSyncStatusID;
            prismDailySyncRecord.ErrorMessage = e.Message;
            _dbContext.Update(prismDailySyncRecord);
            await _dbContext.SaveChangesAsync();
            return false;
        }
    }

    /// <summary>
    /// Unzips a downloaded PRISM climate data zip file to a specified directory.
    /// </summary>
    /// <param name="dataType">The type of climate data contained in the zip file (e.g., ppt, tmin, tmax).</param>
    /// <param name="date">The date corresponding to the data in the zip file. Format can be YYYYMMDD for daily, YYYYMM for monthly, or YYYY for annual/historical.</param>
    /// <returns>A boolean indicating whether the zip file was successfully unzipped or if the data already exists in the target directory.</returns>
    //public bool UnzipFolder(PrismDataElement element, string date)
    //{
    //    var containingDirectoryFullPath = GetContainingDirectoryFullPath(element, date);
    //    if (Directory.Exists(containingDirectoryFullPath))
    //    {
    //        return true;
    //    }

    //    var zipFileFullPath = GetZipFileFullPath(element, date);
    //    if (!File.Exists(zipFileFullPath))
    //    {
    //        return false;
    //    }

    //    if (!Directory.Exists(_baseDataDirectory))
    //    {
    //        Directory.CreateDirectory(_baseDataDirectory);
    //    }

    //    if (!Directory.Exists($"{_baseDataDirectory}/{element}"))
    //    {
    //        Directory.CreateDirectory($"{_baseDataDirectory}/{element}");
    //    }

    //    Directory.CreateDirectory(containingDirectoryFullPath!);

    //    // Extract the ZIP file to the target directory
    //    using var zip = new ZipArchive(new FileStream(zipFileFullPath, FileMode.Open, FileAccess.Read));
    //    zip.ExtractToDirectory(containingDirectoryFullPath);
    //    return true;
    //}

    //public string GetContainingDirectoryFullPath(PrismDataElement element, string date)
    //{
    //    return $"{_baseDataDirectory}/{element}/{GetFolderName(element, date)}";
    //}

    public string GetFolderName(PrismDataType dataType, DateTime date)
    {
        var dateAsString = date.ToString("yyyyMMdd");
        return $"PRISM_{dataType}_stable_4km_{dateAsString}_bil";
    }

    public string GetFileName(PrismDataType dataType, DateTime date, string extension)
    {
        var dateAsString = date.ToString("yyyyMMdd");
        return $"PRISM_{dataType.PrismDataTypeName}_stable_4kmD2_{dateAsString}_bil.{extension}";
    }

    //public string GetZipFileFullPath(PrismDataElement element, string date)
    //{
    //    return $"{_baseZIPDirectory}/{element}/{GetZipFileName(element, date)}";
    //}

    //public string GetZipFileName(PrismDataElement element, string date)
    //{
    //    var containingFolderName = GetFolderName(element, date);
    //    return $"{containingFolderName}.zip";
    //}
}