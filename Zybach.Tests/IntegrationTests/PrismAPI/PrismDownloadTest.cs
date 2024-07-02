using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSGeo.GDAL;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.Prism;

namespace Zybach.Tests.IntegrationTests.PrismAPI;

[TestClass]
public class PrismDownloadTest
{
    private static PrismAPIService _prismAPIHelper;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _prismAPIHelper = new PrismAPIService();
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

        var success = await _prismAPIHelper.GetDataForDateRange(element, start, end);
        Assert.IsTrue(success);
    }

    [DataTestMethod]
    [DataRow("ppt", "20210101")]
    public async Task CanReadBILData(string elementAsQueryValue, string date)
    {
        var dbCS = "Data Source=host.docker.internal;Initial Catalog=ZybachDB;Persist Security Info=True;User ID=DockerWebUser;Password=password#1;Encrypt=False;";
        var dbOptions = new DbContextOptionsBuilder<ZybachDbContext>();
        dbOptions.UseSqlServer(dbCS, x => x.UseNetTopologySuite());
        var dbContext = new ZybachDbContext(dbOptions.Options);

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


                dbContext.PrismRecords.Add(new PrismRecord()
                {
                    ElementType = element.ToString(),
                    Date = DateTime.ParseExact(date, "yyyyMMdd", null),
                    X = col,
                    Y = row,
                    Value = buffer[row * width + col]
                });
            }

            Console.WriteLine();
        }

        await dbContext.SaveChangesAsync();

        // Cleanup
        dataset.Dispose();
    }
}