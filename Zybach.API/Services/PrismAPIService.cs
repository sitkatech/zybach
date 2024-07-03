using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Zybach.Models.Prism;

namespace Zybach.API.Services;

//TODO: MK 7/3/2024 -- This service needs to be refactored to:
//  TODO: Use blob storage instead of local storage, however I am not sure that there is blob storage atm in Zybach so punting for now.
//  TODO: Clean up the unzipped files after use.
//  TODO: Check if the SQL data exists instead of the files for skipping.
//  TODO: Insert the data into SQL? Might be best to do it as close to where we get it as possible so we don't have to bring it into memory multiple times. Will require passing in DBContext which couples this service to the EF layer.
//  TODO: Possibly need to save the header file in SQL as well might need a new entity and a FK.
//  TODO: SQL might still not be correct for this data as GDAL might just work better using the files natively, but I wanted to prove it out and also the card has an acceptance criteria to show the bil data from the website and serving via our api/sql would be my default way to do that.
//  TODO: It takes about 4 seconds to query a days worth of data (~870,000 rows). Not sure if there is more  I could do to speed that up and given the previous TODO it might not be worth it.
//  TODO: If we do stick with SQL it could be good to have some functions to rebuild the BIL and header file from SQL which should be very doable. 
//  TODO: Provide override to force a redownload and reprocessing if user requests it. 
//  TODO: Inject this service in startup instead of just newing it up. Not the highest priority IMO but good for consistency/correctness. Alternatively it could probably be made static with a little thought.
//  TODO: Possibly should add support for the monthly and yearly data, requested with yyyyMM and yyyy respectively. Going with daily by default by now as that is the most granular, and should be a good example of how to get the other types if we need.
//  TODO: Really not sure with how to deal with the fact that we a) don't know when new data will show up and b) the data changes up to 8 times over 6 months before its considered stable.


public class PrismAPIService
{
    private const string _baseZIPDirectory = "C:/Sitka/Zybach/PRISM_API/ZIP_ARCHIVE";
    private const string _baseDataDirectory = "C:/Sitka/Zybach/PRISM_API/DATA";

    private const string _baseURL = "https://services.nacse.org/prism/data/public/4km";

    /// <summary>
    /// Retrieves and processes data for a specified date range for a given Prism data element.
    /// </summary>
    /// <param name="element">The data element to retrieve and process.</param>
    /// <param name="start">The start date of the date range.</param>
    /// <param name="end">The end date of the date range.</param>
    /// <returns>Returns true if the data retrieval and processing is successful.</returns>
    /// <exception cref="ArgumentException">Thrown when the start date is after the end date.</exception>
    public async Task<bool> GetDataForDateRange(PrismDataElement element, DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        var currentDate = start;
        while (currentDate <= end)
        {
            var date = currentDate.ToString("yyyyMMdd");
            await GetDataAsZipFolder(element, date);
            UnzipFolder(element, date);
            currentDate = currentDate.AddDays(1); 
            Thread.Sleep(2000); //MK 6/26/2024 -- Their example code has a 2 second delay between requests, with a note that says to be kind to their server.
        }

        return true;
    }

    public bool GetDataForDate(PrismDataElement element, DateTime date)
    {
        var dateAsString = date.ToString("yyyyMMdd");
        var hadOrGotZip = GetDataAsZipFolder(element, dateAsString);
        var haveUnzipped = UnzipFolder(element, dateAsString);
        return hadOrGotZip.Result && haveUnzipped;
    }
    
    /// <summary>
    /// Downloads climate data from the PRISM API and saves it as a zip file if not already present locally. Please see https://prism.oregonstate.edu/documents/PRISM_downloads_web_service.pdf for more information.
    /// </summary>
    /// <param name="element">The type of climate data to retrieve (e.g., ppt, tmin, tmax).</param>
    /// <param name="date">The date for which to retrieve the data. Format can be YYYYMMDD for daily, YYYYMM for monthly, or YYYY for annual/historical.</param>
    /// <returns>A boolean indicating whether the data was successfully downloaded or already exists locally.</returns>
    public async Task<bool> GetDataAsZipFolder(PrismDataElement element, string date)
    {
        //MK 6/26/2024 -- Check if we have the file already, if we do skip the download so we don't get IP banned. 
        var containingDirectoryFullPath = GetContainingDirectoryFullPath(element, date);
        if (Directory.Exists(containingDirectoryFullPath))
        {
            return true;
        }

        var requestURL = $"{_baseURL}/{element}/{date}";
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(requestURL);

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        if (!Directory.Exists(_baseZIPDirectory))
        {
            Directory.CreateDirectory(_baseZIPDirectory);
        }

        if (!Directory.Exists($"{_baseZIPDirectory}/{element}"))
        {
            Directory.CreateDirectory($"{_baseZIPDirectory}/{element}");
        }

        await using var streamToReadFrom = await response.Content.ReadAsStreamAsync();

        // Save the ZIP file to disk
        var zipFileFullPath = GetZipFileFullPath(element, date);
        await using (var fileStream = new FileStream(zipFileFullPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await streamToReadFrom.CopyToAsync(fileStream);
        }
        
        await streamToReadFrom.DisposeAsync();

        var file = new FileInfo(zipFileFullPath);
        if (!file.Exists)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Unzips a downloaded PRISM climate data zip file to a specified directory.
    /// </summary>
    /// <param name="element">The type of climate data contained in the zip file (e.g., ppt, tmin, tmax).</param>
    /// <param name="date">The date corresponding to the data in the zip file. Format can be YYYYMMDD for daily, YYYYMM for monthly, or YYYY for annual/historical.</param>
    /// <returns>A boolean indicating whether the zip file was successfully unzipped or if the data already exists in the target directory.</returns>
    public bool UnzipFolder(PrismDataElement element, string date)
    {
        var containingDirectoryFullPath = GetContainingDirectoryFullPath(element, date);
        if (Directory.Exists(containingDirectoryFullPath))
        {
            return true;
        }

        var zipFileFullPath = GetZipFileFullPath(element, date);
        if (!File.Exists(zipFileFullPath))
        {
            return false;
        }

        if (!Directory.Exists(_baseDataDirectory))
        {
            Directory.CreateDirectory(_baseDataDirectory);
        }

        if (!Directory.Exists($"{_baseDataDirectory}/{element}"))
        {
            Directory.CreateDirectory($"{_baseDataDirectory}/{element}");
        }

        Directory.CreateDirectory(containingDirectoryFullPath!);

        // Extract the ZIP file to the target directory
        using var zip = new ZipArchive(new FileStream(zipFileFullPath, FileMode.Open, FileAccess.Read));
        zip.ExtractToDirectory(containingDirectoryFullPath);
        return true;
    }

    public string GetContainingDirectoryFullPath(PrismDataElement element, string date)
    {
        return $"{_baseDataDirectory}/{element}/{GetFolderName(element, date)}";
    }

    public string GetFolderName(PrismDataElement element, string date)
    {
        return $"PRISM_{element}_stable_4km_{date}_bil";
    }

    public string GetFileName(PrismDataElement element, string date, string extension)
    {
        return $"PRISM_{element}_stable_4kmD2_{date}_bil.{extension}";
    }

    public string GetZipFileFullPath(PrismDataElement element, string date)
    {
        return $"{_baseZIPDirectory}/{element}/{GetZipFileName(element, date)}";
    }

    public string GetZipFileName(PrismDataElement element, string date)
    {
        var containingFolderName = GetFolderName(element, date);
        return $"{containingFolderName}.zip";
    }
}