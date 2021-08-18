using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zybach.Models.DataTransferObjects;

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
            httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
            using var streamReader = new StreamReader(httpResponse.Content.ReadAsStreamAsync().Result);
            using var jsonTextReader = new JsonTextReader(streamReader);
            return new JsonSerializer().Deserialize<TV>(jsonTextReader);
        }

        public async Task<List<AgHubWellRaw>> GetWellCollection(string textToSearch)
        {
            var geoOptixSearchResults = await GetJsonFromCatalogImpl<List<AgHubWellRaw>>($"");
            return geoOptixSearchResults;
        }

        public class AgHubWellRaw
        {
            public string WellRegistrationID { get; set; }
            public Feature Location { get; set; }
            public string TpnrdPumpRateUpdated { get; set; }
            public double WellTpnrdPumpRate { get; set; }
            public string AuditPumpRateUpdated { get; set; }
            public double WellAuditPumpRate { get; set; }
            public bool WellConnectedMeter { get; set; }
            public string WellTPID { get; set; }
            public DateTime? FetchDate { get; set; }
            public bool HasElectricalData { get; set; }
        }
    }
}