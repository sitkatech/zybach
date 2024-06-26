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
    private PrismAPIHelper _prismAPIHelper;

    [ClassInitialize]
    public void ClassInitialize()
    {
        _prismAPIHelper = new PrismAPIHelper();
    }

    [DataTestMethod]
    [DataRow("ppt", "2021-01-01")]
    public async Task TestDownload(string elementAsQueryValue, string date)
    {
        var element = PrismDataElement.FromString(elementAsQueryValue);
        var cached = await _prismAPIHelper.GetDataAsZipFolder(element, date);

        var zipFileName = _prismAPIHelper.GetZipFileName(element, date);
        Assert.IsTrue(File.Exists(zipFileName));

        var unzipSuccessful = _prismAPIHelper.UnzipFolder(element, date);
        Assert.IsTrue(unzipSuccessful);
        var directoryName = _prismAPIHelper.GetContainingDirectoryName(element, date);
        Assert.IsTrue(Directory.Exists(directoryName));
    }
}

public class PrismAPIHelper
{
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
        var containingFolderName = GetContainingDirectoryName(element, date);
        if (Directory.Exists(containingFolderName))
        {
            return true;
        }

        var requestURL = $"{_baseURL}/{element}/{date}";
        var httpClient = new HttpClient();
        var response = await httpClient.GetByteArrayAsync(requestURL);
        await File.WriteAllBytesAsync($"{element}-{date}", response);
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
        var containingFolderName = GetContainingDirectoryName(element, date);
        if (Directory.Exists(containingFolderName))
        {
            return true;
        }

        var zipFileName = GetZipFileName(element, date);
        if (!File.Exists(zipFileName))
        {
            return false;
        }

        Directory.CreateDirectory(containingFolderName);
        ZipFile.ExtractToDirectory(zipFileName, containingFolderName);

        return true;
    }

    public string GetContainingDirectoryName(PrismDataElement element, string date)
    {
        return $"PRISM_{element}_stable_4km_{date}_bil";
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