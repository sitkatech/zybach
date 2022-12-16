using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Operation.Buffer;
using NetTopologySuite.Utilities;
using Newtonsoft.Json;
using Zybach.API.Controllers;
using Zybach.API.Services.Telemetry;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services
{
    public class OpenETService : IOpenETService
    {
        private readonly ILogger<OpenETService> _logger;
        private readonly ZybachConfiguration _zybachConfiguration;
        private readonly ZybachDbContext _zybachDbContext;
        private readonly HttpClient _httpClient;

        private readonly string[] _rebuildingModelResultsErrorMessages =
            {"Expecting value: line 1 column 1 (char 0)"};

        public OpenETService(ILogger<OpenETService> logger, IOptions<ZybachConfiguration> zybachConfiguration, ZybachDbContext zybachDbContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _zybachConfiguration = zybachConfiguration.Value;
            _zybachDbContext = zybachDbContext;
            _httpClient = httpClientFactory.CreateClient("OpenETClient");
        }

        private bool RasterUpdatedSinceMinimumLastUpdatedDate(int month, int year, OpenETSyncHistoryDto newSyncHistory)
        {
            var openETRequestURL = _zybachConfiguration.OpenETRasterMetadataRoute;
            var top = _zybachConfiguration.DefaultBoundingBoxTop;
            var bottom = _zybachConfiguration.DefaultBoundingBoxBottom;
            var left = _zybachConfiguration.DefaultBoundingBoxLeft;
            var right = _zybachConfiguration.DefaultBoundingBoxRight;
            var polygon = new WKTReader().Read(@$"POLYGON (({left} {top},
 {right} {top},
 {right} {bottom},
 {left} {bottom},
 {left} {top}))");
            var centerBufferedBy5000SurveyFeet = polygon.Centroid.ProjectTo26860().Buffer(16000, EndCapStyle.Square).ProjectTo4326();
            var envelope = centerBufferedBy5000SurveyFeet.EnvelopeInternal;
            var geometryArray = new[]
            {
                envelope.MinX.ToString(), envelope.MaxY.ToString(), 
                envelope.MaxX.ToString(), envelope.MaxY.ToString(), 
                envelope.MaxX.ToString(), envelope.MinY.ToString(), 
                envelope.MinX.ToString(), envelope.MinY.ToString()
            };
            var argumentsObject = new OpenETRasterMetadataPostRequestBody("ensemble", "et", year, month, "gridmet", true, geometryArray, "monthly");

            try
            {
                var response = _httpClient.PostAsync(_zybachConfiguration.OpenETRasterMetadataRoute, new StringContent(JsonConvert.SerializeObject(argumentsObject), Encoding.UTF8, "application/json")).Result;

                var body = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    //We want to deserialize this separately and not throw an error if it doesn't work.
                    //We're more concerned about if there is a helpful message in the potentially returned object, and if there isn't just return the body and throw a full-fledged exception.
                    var unsuccessfulObject =
                        JsonConvert.DeserializeObject<RasterMetadataDateIngested>(body, new JsonSerializerSettings
                        {
                            Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                            {
                                args.ErrorContext.Handled = true;
                            }
                        });

                    if (unsuccessfulObject != null && !String.IsNullOrWhiteSpace(unsuccessfulObject.Description) &&
                        _rebuildingModelResultsErrorMessages.Contains(unsuccessfulObject.Description))
                    {
                        OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, newSyncHistory.OpenETSyncHistoryID,
                            OpenETSyncResultTypeEnum.Failed, "OpenET is currently rebuilding collections that include this date. Please try again later.");
                        return false;
                    }
                    throw new OpenETException($"Call to {openETRequestURL} was unsuccessful. Status Code: {response.StatusCode} Message: {body}");
                }

                var responseObject =
                    JsonConvert.DeserializeObject<RasterMetadataDateIngested>(body);

                if (string.IsNullOrEmpty(responseObject.DateIngested) ||
                    !DateTime.TryParse(responseObject.DateIngested, out DateTime responseDate))
                {
                    OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, newSyncHistory.OpenETSyncHistoryID,
                        OpenETSyncResultTypeEnum.DataNotAvailable);
                    return false;
                }

                var openETSyncHistoriesThatWereSuccessful = _zybachDbContext.OpenETSyncHistories
                    .Include(x => x.WaterYearMonth)
                    .Where(x => x.WaterYearMonth.Year == year && x.WaterYearMonth.Month == month &&
                                x.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.Succeeded);

                if (!openETSyncHistoriesThatWereSuccessful.Any())
                {
                    return true;
                }

                var mostRecentSyncHistory =
                    openETSyncHistoriesThatWereSuccessful.OrderByDescending(x => x.UpdateDate).First();

                if (responseDate > mostRecentSyncHistory.UpdateDate)
                {
                    return true;
                }

                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, newSyncHistory.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.NoNewData);
                return false;
            }
            catch (TaskCanceledException ex)
            {
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, newSyncHistory.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.Failed, "OpenET API did not respond");
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, ex, "Error communicating with OpenET API.");
                return false;
            }
            catch (Exception ex)
            {
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, ex, "Error when attempting to check raster metadata date ingested.");
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, newSyncHistory.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.Failed, ex.Message);
                return false;
            }
        }

        public class RasterMetadataDateIngested : OpenETGeneralJsonResponse
        {
            [JsonProperty("date_ingested")]
            public string DateIngested { get; set; }
        }

        public class OpenETGeneralJsonResponse
        {
            [JsonProperty("ERROR")]
            public string ErrorMessage { get; set; }
            [JsonProperty("SOLUTION")]
            public string SuggestedSolution { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("type")]
            public string ResponseType { get; set; }
        }

        public class ExportAllFilesResponse
        {
            [JsonProperty("timeseries")]
            public string[] TimeseriesFilesReadyForExport { get; set; }
        }


        public HttpResponseMessage TriggerOpenETGoogleBucketRefresh(int waterYearMonthID, OpenETDataType openETDataType)
        {
            if (!_zybachConfiguration.AllowOpenETSync)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Syncing with OpenET is not enabled at this time")
                };
            }

            var waterYearMonthDto = WaterYearMonths.GetByWaterYearMonthID(_zybachDbContext, waterYearMonthID);

            if (waterYearMonthDto == null)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Requested Water Year and Month not found")
                };
            }

            var year = waterYearMonthDto.Year;
            var month = waterYearMonthDto.Month;
            var monthNameToDisplay = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

            if (_zybachDbContext.OpenETSyncHistories
                .Any(x => x.WaterYearMonthID == waterYearMonthID && 
                          x.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.InProgress &&
                          x.OpenETDataTypeID == openETDataType.OpenETDataTypeID))
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent($"Sync already in progress for {monthNameToDisplay} {year}")
                };
            }

            var newSyncHistory = OpenETSyncHistory.CreateNew(_zybachDbContext, waterYearMonthID, openETDataType.OpenETDataTypeID);

            if (!RasterUpdatedSinceMinimumLastUpdatedDate(month, year, newSyncHistory))
            {
                newSyncHistory =
                    OpenETSyncHistory.GetByOpenETSyncHistoryID(_zybachDbContext, newSyncHistory.OpenETSyncHistoryID);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Content = new StringContent(
                        $"The sync for {monthNameToDisplay} {year} will not be completed for the following reason: {newSyncHistory.OpenETSyncResultType.OpenETSyncResultTypeDisplayName}.{(newSyncHistory.OpenETSyncResultType.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.Failed ? $" Error Message:{newSyncHistory.ErrorMessage}" : "")}")
                };
            }
            Thread.Sleep(1000); // intentional sleep here to avoid maximum rate limit message

            var openETRequestURL = $"{_zybachConfiguration.OpenETRasterTimeSeriesMultipolygonRoute}?shapefile_asset_id={_zybachConfiguration.OPENET_SHAPEFILE_PATH}&start_date={new DateTime(year, month, 1):yyyy-MM-dd}&end_date={new DateTime(year, month, DateTime.DaysInMonth(year, month)):yyyy-MM-dd}&model=ensemble&variable={openETDataType.OpenETDataTypeVariableName}&units=english&output_date_format=standard&ref_et_source=gridmet&filename_suffix={$"TPNRD_{month}_{year}_public{openETDataType.OpenETDataTypeName}"}&include_columns={_zybachConfiguration.OpenETRasterTimeseriesMultipolygonColumnToUseAsIdentifier}&provisional=true&interval=monthly";

            try
            {
                var response = _httpClient.GetAsync(openETRequestURL).Result;

                var body = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new OpenETException(
                        $"Call to {openETRequestURL} failed. Status Code: {response.StatusCode} Message: {body}");
                }

                var responseObject =
                    JsonConvert.DeserializeObject<TimeseriesMultipolygonSuccessfulResponse>(body);

                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, newSyncHistory.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.InProgress, null, responseObject.FileRetrievalURL);

                return response;
            }
            catch (TaskCanceledException ex)
            {
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, newSyncHistory.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.Failed, "OpenET API did not respond");
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, ex, "Error communicating with OpenET API.");
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(
                        $"The OpenET API did not respond. The error has been logged and support has been notified.")
                };
            }
            catch (Exception ex)
            {
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, newSyncHistory.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.Failed, ex.Message);
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, ex, "Error communicating with OpenET API.");
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
            [JsonProperty("destination")]
            public string FileRetrievalURL { get; set; }
        }

        private void UpdateStatusAndFailIfOperationHasExceeded24Hours(ZybachDbContext zybachDbContext, OpenETSyncHistoryDto syncHistory, string errorMessage)
        {
            var timeBetweenSyncCreationAndNow = DateTime.UtcNow.Subtract(syncHistory.CreateDate).TotalHours;
            var resultType = timeBetweenSyncCreationAndNow > 24
                ? OpenETSyncResultTypeEnum.Failed
                : OpenETSyncResultTypeEnum.InProgress;

            //One very unfortunate thing about OpenET's design is that they're forced to create a queue of requests and can't process multiple requests at once.
            //That, combined with no (at this moment 7/14/21) means of knowing whether or not a run has completed or failed other than checking to see if the file is ready for export means we have to implement some kind of terminal state.
            OpenETSyncHistory.UpdateOpenETSyncEntityByID(zybachDbContext, syncHistory.OpenETSyncHistoryID, resultType, resultType == OpenETSyncResultTypeEnum.Failed ? errorMessage : null);
        }

        /// <summary>
        /// Check if OpenET has created data for a particular Year and Month precipitation sync that has been triggered and update
        /// </summary>
        public void ProcessOpenETData(int syncHistoryID, HttpClient httpClient, OpenETDataTypeEnum openEtDataTypeEnum)
        {
            var syncHistoryObject = OpenETSyncHistory.GetByOpenETSyncHistoryID(_zybachDbContext, syncHistoryID);

            if (syncHistoryObject == null || syncHistoryObject.OpenETSyncResultType.OpenETSyncResultTypeID !=
                (int)OpenETSyncResultTypeEnum.InProgress)
            {
                //Bad request, we completed already and somehow were called again, or someone else decided we were done
                return;
            }

            if (string.IsNullOrWhiteSpace(syncHistoryObject.GoogleBucketFileRetrievalURL))
            {
                //We are somehow storing sync histories without file retrieval urls, this is not good
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, new OpenETException(
                    $"OpenETSyncHistory record:{syncHistoryObject.OpenETSyncHistoryID} was saved without a file retrieval URL but we attempted to update with it. Check integration!"), "Error communicating with OpenET API.");
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.Failed, "Record was saved with a Google Bucket File Retrieval URL. Support has been notified.");
                return;
            }

            var response = httpClient.GetAsync(syncHistoryObject.GoogleBucketFileRetrievalURL).Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.StatusCode == HttpStatusCode.NotFound ? "OpenET API never reported the results as available." : response.Content.ReadAsStringAsync().Result;
                UpdateStatusAndFailIfOperationHasExceeded24Hours(_zybachDbContext, syncHistoryObject, errorMessage);
                return;
            }

            try
            {
                List<OpenETCSVFormat> distinctRecords;
                using (var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                {
                    var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture);
                    var finalizedWaterYearMonths = _zybachDbContext.WaterYearMonths
                        .Where(x => x.FinalizeDate.HasValue)
                        .Select(x => new DateTime(x.Year, x.Month, 1))
                        .ToList();
                    csvReader.Context.RegisterClassMap(new OpenETCSVFormatMap(_zybachConfiguration.OpenETRasterTimeseriesMultipolygonColumnToUseAsIdentifier));
                    //Sometimes the results will produce exact duplicates, so we need to filter those out
                    //Also one final check to make sure we don't get any finalized dates
                    distinctRecords = csvReader.GetRecords<OpenETCSVFormat>()
                        .Where(x => !finalizedWaterYearMonths.Contains(x.Date))
                        .Distinct(new DistinctOpenETCSVFormatComparer())
                        .ToList();
                }

                //This shouldn't happen, but if we enter here we've attempted to grab data for a water year that was finalized
                if (!distinctRecords.Any())
                {
                    OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.NoNewData);
                }
                else
                {

                    switch (openEtDataTypeEnum)
                    {
                        case OpenETDataTypeEnum.Evapotranspiration:
                            BulkCopyToTable(distinctRecords, "OpenETGoogleBucketResponseEvapotranspirationDatum", "EvapotranspirationInches");
                            _zybachDbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pUpdateAgHubIrrigationUnitMonthlyEvapotranspirationWithETData");
                            break;
                        case OpenETDataTypeEnum.Precipitation:
                            BulkCopyToTable(distinctRecords, "OpenETGoogleBucketResponsePrecipitationDatum", "PrecipitationInches");
                            _zybachDbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pUpdateAgHubIrrigationUnitMonthlyPrecipitationWithETData");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(openEtDataTypeEnum), openEtDataTypeEnum, $"Invalid OpenETDataType {openEtDataTypeEnum}");
                    }

                    OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext,
                        syncHistoryObject.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.Succeeded);
                }
            }
            catch (Exception ex)
            {
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, ex, "Error parsing file from OpenET or getting records into database.");
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.Failed, ex.Message);
            }
        }

        private void BulkCopyToTable(List<OpenETCSVFormat> distinctRecords, string destinationTableName, string measurementColumnName)
        {
            var tableNameWithSchema = $"dbo.{destinationTableName}";
            _zybachDbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE {tableNameWithSchema}");
            var table = new DataTable();
            table.Columns.Add($"{destinationTableName}ID", typeof(int));
            table.Columns.Add("WellTPID", typeof(string));
            table.Columns.Add("WaterMonth", typeof(int));
            table.Columns.Add("WaterYear", typeof(int));
            table.Columns.Add(measurementColumnName, typeof(decimal));

            var index = 0;
            distinctRecords.ForEach(x =>
            {
                table.Rows.Add(++index, x.WellTPID, x.Date.Month, x.Date.Year, x.ValueInInches);
            });

            using var con = new SqlConnection(_zybachConfiguration.DB_CONNECTION_STRING);
            using var sqlBulkCopy = new SqlBulkCopy(con);
            sqlBulkCopy.DestinationTableName = tableNameWithSchema;
            con.Open();
            sqlBulkCopy.WriteToServer(table);
            con.Close();
        }

        public bool IsOpenETAPIKeyValid()
        {
            var openETRequestURL = "home/key_expiration";
            try
            {
                var response = _httpClient.GetAsync(openETRequestURL).Result;

                var body = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new OpenETException(
                        $"Call to {openETRequestURL} was unsuccessful. Status Code: {response.StatusCode} Message: {body}.");
                }

                var responseObject =
                    JsonConvert.DeserializeObject<OpenETController.OpenETTokenExpirationDate>(body);

                if (responseObject == null || responseObject.ExpirationDate < DateTime.UtcNow)
                {
                    throw new OpenETException($"Deserializing OpenET API Key validation response failed, or the key is expired. Expiration Date: {(responseObject?.ExpirationDate != null ? responseObject.ExpirationDate.ToString(CultureInfo.InvariantCulture) : "Not provided")}");
                }

                return true;
            }
            catch (Exception ex)
            {
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, ex, "Error validating OpenET API Key.");
                return false;
            }
        }
    }

    public interface IOpenETService
    {
        HttpResponseMessage TriggerOpenETGoogleBucketRefresh(int waterYearMonthID, OpenETDataType openETDataType);
        void ProcessOpenETData(int syncHistoryID, HttpClient httpClient, OpenETDataTypeEnum openEtDataTypeEnum);
        bool IsOpenETAPIKeyValid();
    }

    public class OpenETCSVFormat
    {
        public string WellTPID { get; set; }
        public DateTime Date { get; set; }
        public decimal ValueInInches { get; set; }
    }

    public class OpenETCSVFormatMap : ClassMap<OpenETCSVFormat>
    {
        public OpenETCSVFormatMap(string irrigationUnitTPIDColumnName)
        {
            Map(m => m.WellTPID).Name(irrigationUnitTPIDColumnName);
            Map(m => m.Date).Name("time");
            Map(m => m.ValueInInches).Name("mean");
        }
    }

    public class DistinctOpenETCSVFormatComparer : IEqualityComparer<OpenETCSVFormat>
    {

        public bool Equals(OpenETCSVFormat x, OpenETCSVFormat y)
        {
            return x.WellTPID == y.WellTPID &&
                   x.Date == y.Date &&
                   x.ValueInInches == y.ValueInInches;
        }

        public int GetHashCode(OpenETCSVFormat obj)
        {
            return obj.WellTPID.GetHashCode() ^
                   obj.Date.GetHashCode() ^
                   obj.ValueInInches.GetHashCode();
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
        public OpenETRasterMetadataPostRequestBody(string model, string variable, int year, int month, string refETSource, bool provisional, string[] geometry, string interval)
        {
            Model = model;
            Variable = variable;
            StartDate = $"{new DateTime(year, month, 1):yyyy-MM-dd}";
            EndDate = $"{new DateTime(year, month, DateTime.DaysInMonth(year, month)):yyyy-MM-dd}";
            RefETSource = refETSource;
            Provisional = provisional;
            Geometry = geometry;
            Interval = interval;
        }
        [JsonProperty("interval")]
        public string Interval { get; set; }
        [JsonProperty("geometry")]
        public string[] Geometry { get; set; }
        [JsonProperty("provisional")]
        public bool Provisional { get; set; }
        [JsonProperty("ref_et_source")]
        public string RefETSource { get; set; }
        [JsonProperty("end_date")]
        public string EndDate { get; set; }
        [JsonProperty("start_date")]
        public string StartDate { get; set; }
        [JsonProperty("variable")]
        public string Variable { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
    }
}
