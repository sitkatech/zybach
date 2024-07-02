using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSGeo.GDAL;
using Zybach.Models.Abstracts;

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