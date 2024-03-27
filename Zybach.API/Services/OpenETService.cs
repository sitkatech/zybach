using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTopologySuite.IO;
using NetTopologySuite.Operation.Buffer;
using Zybach.API.Controllers;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services;

public class OpenETService
{
    private readonly ILogger<OpenETService> _logger;
    private readonly ZybachConfiguration _zybachConfiguration;
    private readonly ZybachDbContext _zybachDbContext;
    private readonly HttpClient _httpClient;

    private const double BoundingBoxLeft = -102.05591553769607;
    private const double BoundingBoxRight = -100.2234130905508;
    private const double BoundingBoxTop = 41.743487069127966;
    private const double BoundingBoxBottom = 40.74358623146166;

    public const string OpenETEvapoWaterMeasurementTypeName = "OpenET Evapotranspiration";
    public const string OpenETPrecipWaterMeasurementTypeName = "OpenET Precipitation";
    public const string OpenETConsumptiveUseWaterMeasurementTypeName = "OpenET Consumptive Use";
    public static readonly List<string> OpenETWaterMeasurementTypeNames = new() { OpenETEvapoWaterMeasurementTypeName, OpenETPrecipWaterMeasurementTypeName, OpenETConsumptiveUseWaterMeasurementTypeName };

    public OpenETService(HttpClient httpClient, ILogger<OpenETService> logger, IOptions<ZybachConfiguration> zybachConfiguration, ZybachDbContext zybachDbContext, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _zybachConfiguration = zybachConfiguration.Value;
        _zybachDbContext = zybachDbContext;
        _httpClient = httpClient;
    }

    private async Task<bool> RasterUpdatedSinceMinimumLastUpdatedDate(int month, int year, int openETDataTypeID, OpenETSyncHistory openETSyncHistory)
    {
        var polygon = new WKTReader().Read(@$"POLYGON (({BoundingBoxLeft} {BoundingBoxTop}, {BoundingBoxRight} {BoundingBoxTop}, {BoundingBoxRight} {BoundingBoxBottom}, {BoundingBoxLeft} {BoundingBoxBottom}, {BoundingBoxLeft} {BoundingBoxTop}))");

        var centerBufferedBy5000SurveyFeet = polygon.Centroid.ProjectTo26860().Buffer(16000, EndCapStyle.Square).ProjectTo4326();
        var envelope = centerBufferedBy5000SurveyFeet.EnvelopeInternal;

        var geometryArray = new[]
        {
            envelope.MinX.ToString(), envelope.MaxY.ToString(),
            envelope.MaxX.ToString(), envelope.MaxY.ToString(),
            envelope.MaxX.ToString(), envelope.MinY.ToString(),
            envelope.MinX.ToString(), envelope.MinY.ToString()
        };
        var openETDataType = openETSyncHistory.OpenETDataType;
        var argumentsObject = new OpenETRasterMetadataPostRequestBody(openETDataType.OpenETDataTypeVariableName, geometryArray);

        try
        {
            var openETRasterMetadataRoute = _zybachConfiguration.OpenETRasterMetadataRoute;
            var response = await _httpClient.PostAsJsonAsync(openETRasterMetadataRoute, argumentsObject);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new OpenETException($"Call to {openETRasterMetadataRoute} was unsuccessful. Status Code: {response.StatusCode} Message: {body}");
            }

            var responseObject = JsonSerializer.Deserialize<RasterMetadataDateIngested>(body);

            if (string.IsNullOrEmpty(responseObject.DateIngested) ||
                !DateTime.TryParse(responseObject.DateIngested, out var responseDate))
            {
                await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.DataNotAvailable);
                return false;
            }

            var openETSyncHistoriesThatWereSuccessful = _zybachDbContext.OpenETSyncHistories.Include(x => x.OpenETSync)
                .Where(x => x.OpenETSync.Year == year && x.OpenETSync.Month == month
                            && x.OpenETSync.OpenETDataTypeID == openETDataTypeID && x.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.Succeeded);

            if (!openETSyncHistoriesThatWereSuccessful.Any())
            {
                return true;
            }

            var mostRecentSyncHistory = openETSyncHistoriesThatWereSuccessful.OrderByDescending(x => x.UpdateDate).First();

            if (responseDate > mostRecentSyncHistory.UpdateDate)
            {
                return true;
            }

            await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.NoNewData);
            return false;
        }
        catch (TaskCanceledException ex)
        {
            await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID,
                OpenETSyncResultTypeEnum.Failed, "OpenET API did not respond");
            _logger.Log(LogLevel.Error, ex, "Error communicating with OpenET API.");
            return false;
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, "Error when attempting to check raster metadata date ingested.");
            await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID,
                OpenETSyncResultTypeEnum.Failed, ex.Message);
            return false;
        }
    }

    public class RasterMetadataDateIngested : OpenETGeneralJsonResponse
    {
        [JsonPropertyName("date_ingested")]
        public string DateIngested { get; set; }
    }

    public class OpenETGeneralJsonResponse
    {
        [JsonPropertyName("ERROR")]
        public string ErrorMessage { get; set; }
        [JsonPropertyName("SOLUTION")]
        public string SuggestedSolution { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("type")]
        public string ResponseType { get; set; }
    }

    public async Task<HttpResponseMessage> TriggerOpenETGoogleBucketRefresh(int year, int month, int openETDataTypeID)
    {
        var monthNameToDisplay = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        var openETDataType = OpenETDataType.AllLookupDictionary[openETDataTypeID];

        if (_zybachDbContext.OpenETSyncHistories.Include(x => x.OpenETSync).AsNoTracking()
            .Any(x => x.OpenETSync.Year == year && x.OpenETSync.Month == month &&
                      (x.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.Created || x.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.InProgress)
                      && x.OpenETSync.OpenETDataTypeID == openETDataTypeID))
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent($"Sync already in progress for {monthNameToDisplay} {year}")
            };
        }

        var openETSyncHistory = OpenETSyncHistories.CreateNew(_zybachDbContext, year, month, openETDataTypeID);

        if (!await RasterUpdatedSinceMinimumLastUpdatedDate(month, year, openETDataTypeID, openETSyncHistory))
        {
            openETSyncHistory = OpenETSyncHistories.GetByOpenETSyncHistoryID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID);
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Content = new StringContent(
                    $"The sync for {monthNameToDisplay} {year} will not be completed for the following reason: {openETSyncHistory.OpenETSyncResultType.OpenETSyncResultTypeDisplayName}.{(openETSyncHistory.OpenETSyncResultType.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.Failed ? $" Error Message:{openETSyncHistory.ErrorMessage}" : "")}")
            };
        }
        Thread.Sleep(3000); // intentional sleep here to avoid maximum rate limit message

        var argumentsObject = new OpenETRasterTimeseriesMultiPolygonPostRequestBody(
            new[]
            {
                $"{openETSyncHistory.OpenETSync.ReportedDate:yyyy-MM-dd}",
                $"{openETSyncHistory.OpenETSync.ReportedDate.AddMonths(1).AddDays(-1):yyyy-MM-dd}"
            }, _zybachConfiguration.OPENET_SHAPEFILE_PATH,
            new[] { _zybachConfiguration.OpenETRasterTimeseriesMultipolygonColumnToUseAsIdentifier },
            openETDataType.OpenETDataTypeVariableName);

        try
        {
            var openETRasterMetadataRoute = _zybachConfiguration.OpenETRasterTimeSeriesMultipolygonRoute;
            var response = await _httpClient.PostAsJsonAsync(openETRasterMetadataRoute, argumentsObject);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new OpenETException(
                    $"Call to {openETRasterMetadataRoute} failed. Status Code: {response.StatusCode} Message: {body}");
            }

            var responseObject = JsonSerializer.Deserialize<TimeseriesMultipolygonSuccessfulResponse>(body);

            await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID,
                OpenETSyncResultTypeEnum.InProgress, null, responseObject.FileRetrievalURL);

            await ProcessOpenETData(_httpClient, openETSyncHistory, openETDataTypeID);

            return response;
        }
        catch (TaskCanceledException ex)
        {
            await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID,
                OpenETSyncResultTypeEnum.Failed, "OpenET API did not respond");
            _logger.Log(LogLevel.Error, ex, "Error communicating with OpenET API.");
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(
                    $"The OpenET API did not respond. The error has been logged and support has been notified.")
            };
        }
        catch (Exception ex)
        {
            await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID,
                OpenETSyncResultTypeEnum.Failed, ex.Message);
            _logger.Log(LogLevel.Error, ex, "Error communicating with OpenET API.");
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(
                    $"There was an error when attempting to create the request. Message: {ex.Message}")
            };
        }
    }

    public class TimeseriesMultipolygonSuccessfulResponse
    {
        [JsonPropertyName("url")]
        public string FileRetrievalURL { get; set; }
    }

    private async Task UpdateStatusAndFailIfOperationHasExceeded24Hours(ZybachDbContext zybachDbContext, OpenETSyncHistory syncHistory, string errorMessage)
    {
        var timeBetweenSyncCreationAndNow = DateTime.UtcNow.Subtract(syncHistory.CreateDate).TotalHours;
        var resultType = timeBetweenSyncCreationAndNow > 24
            ? OpenETSyncResultTypeEnum.Failed
            : OpenETSyncResultTypeEnum.InProgress;

        //One very unfortunate thing about OpenET's design is that they're forced to create a queue of requests and can't process multiple requests at once.
        //That, combined with no (at this moment 7/14/21) means of knowing whether or not a run has completed or failed other than checking to see if the file is ready for export means we have to implement some kind of terminal state.
        await OpenETSyncHistories.UpdateOpenETSyncEntityByID(zybachDbContext, syncHistory.OpenETSyncHistoryID, resultType, resultType == OpenETSyncResultTypeEnum.Failed ? errorMessage : null);
    }

    /// <summary>
    /// Check if OpenET has created data for a particular Year and Month precipitation sync that has been triggered and update
    /// </summary>
    public async Task ProcessOpenETData(HttpClient httpClient, OpenETSyncHistory openETSyncHistory, int openETDataTypeID)
    {
        var openETSync = openETSyncHistory.OpenETSync;
        var openETSyncReportedDate = openETSync.ReportedDate;

        if (string.IsNullOrWhiteSpace(openETSyncHistory.GoogleBucketFileRetrievalURL))
        {
            //We are somehow storing sync histories without file retrieval urls, this is not good
            _logger.Log(LogLevel.Error, new OpenETException(
                $"OpenETSyncHistory record:{openETSyncHistory.OpenETSyncHistoryID} was saved without a file retrieval URL but we attempted to update with it. Check integration!"), "Error communicating with OpenET API.");
            await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.Failed, "Record was saved with a Google Bucket File Retrieval URL. Support has been notified.");
            return;
        }

        var response = await httpClient.GetAsync(openETSyncHistory.GoogleBucketFileRetrievalURL);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = response.StatusCode == HttpStatusCode.NotFound ? "OpenET API never reported the results as available." : await response.Content.ReadAsStringAsync();
            await UpdateStatusAndFailIfOperationHasExceeded24Hours(_zybachDbContext, openETSyncHistory, errorMessage);
            return;
        }

        try
        {
            List<OpenETCSVFormat> records;
            using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture);
                var finalizedOpenETSyncs = _zybachDbContext.OpenETSyncs
                    .Where(x => x.OpenETDataTypeID == openETDataTypeID && x.FinalizeDate.HasValue).ToList()
                    .Select(x => x.ReportedDate);
                csvReader.Context.RegisterClassMap(new OpenETCSVFormatMap(_zybachConfiguration.OpenETRasterTimeseriesMultipolygonColumnToUseAsIdentifier, openETSync.OpenETDataType.OpenETDataTypeVariableName));

                // one final check to make sure we don't get any finalized dates
                records = csvReader.GetRecords<OpenETCSVFormat>()
                    .Where(x => !finalizedOpenETSyncs.Contains(x.Date))
                    .ToList();
            }

            //This shouldn't happen, but if we enter here we've attempted to grab data for a water year that was finalized
            if (!records.Any())
            {
                await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.NoNewData);
            }
            else
            {
                var reportedDate = openETSyncReportedDate.AddMonths(1).AddDays(-1);
                var transactionDate = DateTime.UtcNow;

                await SaveToOpenETWaterMeasurement(records, reportedDate, transactionDate, openETDataTypeID);
                await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.Succeeded);

                await _zybachDbContext.Database.ExecuteSqlRawAsync($"EXECUTE dbo.pUpdateAgHubIrrigationUnitDataWithOpenETWaterMeasurements @reportedDate, @transactionDate, @openETDataTypeID",
                    new SqlParameter("reportedDate", reportedDate), new SqlParameter("transactionDate", transactionDate), new SqlParameter("openETDataTypeID", openETDataTypeID));
            }
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, "Error parsing file from OpenET or getting records into database.");
            await OpenETSyncHistories.UpdateOpenETSyncEntityByID(_zybachDbContext, openETSyncHistory.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.Failed, ex.Message);
        }
    }

    private async Task SaveToOpenETWaterMeasurement(List<OpenETCSVFormat> distinctRecords, DateTime reportedDate, DateTime transactionDate, int openETDataTypeID)
    {
        await _zybachDbContext.OpenETWaterMeasurements.Where(x =>
            x.ReportedDate == reportedDate && x.OpenETDataTypeID == openETDataTypeID).ExecuteDeleteAsync();

        var waterMeasurements = distinctRecords.Select(x => new OpenETWaterMeasurement() {
            WellTPID = x.WellTPID,
            OpenETDataTypeID = openETDataTypeID,
            ReportedDate = reportedDate,
            TransactionDate = transactionDate,
            ReportedValueInches = x.ReportedValueInches,
            ReportedValueAcreFeet = x.ReportedValueAcreFeet,
            IrrigationUnitArea = x.IrrigationUnitArea
        }).ToList();

        _zybachDbContext.OpenETWaterMeasurements.AddRange(waterMeasurements);
        await _zybachDbContext.SaveChangesAsync();
    }

    public async Task<bool> IsOpenETAPIKeyValid()
    {
        const string openETRequestURL = "account/status";
        try
        {
            var response = await _httpClient.GetAsync(openETRequestURL);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new OpenETException(
                    $"Call to {openETRequestURL} was unsuccessful. Status Code: {response.StatusCode} Message: {body}.");
            }

            var responseObject = JsonSerializer.Deserialize<OpenETController.OpenETTokenExpirationDate>(body);

            if (responseObject == null || responseObject.ExpirationDate < DateTime.UtcNow)
            {
                throw new OpenETException($"Deserializing OpenET API Key validation response failed, or the key is expired. Expiration Date: {(responseObject?.ExpirationDate != null ? responseObject.ExpirationDate.ToString(CultureInfo.InvariantCulture) : "Not provided")}");
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, "Error validating OpenET API Key.");
            return false;
        }
    }
}

public class OpenETCSVFormat
{
    public string WellTPID { get; set; }
    public DateTime Date { get; set; }
    public decimal ReportedValueInches { get; set; }
    public decimal ReportedValueAcreFeet { get; set; }
    public decimal IrrigationUnitArea { get; set; }
}

public class OpenETCSVFormatMap : ClassMap<OpenETCSVFormat>
{
    public OpenETCSVFormatMap(string irrigationUnitTPIDColumnName, string reportedValueColumnName)
    {
        Map(m => m.WellTPID).Name(irrigationUnitTPIDColumnName);
        Map(m => m.Date).Name("time");
        Map(m => m.ReportedValueInches).Name(reportedValueColumnName);
        Map(m => m.ReportedValueAcreFeet).Name("acre-feet");
        Map(m => m.IrrigationUnitArea).Name("acres");
    }
}

public class OpenETException : Exception
{
    public OpenETException()
    {
    }

    public OpenETException(string message)
        : base(message)
    {
    }

    public OpenETException(string message, Exception inner)
        : base(message, inner)
    {
    }
}


public class OpenETRasterMetadataPostRequestBody
{
    public OpenETRasterMetadataPostRequestBody(string variable, string[] geometry) : this(
        "ensemble", variable, "gridMET", geometry, "monthly")
    {
    }

    public OpenETRasterMetadataPostRequestBody(string model, string variable, string referenceET, string[] geometry, string interval)
    {
        Model = model;
        Variable = variable;
        ReferenceET = referenceET;
        Geometry = geometry;
        Interval = interval;
    }
    [JsonPropertyName("interval")]
    public string Interval { get; set; }
    [JsonPropertyName("geometry")]
    public string[] Geometry { get; set; }
    [JsonPropertyName("reference_et")]
    public string ReferenceET { get; set; }
    [JsonPropertyName("variable")]
    public string Variable { get; set; }
    [JsonPropertyName("model")]
    public string Model { get; set; }
}

public class OpenETRasterTimeseriesMultiPolygonPostRequestBody
{
    public OpenETRasterTimeseriesMultiPolygonPostRequestBody(string[] dateRange, string interval, string assetID, string[] attributes, string reducer, string model, string variable, string referenceET, string units)
    {
        DateRange = dateRange;
        Interval = interval;
        AssetID = assetID;
        Attributes = attributes;
        Reducer = reducer;
        Model = model;
        Variable = variable;
        ReferenceET = referenceET;
        Units = units;
    }

    public OpenETRasterTimeseriesMultiPolygonPostRequestBody(string[] dateRange, string assetID, string[] attributes, string variable) :
        this(dateRange, "monthly", assetID, attributes, "mean", "ensemble", variable, "gridMET", "in")
    {
        DateRange = dateRange;
        AssetID = assetID;
        Attributes = attributes;
    }

    [JsonPropertyName("date_range")]
    public string[] DateRange { get; set; }
    [JsonPropertyName("interval")]
    public string Interval { get; set; }
    [JsonPropertyName("asset_id")]
    public string AssetID { get; set; }
    [JsonPropertyName("attributes")]
    public string[] Attributes { get; set; }
    [JsonPropertyName("reducer")]
    public string Reducer { get; set; }
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("variable")]
    public string Variable { get; set; }
    [JsonPropertyName("reference_et")]
    public string ReferenceET { get; set; }
    [JsonPropertyName("units")]
    public string Units { get; set; }
}