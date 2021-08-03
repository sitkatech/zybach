using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zybach.Models.DataTransferObjects;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Zybach.API.Services
{
    public class GeoOptixService
    {
        private const string GeoOptixSitesUri = "project-overview-web/water-data-program/sites";

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

        public async Task<AbbreviatedWellDataResponse> GetWellAsAbbreviatedWellDataResponse(string wellRegistrationID)
        {
            var geoOptixSite = await GetGeoOptixSite(wellRegistrationID);
            var geoOptixStations = await GetGeoOptixStationsForSite(wellRegistrationID);
            return new AbbreviatedWellDataResponse(geoOptixSite, geoOptixStations);
        }

        public async Task<WellSummaryDto> GetWellSummary(string wellRegistrationID)
        {
            var geoOptixSite = await GetGeoOptixSite(wellRegistrationID);
            return new WellSummaryDto(geoOptixSite);
        }

        private async Task<List<GeoOptixSite>> GetGeoOptixSites()
        {
            var geoOptixSites = await GetJsonFromCatalogImpl<List<GeoOptixSite>>(GeoOptixSitesUri);
            return geoOptixSites;
        }

        public async Task<GeoOptixSite> GetGeoOptixSite(string siteID)
        {
            var geoOptixSite = await GetJsonFromCatalogImpl<GeoOptixSite>($"{GeoOptixSitesUri}/{siteID}");
            return geoOptixSite;
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

        public async Task<List<SensorSummaryDto>> GetSensorSummariesForWell(string wellRegistrationID)
        {
            var geoOptixStations = await GetGeoOptixStationsForSite(wellRegistrationID);
            return geoOptixStations.AsParallel().Select(x => new SensorSummaryDto(x)).ToList();
        }

        private async Task<List<GeoOptixStation>> GetGeoOptixStations()
        {
            const string uri = "project-overview-web/water-data-program/stations";
            var geoOptixStations = await GetJsonFromCatalogImpl<List<GeoOptixStation>>(uri);
            return geoOptixStations;
        }

        public async Task<List<GeoOptixStation>> GetGeoOptixStationsForSite(string siteID)
        {
            var geoOptixStations = await GetJsonFromCatalogImpl<List<GeoOptixStation>>($"{GeoOptixSitesUri}/{siteID}/stations");
            return geoOptixStations;
        }

        public async Task<List<GeoOptixSample>> GetGeoOptixSamplesForSite(string siteID)
        {
            var getGeoOptixSamplesForSite = await GetJsonFromCatalogImpl<List<GeoOptixSample>>($"{GeoOptixSitesUri}/{siteID}/samples");
            return getGeoOptixSamplesForSite;
        }

        public async Task<List<GeoOptixMethod>> GetGeoOptixSampleMethodsForSite(string siteID, string methodID)
        {
            var geoOptixStations = await GetJsonFromCatalogImpl<List<GeoOptixMethod>>($"{GeoOptixSitesUri}/{siteID}/samples/{methodID}/methods");
            return geoOptixStations;
        }

        public async Task<List<InstallationRecordDto>> GetInstallationRecords(string wellRegistrationID)
        {
            return new List<InstallationRecordDto>();
        //    var geoOptixSamples = await GetGeoOptixSamplesForSite(wellRegistrationID);
        //    foreach (var geoOptixSample in geoOptixSamples)
        //    {
        //        var installationRecordDto = new InstallationRecordDto();
        //        var geoOptixSampleMethodsForSite = await GetGeoOptixSampleMethodsForSite(wellRegistrationID, geoOptixSample.CanonicalName);
        //        if (!geoOptixSampleMethodsForSite.Any())
        //        {
        //            break;
        //        }

        //        foreach (var geoOptixMethod in geoOptixSampleMethodsForSite)
        //        {
        //            var installationRecord = 
        //        }
        //    }
        //
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
}