using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Zybach.API.Services
{
    public class GeoOptixService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeoOptixService> _logger;

        public GeoOptixService(HttpClient httpClient, ILogger<GeoOptixService> logger)
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

        public async Task<List<WellSummaryDto>> GetWellSummaries()
        {
            const string uri = "project-overview-web/water-data-program/sites";
            var geoOptixSites = await GetJsonFromCatalogImpl<List<GeoOptixSite>>(uri);
            return geoOptixSites.AsParallel().Select(x => new WellSummaryDto(x)).ToList();
        }
    }

    public class WellSummaryDto
    {
        public WellSummaryDto()
        {
        }

        public string WellRegistrationID { get; set; }
        public string WellTPID { get; set; }
        public string Description { get; set; }
        public Feature Location { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public DateTime? FirstReadingDate { get; set; }
        public bool? InGeoOptix { get; set; }
        public DateTime? FetchDate { get; set; }
        public bool? HasElectricalData { get; set; }
        public List<IrrigatedAcresPerYearDto> IrrigatedAcresPerYear { get; set; }


        public WellSummaryDto(GeoOptixSite geoOptixSite)
        {
            WellRegistrationID = geoOptixSite.CanonicalName.ToUpper();
            Location = geoOptixSite.Location;
            Description = geoOptixSite.Description;
        }
    }

    public class IrrigatedAcresPerYearDto
    {
        public int Year { get; set; }
        public double Acres { get; set; }
    }

    public class GeoOptixSite
    {
        public string CanonicalName { get; set; }
        public string Description { get; set; }
        public Feature Location { get; set; }
    }
}