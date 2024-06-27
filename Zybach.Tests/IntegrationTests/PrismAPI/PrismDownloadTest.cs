using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    public async Task TestDownload(string elementAsQueryValue, string date)
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
}

public class PrismAPIHelper
{
    private static string _baseZIPDirectory = "C:/Sitka/Zybach/PRISM_API/ZIP_ARCHIVE";
    private static string _baseDataDirectory = "C:/Sitka/Zybach/PRISM_API/DATA";

    private static string _baseURL  = "https://services.nacse.org/prism/data/public/4km";

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
        
        var zipDirectory = Directory.Exists(_baseZIPDirectory);
        if (!zipDirectory)
        {
            Directory.CreateDirectory(_baseZIPDirectory);
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


        Directory.CreateDirectory(containingDirectoryFullPath!);

        // Extract the ZIP file to the target directory
        using var zip = new ZipArchive(new FileStream(zipFileFullPath, FileMode.Open, FileAccess.Read));
        zip.ExtractToDirectory(containingDirectoryFullPath);
        return true;
    }

    public string GetContainingDirectoryFullPath(PrismDataElement element, string date)
    {
        return $"{_baseDataDirectory}/{GetContainingDirectoryName(element, date)}";
    }

    public string GetContainingDirectoryName(PrismDataElement element, string date)
    {
        return $"PRISM_{element}_stable_4km_{date}_bil";
    }

    public string GetZipFileFullPath(PrismDataElement element, string date)
    {
        return $"{_baseZIPDirectory}/{GetZipFileName(element, date)}";
    }

    public string GetZipFileName(PrismDataElement element, string date)
    {
        var containingFolderName = GetContainingDirectoryName(element, date);
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