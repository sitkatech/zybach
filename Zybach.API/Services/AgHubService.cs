using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Zybach.API.Services
{
    public class AgHubService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AgHubService> _logger;
        private readonly ZybachConfiguration _zybachConfiguration;
        private readonly string _apiBucket;

        public AgHubService(HttpClient httpClient, ILogger<AgHubService> logger, IOptions<ZybachConfiguration> zybachConfiguration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _zybachConfiguration = zybachConfiguration.Value;
            _apiBucket = _zybachConfiguration.AGHUB_API_BUCKET;
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
            var agHubWellResponse = await GetJsonFromCatalogImpl<AgHubWellResponse>($"{_apiBucket}/wells");
            return agHubWellResponse.Data;
        }

        public async Task<AgHubWellRawWithAcreYears> GetWellIrrigatedAcresPerYear(string wellRegistrationID)
        {
            try
            {
                var agHubWellResponse =
                    await GetJsonFromCatalogImpl<AgHubWellWithAcreYearsResponse>(
                        $"{_apiBucket}/wells/{wellRegistrationID}/summary-statistics");
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
                var agHubWellResponse = await GetJsonFromCatalogImpl<PumpedVolumeDailyForWellResponse>($"{_apiBucket}/wells/{wellRegistrationID}/pumped-volume/daily-summary?startDateISO={FormatToYYMMDD(startDate)}&endDateISO={FormatToYYMMDD(DateTime.Today)}");
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
            public string Location { get; set; }
            public DateTime? DistrictPumpRateUpdated { get; set; }
            public int WellDistrictPumpRate { get; set; }
            public DateTime? AuditPumpRateUpdated { get; set; }
            public int? WellAuditPumpRate { get; set; }
            public bool? WellConnectedMeter { get; set; }
            public string WellIrrigUnitID { get; set; }

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
            public string Location { get; set; }
            public DateTime? DistrictPumpRateUpdated { get; set; }
            public int WellDistrictPumpRate { get; set; }
            public DateTime? AuditPumpRateUpdated { get; set; }
            public int? WellAuditPumpRate { get; set; }
            public bool? WellConnectedMeter { get; set; }
            public string WellIrrigUnitID { get; set; }
            [JsonProperty("electric")]
            public int HasElectricalData { get; set; }
            public string IrrigationUnitGeometry { get; set; }
            public DateTime? RegisteredUpdated { get; set; }
            public int? RegisteredPumpRate { get; set; }
            public List<IrrigationUnitDetailsPerYear> IrrigUnitDetails { get; set; }

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
            public bool IsElectricSource { get; set; }
        }

        public class IrrigationUnitDetailsPerYear
        {
            public int Year { get; set; }
            public double? TotalAcres { get; set; }
            public List<FarmPractice> FarmPractices { get; set; }
        }

        public class FarmPractice
        {
            public string Crop { get; set; }
            public string Tillage { get; set; }
            public double? Acres { get; set; }
        }
    }
}