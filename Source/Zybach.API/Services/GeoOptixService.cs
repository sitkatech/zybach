using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zybach.Models.DataTransferObjects;
using Zybach.Models.GeoOptix;
using Zybach.Models.GeoOptix.SampleMethod;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Zybach.API.Services
{
    public class GeoOptixService
    {
        private const string GeoOptixSitesProjectOverviewWebUri = "project-overview-web/water-data-program/sites";
        private const string GeoOptixSitesProjectsUri = "projects/water-data-program/sites";

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

        private async Task<List<Site>> GetGeoOptixSites()
        {
            var geoOptixSites = await GetJsonFromCatalogImpl<List<Site>>(GeoOptixSitesProjectOverviewWebUri);
            return geoOptixSites;
        }

        public async Task<Site> GetGeoOptixSite(string siteID)
        {
            var geoOptixSite = await GetJsonFromCatalogImpl<Site>($"{GeoOptixSitesProjectOverviewWebUri}/{siteID}");
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

        private async Task<List<Station>> GetGeoOptixStations()
        {
            const string uri = "project-overview-web/water-data-program/stations";
            var geoOptixStations = await GetJsonFromCatalogImpl<List<Station>>(uri);
            return geoOptixStations;
        }

        public async Task<List<Station>> GetGeoOptixStationsForSite(string siteID)
        {
            var geoOptixStations = await GetJsonFromCatalogImpl<List<Station>>($"{GeoOptixSitesProjectOverviewWebUri}/{siteID}/stations");
            return geoOptixStations;
        }

        public async Task<List<Sample>> GetGeoOptixSamplesForSite(string siteID)
        {
            var getGeoOptixSamplesForSite = await GetJsonFromCatalogImpl<List<Sample>>($"{GeoOptixSitesProjectsUri}/{siteID}/samples");
            return getGeoOptixSamplesForSite;
        }

        public async Task<List<SampleMethodDto>> GetGeoOptixSampleMethodsForSite(string siteID, string methodID)
        {
            var geoOptixStations = await GetJsonFromCatalogImpl<List<SampleMethodDto>>($"{GeoOptixSitesProjectsUri}/{siteID}/samples/{methodID}/methods");
            return geoOptixStations;
        }

        public async Task<List<InstallationRecordDto>> GetInstallationRecords(string wellRegistrationID)
        {
            var installationRecordDtos = new List<InstallationRecordDto>();
            var geoOptixSamples = await GetGeoOptixSamplesForSite(wellRegistrationID);
            foreach (var geoOptixSample in geoOptixSamples)
            {
                var installationRecordDto = new InstallationRecordDto();
                var geoOptixSampleMethodsForSite = await GetGeoOptixSampleMethodsForSite(wellRegistrationID, geoOptixSample.CanonicalName);
                if (!geoOptixSampleMethodsForSite.Any())
                {
                    break;
                }

                var geoOptixMethod = geoOptixSampleMethodsForSite.First();
                var installationRecord = geoOptixMethod.MethodInstance.RecordSets.First().Records.First().RecordInstance;
                var sensorRecord = installationRecord.Fields.SingleOrDefault(x => x.CanonicalName == "sensor");
            }
            return installationRecordDtos;
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