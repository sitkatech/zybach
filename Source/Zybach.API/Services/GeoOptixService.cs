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
            var geoOptixSites = await GetGeoOptixSites();
            return geoOptixSites.AsParallel().Select(x => new WellSummaryDto(x)).ToList();
        }

        public async Task<List<WellSummaryDto>> GetWellSummariesCreatedAsOfYear(int year)
        {
            var geoOptixSites = await GetGeoOptixSites();
            return geoOptixSites.Where(x => x.CreateDate.Year <= year).AsParallel().Select(x => new WellSummaryDto(x)).ToList();
        }

        private async Task<List<GeoOptixSite>> GetGeoOptixSites()
        {
            const string uri = "project-overview-web/water-data-program/sites";
            var geoOptixSites = await GetJsonFromCatalogImpl<List<GeoOptixSite>>(uri);
            return geoOptixSites;
        }

        public async Task<List<SensorSummaryDto>> GetSensorSummaries()
        {
            var geoOptixStations = await GetGeoOptixStations();
            return geoOptixStations.Select(x => new SensorSummaryDto(x)).ToList();
        }

        public async Task<List<SensorSummaryDto>> GetSensorSummariesCreatedAsOfYear(int year)
        {
            var geoOptixStations = await GetGeoOptixStations();
            return geoOptixStations.Where(x => x.CreateDate.Year <= year).AsParallel().Select(x => new SensorSummaryDto(x)).ToList();
        }

        private async Task<List<GeoOptixStation>> GetGeoOptixStations()
        {
            const string uri = "project-overview-web/water-data-program/stations";
            var geoOptixStations = await GetJsonFromCatalogImpl<List<GeoOptixStation>>(uri);
            return geoOptixStations;
        }

        public async Task<Dictionary<string, WellWithSensorSummaryDto>> GetWellsWithSensors()
        {
            var sensors = await GetSensorSummaries();
            var wells = await GetWellSummaries();

            // add a sensor array to the wells
            var wellsWithSensors = wells.Select(x => new WellWithSensorSummaryDto
            {
                WellRegistrationID = x.WellRegistrationID,
                Description = x.Description,
                Location = x.Location,
                Sensors = sensors.Where(y => y.WellRegistrationID == x.WellRegistrationID).ToList(),
                InGeoOptix = true
            }).ToList();

            // create a Map from the array of wells
            return wellsWithSensors.ToDictionary(x => x.WellRegistrationID);
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

    public class SensorSummaryDto
    {
        private static readonly Dictionary<string, string> SensorTypeMap = new Dictionary<string, string>{{"FlowMeter", "Flow Meter"}, {"PumpMonitor", "Continuity Meter"}, {"WellPressure", "Well Pressure"}};

        public SensorSummaryDto()
        {
        }

        public string WellRegistrationID { get; set; }
        public string SensorName { get; set; }
        public string SensorType { get; set; }

        public SensorSummaryDto(GeoOptixStation geoOptixStation)
        {
            WellRegistrationID = geoOptixStation.SiteCanonicalName.ToUpper();
            SensorName = geoOptixStation.Name;
            SensorType =  SensorTypeMap[geoOptixStation.Definition.SensorType];
        }
    }

    public class WellWithSensorSummaryDto : WellSummaryDto
    {
        public List<SensorSummaryDto> Sensors { get; set; }
    }

    public class WellWithSensorMessageAgeDto : WellSummaryDto
    {
        public List<SensorMessageAgeDto> Sensors { get; set; }
    }

    public class SensorMessageAgeDto
    {
        public string SensorName { get; set; }
        public int? MessageAge { get; set; }
        public string SensorType { get; set; }
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
        public DateTime CreateDate { get; set; }
    }

    public class GeoOptixStation
    {
        public string SiteCanonicalName { get; set; }
        public string Name { get; set; }
        public GeoOptixDefinition Definition { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class GeoOptixDefinition
    {
        public string SensorType { get; set; }
    }
}