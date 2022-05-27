using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Zybach.API.Services
{
    public class AgHubService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AgHubService> _logger;

        public AgHubService(HttpClient httpClient, ILogger<AgHubService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        private async Task<TV> GetJsonFromCatalogImpl<TV>(string uri)
        {
            using var httpResponse = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            try
            {
                
                httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

                var readAsStringAsync = httpResponse.Content.ReadAsStringAsync().Result;

                using var streamReader = new StreamReader(httpResponse.Content.ReadAsStreamAsync().Result);
                using var jsonTextReader = new JsonTextReader(streamReader);
                return new JsonSerializer().Deserialize<TV>(jsonTextReader);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("HttpRequestException thrown when hitting this uri: " + uri);
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<List<AgHubWellRaw>> GetWellCollection()
        {
            var agHubWellResponse = await GetJsonFromCatalogImpl<AgHubWellResponse>($"prod/wells");
            return agHubWellResponse.Data;
        }

        public async Task<AgHubWellRawWithAcreYears> GetWellIrrigatedAcresPerYear(string wellRegistrationID)
        {
            try
            {
                var agHubWellResponse =
                    await GetJsonFromCatalogImpl<AgHubWellWithAcreYearsResponse>(
                        $"prod/wells/{wellRegistrationID}/summary-statistics");
                return agHubWellResponse.Code != 200 ? null : agHubWellResponse.Data;
            }
            catch
            {
                return null;
            }
        }

        public async Task<PumpedVolumeDaily> GetPumpedVolume(string wellRegistrationID, DateTime startDate)
        {
            try
            {
                var agHubWellResponse = await GetJsonFromCatalogImpl<PumpedVolumeDailyForWellResponse>($"prod/wells/{wellRegistrationID}/pumped-volume/daily-summary?startDateISO={FormatToYYMMDD(startDate)}&endDateISO={FormatToYYMMDD(DateTime.Today)}");
                return agHubWellResponse.Code != 200 ? null : agHubWellResponse.Data;
            }
            catch
            {
                return null;
            }
        }

        private static string FormatToYYMMDD(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public class AgHubWellResponse
        {
            public string Message { get; set; }
            public List<AgHubWellRaw> Data { get; set; }
            public int Code { get; set; }

            [JsonProperty("message_code")] public int MessageCode { get; set; }
        }

        public class AgHubWellRaw
        {
            public string WellRegistrationID { get; set; }
            public Point Location { get; set; }
            public DateTime? TpnrdPumpRateUpdated { get; set; }
            public int WellTpnrdPumpRate { get; set; }
            public DateTime? AuditPumpRateUpdated { get; set; }
            public int? WellAuditPumpRate { get; set; }
            public bool? WellConnectedMeter { get; set; }
            public string WellTPID { get; set; }

        }

        public class AgHubWellWithAcreYearsResponse
        {
            public string Message { get; set; }
            public AgHubWellRawWithAcreYears Data { get; set; }
            public int Code { get; set; }

            [JsonProperty("message_code")] public int MessageCode { get; set; }
        }


        public class AgHubWellRawWithAcreYears
        {
            public string WellRegistrationID { get; set; }
            public Point Location { get; set; }
            public DateTime? TpnrdPumpRateUpdated { get; set; }
            public int WellTpnrdPumpRate { get; set; }
            public DateTime? AuditPumpRateUpdated { get; set; }
            public int? WellAuditPumpRate { get; set; }
            public bool? WellConnectedMeter { get; set; }
            public string WellTPID { get; set; }
            [JsonProperty("electric")]
            public bool HasElectricalData { get; set; }
            [JsonProperty("irrigation_unit_geometry")]
            public string? IrrigationUnitGeometry { get; set; }
            public DateTime? RegisteredUpdated { get; set; }
            public int? RegisteredPumpRate { get; set; }
            public List<IrrigatedAcresPerYear> AcresYear { get; set; }

            public RegisteredUserDetails RegisteredUserDetails { get; set; }
        }

        public class RegisteredUserDetails
        {
            public string RegisteredUser { get; set; }
            public string RegisteredFieldName { get; set; }
        }

        public class PumpedVolumeDailyForWellResponse
        {
            public string Message { get; set; }
            public PumpedVolumeDaily Data { get; set; }
            public int Code { get; set; }

            [JsonProperty("message_code")] public int MessageCode { get; set; }
        }


        public class PumpedVolumeDaily
        {
            public int ReportingIntervalDays { get; set; }
            public List<PumpedVolumeTimePoint> PumpedVolumeTimeSeries { get; set; }
        }

        public class PumpedVolumeTimePoint
        {
            [JsonProperty("intervalDate")]
            public DateTime MeasurementDate { get; set; }
            public double PumpedVolumeGallons { get; set; }
        }

        public class IrrigatedAcresPerYear
        {
            public int Year { get; set; }
            public double? Acres { get; set; }
        }
    }
}