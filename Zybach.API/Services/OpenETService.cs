using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            var geometryArray = new[] { left, top, right, top, right, bottom, left, bottom };
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

        public string[] GetAllFilesReadyForExport()
        {
            var openETRequestURL = _zybachConfiguration.OpenETAllFilesReadyForExportRoute;

            try
            {
                var response = _httpClient.GetAsync(openETRequestURL).Result;

                var body = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new OpenETException(
                        $"Call to {openETRequestURL} was unsuccessful. Status code: ${response.StatusCode} Message: {body}");
                }

                var responseObject =
                    JsonConvert.DeserializeObject<ExportAllFilesResponse>(body);
                return responseObject.TimeseriesFilesReadyForExport;
            }
            catch (Exception ex)
            {
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, ex, "Error when attempting to get all files that are ready for export.");
                return null;
            }
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

            if (!IsOpenETAPIKeyValid())
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.PreconditionFailed,
                    Content = new StringContent(
                        "OpenET API Key is invalid or expired. Support has been notified and will work to remedy the situation shortly")
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
            [JsonProperty("bucket_url")]
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
        /// Check if OpenET has created data for a particular Year and Month evapotranspiration sync that has been triggered and update 
        /// </summary>
        public void UpdateAgHubIrrigationUnitMonthlyEvapotranspirationWithETData(int syncHistoryID, string[] filesReadyForExport,
            HttpClient httpClient)
        {
            var syncHistoryObject = OpenETSyncHistory.GetByOpenETSyncHistoryID(_zybachDbContext, syncHistoryID);

            if (syncHistoryObject == null || syncHistoryObject.OpenETSyncResultType.OpenETSyncResultTypeID !=
                (int)OpenETSyncResultTypeEnum.InProgress)
            {
                //Bad request, we completed already and somehow were called again, or someone else decided we were done
                return;
            }

            if (String.IsNullOrWhiteSpace(syncHistoryObject.GoogleBucketFileRetrievalURL))
            {
                //We are somehow storing sync histories without file retrieval urls, this is not good
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, new OpenETException(
                    $"OpenETSyncHistory record:{syncHistoryObject.OpenETSyncHistoryID} was saved without a file retrieval URL but we attempted to update with it. Check integration!"), "Error communicating with OpenET API.");
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.Failed, "Record was saved with a Google Bucket File Retrieval URL. Support has been notified.");
                return;
            }

            if (filesReadyForExport == null || !filesReadyForExport.Contains(syncHistoryObject.GoogleBucketFileRetrievalURL))
            {
                UpdateStatusAndFailIfOperationHasExceeded24Hours(_zybachDbContext, syncHistoryObject, "OpenET API never reported the results as available.");
                return;
            }

            var response = httpClient.GetAsync(syncHistoryObject.GoogleBucketFileRetrievalURL).Result;

            if (!response.IsSuccessStatusCode)
            {
                UpdateStatusAndFailIfOperationHasExceeded24Hours(_zybachDbContext, syncHistoryObject, response.Content.ReadAsStringAsync().Result);
                return;
            }

            try
            {
                List<OpenETGoogleBucketResponseEvapotranspirationDatum> distinctRecords;
                using (var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                {
                    var csvr = new CsvReader(reader, CultureInfo.CurrentCulture);
                    var finalizedWaterYearMonths = _zybachDbContext.WaterYearMonths
                        .Where(x => x.FinalizeDate.HasValue)
                        .Select(x => new DateTime(x.Year, x.Month, 1))
                        .ToList();
                    csvr.Context.RegisterClassMap(
                        new OpenETEvapCSVFormatMap(_zybachConfiguration.OpenETRasterTimeseriesMultipolygonColumnToUseAsIdentifier));
                    //Sometimes the results will produce exact duplicates, so we need to filter those out
                    //Also one final check to make sure we don't get any finalized dates
                    distinctRecords = csvr.GetRecords<OpenETEvapCSVFormat>().Where(x => !finalizedWaterYearMonths.Contains(x.Date))
                        .Distinct(new DistinctOpenETEvapCSVFormatComparer())
                        .Select(x => x.AsOpenETGoogleBucketResponseEvapotranspirationDatum())
                        .ToList();
                }

                //This shouldn't happen, but if we enter here we've attempted to grab data for a water year that was finalized
                if (!distinctRecords.Any())
                {
                    OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.NoNewData);
                    return;
                }

                _zybachDbContext.Database.ExecuteSqlRaw(
                    "TRUNCATE TABLE dbo.OpenETGoogleBucketResponseEvapotranspirationDatum");
                DataTable table = new DataTable();
                table.Columns.Add("OpenETGoogleBucketResponseEvapotranspirationDatumID", typeof(int));
                table.Columns.Add("WellTPID", typeof(string));
                table.Columns.Add("WaterMonth", typeof(int));
                table.Columns.Add("WaterYear", typeof(int));
                table.Columns.Add("EvapotranspirationInches", typeof(decimal));

                var index = 0;
                distinctRecords.ForEach(x =>
                {
                    table.Rows.Add(++index, x.WellTPID, x.WaterMonth, x.WaterYear, x.EvapotranspirationInches);
                });

                using (SqlConnection con = new SqlConnection(_zybachConfiguration.DB_CONNECTION_STRING))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "dbo.OpenETGoogleBucketResponseEvapotranspirationDatum";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }

                _zybachDbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pUpdateAgHubIrrigationUnitMonthlyEvapotranspirationWithETData");

                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.Succeeded);
            }
            catch (Exception ex)
            {
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, ex, "Error parsing file from OpenET or getting records into database.");
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.Failed, ex.Message);
            }
        }

        /// <summary>
        /// Check if OpenET has created data for a particular Year and Month precipitation sync that has been triggered and update
        /// </summary>
        public void UpdateAgHubIrrigationUnitMonthlyPrecipitationWithETData(int syncHistoryID, string[] filesReadyForExport,
            HttpClient httpClient)
        {
            var syncHistoryObject = OpenETSyncHistory.GetByOpenETSyncHistoryID(_zybachDbContext, syncHistoryID);

            if (syncHistoryObject == null || syncHistoryObject.OpenETSyncResultType.OpenETSyncResultTypeID !=
                (int)OpenETSyncResultTypeEnum.InProgress)
            {
                //Bad request, we completed already and somehow were called again, or someone else decided we were done
                return;
            }

            if (String.IsNullOrWhiteSpace(syncHistoryObject.GoogleBucketFileRetrievalURL))
            {
                //We are somehow storing sync histories without file retrieval urls, this is not good
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, new OpenETException(
                    $"OpenETSyncHistory record:{syncHistoryObject.OpenETSyncHistoryID} was saved without a file retrieval URL but we attempted to update with it. Check integration!"), "Error communicating with OpenET API.");
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.Failed, "Record was saved with a Google Bucket File Retrieval URL. Support has been notified.");
                return;
            }

            if (filesReadyForExport == null || !filesReadyForExport.Contains(syncHistoryObject.GoogleBucketFileRetrievalURL))
            {
                UpdateStatusAndFailIfOperationHasExceeded24Hours(_zybachDbContext, syncHistoryObject, "OpenET API never reported the results as available.");
                return;
            }

            var response = httpClient.GetAsync(syncHistoryObject.GoogleBucketFileRetrievalURL).Result;

            if (!response.IsSuccessStatusCode)
            {
                UpdateStatusAndFailIfOperationHasExceeded24Hours(_zybachDbContext, syncHistoryObject, response.Content.ReadAsStringAsync().Result);
                return;
            }

            try
            {
                List<OpenETGoogleBucketResponsePrecipitationDatum> distinctRecords;
                using (var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                {
                    var csvr = new CsvReader(reader, CultureInfo.CurrentCulture);
                    var finalizedWaterYearMonths = _zybachDbContext.WaterYearMonths
                        .Where(x => x.FinalizeDate.HasValue)
                        .Select(x => new DateTime(x.Year, x.Month, 1))
                        .ToList();
                    csvr.Context.RegisterClassMap(
                        new OpenETPrecipCSVFormatMap(_zybachConfiguration.OpenETRasterTimeseriesMultipolygonColumnToUseAsIdentifier));
                    //Sometimes the results will produce exact duplicates, so we need to filter those out
                    //Also one final check to make sure we don't get any finalized dates
                    distinctRecords = csvr.GetRecords<OpenETPrecipCSVFormat>().Where(x => !finalizedWaterYearMonths.Contains(x.Date))
                        .Distinct(new DistinctOpenETPrecipCSVFormatComparer())
                        .Select(x => x.AsOpenETGoogleBucketResponsePrecipitationDatum())
                        .ToList();
                }

                //This shouldn't happen, but if we enter here we've attempted to grab data for a water year that was finalized
                if (!distinctRecords.Any())
                {
                    OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID, OpenETSyncResultTypeEnum.NoNewData);
                    return;
                }

                _zybachDbContext.Database.ExecuteSqlRaw(
                    "TRUNCATE TABLE dbo.OpenETGoogleBucketResponsePrecipitationDatum");
                DataTable table = new DataTable();
                table.Columns.Add("OpenETGoogleBucketResponsePrecipitationDatumID", typeof(int));
                table.Columns.Add("WellTPID", typeof(string));
                table.Columns.Add("WaterMonth", typeof(int));
                table.Columns.Add("WaterYear", typeof(int));
                table.Columns.Add("PrecipitationInches", typeof(decimal));

                var index = 0;
                distinctRecords.ForEach(x =>
                {
                    table.Rows.Add(++index, x.WellTPID, x.WaterMonth, x.WaterYear, x.PrecipitationInches);
                });

                using (SqlConnection con = new SqlConnection(_zybachConfiguration.DB_CONNECTION_STRING))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "dbo.OpenETGoogleBucketResponsePrecipitationDatum";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }

                _zybachDbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pUpdateAgHubIrrigationUnitMonthlyPrecipitationWithETData");

                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.Succeeded);
            }
            catch (Exception ex)
            {
                TelemetryHelper.LogCaughtException(_logger, LogLevel.Critical, ex, "Error parsing file from OpenET or getting records into database.");
                OpenETSyncHistory.UpdateOpenETSyncEntityByID(_zybachDbContext, syncHistoryObject.OpenETSyncHistoryID,
                    OpenETSyncResultTypeEnum.Failed, ex.Message);
            }
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
        string[] GetAllFilesReadyForExport();
        HttpResponseMessage TriggerOpenETGoogleBucketRefresh(int waterYearMonthID, OpenETDataType openETDataType);
        void UpdateAgHubIrrigationUnitMonthlyEvapotranspirationWithETData(int syncHistoryID, string[] filesReadyForExport,
            HttpClient httpClient);
        void UpdateAgHubIrrigationUnitMonthlyPrecipitationWithETData(int syncHistoryID, string[] filesReadyForExport,
            HttpClient httpClient);
        bool IsOpenETAPIKeyValid();
    }

    public class OpenETEvapCSVFormat
    {
        public string WellTPID { get; set; }
        public DateTime Date { get; set; }
        public decimal EvapotranspirationInches { get; set; }
    }

    public class OpenETPrecipCSVFormat
    {
        public string WellTPID { get; set; }
        public DateTime Date { get; set; }
        public decimal PrecipitationInches { get; set; }
    }

    public class OpenETEvapCSVFormatMap : ClassMap<OpenETEvapCSVFormat>
    {
        public OpenETEvapCSVFormatMap(string irrigationUnitTPIDColumnName)
        {
            Map(m => m.WellTPID).Name(irrigationUnitTPIDColumnName);
            Map(m => m.Date).Name("time");
            Map(m => m.EvapotranspirationInches).Name("et_mean");
        }
    }

    public class OpenETPrecipCSVFormatMap : ClassMap<OpenETPrecipCSVFormat>
    {
        public OpenETPrecipCSVFormatMap(string irrigationUnitTPIDColumnName)
        {
            Map(m => m.WellTPID).Name(irrigationUnitTPIDColumnName);
            Map(m => m.Date).Name("time");
            Map(m => m.PrecipitationInches).Name("pr_mean");
        }
    }

    public class DistinctOpenETEvapCSVFormatComparer : IEqualityComparer<OpenETEvapCSVFormat>
    {

        public bool Equals(OpenETEvapCSVFormat x, OpenETEvapCSVFormat y)
        {
            return x.WellTPID == y.WellTPID &&
                   x.Date == y.Date &&
                   x.EvapotranspirationInches == y.EvapotranspirationInches;
        }

        public int GetHashCode(OpenETEvapCSVFormat obj)
        {
            return obj.WellTPID.GetHashCode() ^
                   obj.Date.GetHashCode() ^
                   obj.EvapotranspirationInches.GetHashCode();
        }
    }

    public class DistinctOpenETPrecipCSVFormatComparer : IEqualityComparer<OpenETPrecipCSVFormat>
    {

        public bool Equals(OpenETPrecipCSVFormat x, OpenETPrecipCSVFormat y)
        {
            return x.WellTPID == y.WellTPID &&
                   x.Date == y.Date &&
                   x.PrecipitationInches == y.PrecipitationInches;
        }

        public int GetHashCode(OpenETPrecipCSVFormat obj)
        {
            return obj.WellTPID.GetHashCode() ^
                   obj.Date.GetHashCode() ^
                   obj.PrecipitationInches.GetHashCode();
        }
    }

    public static class OpenETCSVFormatExtensionMethods
    {
        public static OpenETGoogleBucketResponseEvapotranspirationDatum AsOpenETGoogleBucketResponseEvapotranspirationDatum(this OpenETEvapCSVFormat openEtEvapCsvFormat)
        {
            return new OpenETGoogleBucketResponseEvapotranspirationDatum()
            {
                WellTPID = openEtEvapCsvFormat.WellTPID,
                WaterYear = openEtEvapCsvFormat.Date.Year,
                WaterMonth = openEtEvapCsvFormat.Date.Month,
                EvapotranspirationInches = openEtEvapCsvFormat.EvapotranspirationInches
            };
        }

        public static OpenETGoogleBucketResponsePrecipitationDatum AsOpenETGoogleBucketResponsePrecipitationDatum(this OpenETPrecipCSVFormat openEtPrecipCsvFormat)
        {
            return new OpenETGoogleBucketResponsePrecipitationDatum()
            {
                WellTPID = openEtPrecipCsvFormat.WellTPID,
                WaterYear = openEtPrecipCsvFormat.Date.Year,
                WaterMonth = openEtPrecipCsvFormat.Date.Month,
                PrecipitationInches = openEtPrecipCsvFormat.PrecipitationInches
            };
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
