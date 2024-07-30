using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MaxRev.Gdal.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zybach.API;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Models.Helpers;

namespace Zybach.Tests.IntegrationTests.PrismAPI;

[TestClass]
public class PrismTests
{
    private static PrismAPIService _prismAPIService;
    private static ZybachDbContext _dbContext;
    private static BlobService _blobService;
    private static int _userID = 46; // Mikey
    private static UserDto _callingUser;

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
        _prismAPIService = new PrismAPIService(nullPrismAPIServiceLogger, null, _dbContext, httpClient, _blobService);

        GdalBase.ConfigureAll();

        _callingUser = Users.GetByUserID(_dbContext, _userID);
    }

    [DataTestMethod]
    [DataRow("ppt", "20210101", "20210131")]
    //[DataRow("tmin", "20210101", "20210131")]
    //[DataRow("tmax", "20210101", "20210131")]
    public async Task CanDownloadDataForDateRange(string dataTypeAsString, string startDate, string endDate)
    {
        var dataType = PrismDataType.All.First(x => x.PrismDataTypeName == dataTypeAsString);
        var start = DateTime.ParseExact(startDate, "yyyyMMdd", null);
        var end = DateTime.ParseExact(endDate, "yyyyMMdd", null);
        var success = await _prismAPIService.GetZipfilesForDateRange(dataType, start, end, _callingUser);
        Assert.IsTrue(success);
    }

    [DataTestMethod]
    [DataRow("ppt", "20240102")]
    [DataRow("ppt", "20210131")]
    [DataRow("ppt", "20240718")]
    public async Task CanUseGdalToRasterizeIrrigationUnitGeometries(string dataTypeAsString, string dateAsString)
    {
        var dataType = PrismDataType.All.First(x => x.PrismDataTypeName == dataTypeAsString);
        var date = DateTime.ParseExact(dateAsString, "yyyyMMdd", null);
        var prismResult = await _prismAPIService.SaveZipFileToBlobStorage(dataType, date, _callingUser);
        Assert.IsNotNull(prismResult.BlobID);

        var dataset = await _prismAPIService.GetBilFileAsDataset(prismResult.BlobID.Value);

        var geoTransform = new double[6];
        dataset.GetGeoTransform(geoTransform);

        var band = dataset.GetRasterBand(1);

        var irrigationUnitGeometries = await _dbContext.AgHubIrrigationUnitGeometries.AsNoTracking().ToListAsync();
        foreach (var irrigationUnitGeometry in irrigationUnitGeometries)
        {
            var geometry = irrigationUnitGeometry.IrrigationUnitGeometry;
            var centroid = geometry.Centroid;

            var x = (int)((centroid.X - geoTransform[0]) / geoTransform[1]);
            var y = (int)((centroid.Y - geoTransform[3]) / geoTransform[5]);
            var rasterValues = new float[1];
            band.ReadRaster(x, y, 1, 1, rasterValues, 1, 1, 0, 0);
            var rasterValueInMM = rasterValues[0];
            var rasterValueInIN = rasterValueInMM / 25.4; // 1 inch = 25.4 mm

            var curveNumber = 70; //TODO: Look up curve number based on tillage type for that irrigation unit.

            var runOff = RunoffCalculatorHelper.Runoff(curveNumber, rasterValueInIN);

            Console.WriteLine($"({rasterValueInIN}, {runOff})");
        }

        dataset.Dispose();
        _prismAPIService.CleanupTempFiles(prismResult.BlobID.Value);
    }

    [DataTestMethod]
    [DataRow("20240401")]
    public async Task CanCalculateAndSaveRunoffForAllIrrigationUnitsForYearMonth(string dateAsString)
    {
        var startDate = DateTime.ParseExact(dateAsString, "yyyyMMdd", null);
        var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
        var endDate = new DateTime(startDate.Year, startDate.Month, daysInMonth);

        var success = await _prismAPIService.GetZipfilesForDateRange(PrismDataType.ppt, startDate, endDate, _callingUser);
        Assert.IsTrue(success);

        await _prismAPIService.CalculateAndSaveRunoffForAllIrrigationUnitsForYearMonth(_callingUser, startDate.Year, startDate.Month);
    }
}