using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSGeo.GDAL;

namespace Zybach.Tests.IntegrationTests.PrismAPI;

[TestClass]
public class PrismDownloadTest
{
    private static PrismAPIHelper _prismAPIHelper;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _prismAPIHelper = new PrismAPIHelper();
    }

    [DataTestMethod]
    [DataRow("ppt", "20210101")]
    public async Task CanDownloadDataForDate(string elementAsQueryValue, string date)
    {
        var element = PrismDataElement.FromString(elementAsQueryValue);
        var cached = await _prismAPIHelper.GetDataAsZipFolder(element, date);

        var zipFileFullPath = _prismAPIHelper.GetZipFileFullPath(element, date);
        Assert.IsTrue(File.Exists(zipFileFullPath));

        var unzipSuccessful = _prismAPIHelper.UnzipFolder(element, date);
        Assert.IsTrue(unzipSuccessful);

        var containingDirectoryFullPath = _prismAPIHelper.GetContainingDirectoryFullPath(element, date);
        Assert.IsTrue(Directory.Exists(containingDirectoryFullPath));
    }

    [DataTestMethod]
    [DataRow("ppt", "20210101", "20210131")]
    [DataRow("tmin", "20210101", "20210131")]
    [DataRow("tmax", "20210101", "20210131")]
    public async Task CanDownloadDataForDateRange(string elementAsQueryValue, string startDate, string endDate)
    {
        var element = PrismDataElement.FromString(elementAsQueryValue);
        var start = DateTime.ParseExact(startDate, "yyyyMMdd", null);
        var end = DateTime.ParseExact(endDate, "yyyyMMdd", null);

        var success = await _prismAPIHelper.GetDataForDateRange(start, end, element);
        Assert.IsTrue(success);
    }

    [DataTestMethod]
    [DataRow("ppt", "20210101")]
    public async Task CanReadBILData(string elementAsQueryValue, string date)
    {
        await CanDownloadDataForDate(elementAsQueryValue, date);

        var element = PrismDataElement.FromString(elementAsQueryValue);
        var containingDirectoryFullPath = _prismAPIHelper.GetContainingDirectoryFullPath(element, date);
        
        GdalConfiguration.ConfigureGdal();
            
        var bilFile = $"{containingDirectoryFullPath}/{_prismAPIHelper.GetFileName(element, date, "bil")}";
        var dataset = Gdal.Open(bilFile, Access.GA_ReadOnly);
        Assert.IsNotNull(dataset, "Error opening file.");

        var band = dataset.GetRasterBand(1);
        Assert.IsNotNull(band, "Error getting band.");

        // Get raster dimensions
        var width = band.XSize;
        var height = band.YSize;

        // Create buffer to hold raster data
        var buffer = new float[width * height];

        // Read raster data into buffer
        band.ReadRaster(0, 0, width, height, buffer, width, height, 0, 0);

        // Print a portion of the data
        for (var row = 0; row < Math.Min(height, 50); row++)
        {
            for (var col = 0; col < Math.Min(width, 50); col++)
            {
                Console.Write(buffer[row * width + col] + " ");
            }

            Console.WriteLine();
        }

        // Cleanup
        dataset.Dispose();
    }
}

public class PrismAPIHelper
{
    private const string _baseZIPDirectory = "C:/Sitka/Zybach/PRISM_API/ZIP_ARCHIVE";
    private const string _baseDataDirectory = "C:/Sitka/Zybach/PRISM_API/DATA";

    private const string _baseURL = "https://services.nacse.org/prism/data/public/4km";

    /// <summary>
    /// Retrieves and processes data for a specified date range for a given Prism data element.
    /// </summary>
    /// <param name="start">The start date of the date range.</param>
    /// <param name="end">The end date of the date range.</param>
    /// <param name="element">The data element to retrieve and process.</param>
    /// <returns>Returns true if the data retrieval and processing is successful.</returns>
    /// <exception cref="ArgumentException">Thrown when the start date is after the end date.</exception>
    public async Task<bool> GetDataForDateRange(DateTime start, DateTime end, PrismDataElement element)
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

public class PrismDataElement : ClassEnum<PrismDataElement>
{
    public string QueryValue { get; set; }

    public PrismDataElement(string val)
    {
        QueryValue = val;
    }

    public static PrismDataElement PPT = new ("ppt");
    public static PrismDataElement TMin = new ("tmin");
    public static PrismDataElement TMax = new("tmax");
    public static PrismDataElement TMean = new ("tmean");
    public static PrismDataElement TDMean = new ("tdmean");
    public static PrismDataElement VPDMin = new ("vpdmin");
    public static PrismDataElement VPDMaxn = new ("vpdmax");

    public override string ToString()
    {
        return QueryValue;
    }

    public static PrismDataElement FromString(string val)
    {
        return All.First(x => x.QueryValue == val);
    }
}

public abstract class ClassEnum<T>
{
    public static List<T> All
    {
        get
        {
            var classType = typeof(T);
            var enumValues = classType
                            .GetFields(BindingFlags.Public | BindingFlags.Static)
                            .Where(x => x.GetValue(null)!.GetType() == classType)
                            .Select(x => (T)x.GetValue(null))
                            .Where(x => x != null);

            return enumValues.ToList();
        }
    }
}