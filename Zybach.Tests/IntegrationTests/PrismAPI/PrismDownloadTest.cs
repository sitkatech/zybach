using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSGeo.GDAL;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.Tests.IntegrationTests.PrismAPI;

[TestClass]
public class PrismDownloadTest
{
    private static PrismAPIService _prismAPIHelper;
    private static ZybachDbContext _dbContext;
    private static BlobService _blobService;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        var dbCS = AssemblySteps.Configuration["sqlConnectionString"];
        var dbOptions = new DbContextOptionsBuilder<ZybachDbContext>();
        dbOptions.UseSqlServer(dbCS, x => x.UseNetTopologySuite());
        _dbContext = new ZybachDbContext(dbOptions.Options);

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://services.nacse.org");

        var nullAzureStorageLogger = new NullLogger<AzureStorage>();
        var azureStorage = new AzureStorage(AssemblySteps.Configuration["AzureBlobStorageConnectionString"], nullAzureStorageLogger);

        var nullBlobServiceLogger = new NullLogger<BlobService>();
        _blobService = new BlobService(nullBlobServiceLogger, azureStorage);

        var nullPrismAPIServiceLogger = new NullLogger<PrismAPIService>();
        _prismAPIHelper = new PrismAPIService(nullPrismAPIServiceLogger, null, _dbContext, httpClient, _blobService);

        GdalConfiguration.ConfigureGdal();
    }

    //[DataTestMethod]
    //[DataRow("ppt", "20210101")]
    //public async Task CanDownloadDataForDate(string elementAsQueryValue, string date)
    //{
    //    var element = PrismDataElement.FromString(elementAsQueryValue);
    //    var cached = await _prismAPIHelper.GetDataAsZipFolder(element, date, null);

    //    var zipFileFullPath = _prismAPIHelper.GetZipFileFullPath(element, date);
    //    Assert.IsTrue(File.Exists(zipFileFullPath));

    //    var unzipSuccessful = _prismAPIHelper.UnzipFolder(element, date);
    //    Assert.IsTrue(unzipSuccessful);

    //    var containingDirectoryFullPath = _prismAPIHelper.GetContainingDirectoryFullPath(element, date);
    //    Assert.IsTrue(Directory.Exists(containingDirectoryFullPath));
    //}

    [DataTestMethod]
    [DataRow("ppt", "20210101", "20210131", 46)] //46 == Mikey
    [DataRow("tmin", "20210101", "20210131", 46)] //46 == Mikey
    [DataRow("tmax", "20210101", "20210131", 46)] //46 == Mikey
    public async Task CanDownloadDataForDateRange(string dataTypeAsString, string startDate, string endDate, int userID)
    {
        var dataType = PrismDataType.All.First(x => x.PrismDataTypeName == dataTypeAsString);
        var start = DateTime.ParseExact(startDate, "yyyyMMdd", null);
        var end = DateTime.ParseExact(endDate, "yyyyMMdd", null);
        var callingUser = Users.GetByUserID(_dbContext, userID);
        var success = await _prismAPIHelper.GetDataForDateRange(dataType, start, end, callingUser);
        Assert.IsTrue(success);
    }

    //[DataTestMethod]
    //[DataRow("ppt", "20210102")]
    //public async Task<Dataset> CanReadAndSaveBILDataToSQL(string elementAsQueryValue, string dateAsString)
    //{
    //    await CanDownloadDataForDate(elementAsQueryValue, dateAsString);

    //    var element = PrismDataElement.FromString(elementAsQueryValue);
    //    var containingDirectoryFullPath = _prismAPIHelper.GetContainingDirectoryFullPath(element, dateAsString);
        
    //    var bilFile = $"{containingDirectoryFullPath}/{_prismAPIHelper.GetFileName(element, dateAsString, "bil")}";
    //    var dataset = Gdal.Open(bilFile, Access.GA_ReadOnly);
    //    Assert.IsNotNull(dataset, "Error opening file.");

    //    //MK 7/3/2024 -- Loop through each band in the raster dataset, index starts at 1 blech... Prism data looks like it might only have a single band though.
    //    for (var i = 1; i <= dataset.RasterCount; i++)
    //    {
    //        await ProcessBand(_dbContext, dataset, i, element.ToString(), dateAsString);
    //    }

    //    return dataset;
    //}

    //[DataTestMethod]
    //[DataRow("ppt", "20210102")]
    //public async Task CanUseGdalToRasterizeIrrigationUnitGeometries(string elementAsQueryValue, string dateAsString)
    //{
    //    var dataset = await CanReadAndSaveBILDataToSQL(elementAsQueryValue, dateAsString);

    //    var element = PrismDataElement.FromString(elementAsQueryValue);
    //    var irrigationUnitGeometries = await _dbContext.AgHubIrrigationUnitGeometries.AsNoTracking().ToListAsync();
        
    //    var geoTransform = new double[6];
    //    dataset.GetGeoTransform(geoTransform);

    //    var band = dataset.GetRasterBand(1);

    //    foreach (var irrigationUnitGeometry in irrigationUnitGeometries)
    //    {
    //        var geometry = irrigationUnitGeometry.IrrigationUnitGeometry;
    //        var centroid = geometry.Centroid;

    //        var x = (int)((centroid.X - geoTransform[0]) / geoTransform[1]);
    //        var y = (int)((centroid.Y - geoTransform[3]) / geoTransform[5]);
    //        var rasterValues = new float[1];
    //        band.ReadRaster(x, y, 1, 1, rasterValues, 1, 1, 0, 0);
    //        var rasterValue = rasterValues[0];

    //        var curveNumber = "look-up-curve-number-provided-in-db";
    //        var calculatedRunOff = "https://en.wikipedia.org/wiki/Runoff_curve_number";

    //        //TODO calculate that runoff and save to DB?
    //    }
    //}
    
    //TODO: Move this into the service
    private async Task ProcessBand(ZybachDbContext dbContext, Dataset dataset, int bandIndex, string element, string dateAsString)
    {
        await dbContext.SaveChangesAsync();

        var band = dataset.GetRasterBand(bandIndex);
        Assert.IsNotNull(band, "Error getting band.");

        // Get raster dimensions
        var width = band.XSize;
        var height = band.YSize;

        // Create buffer to hold raster data
        var buffer = new float[width * height];

        // Read raster data into buffer
        band.ReadRaster(0, 0, width, height, buffer, width, height, 0, 0);

        var maxDataCountToConsoleWrite = 50;
        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                // Print a portion of the data
                if (row < maxDataCountToConsoleWrite && col < maxDataCountToConsoleWrite)
                {
                    Console.Write($"{buffer[row * width + col]:F2} ");
                }
            }

            if (row < maxDataCountToConsoleWrite)
            {
                Console.WriteLine();
            }
        }

        await dbContext.SaveChangesAsync();
    }
}