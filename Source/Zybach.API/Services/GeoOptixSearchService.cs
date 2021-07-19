using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Zybach.API.Services
{
    public class GeoOptixSearchService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeoOptixService> _logger;

        public GeoOptixSearchService(HttpClient httpClient, ILogger<GeoOptixService> logger)
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

        public async Task<List<SearchSummaryDto>> GetSearchSuggestions(string textToSearch)
        {
            var geoOptixSearchResults = await GetJsonFromCatalogImpl<GeoOptixSearchResults>($"suggest/{textToSearch}?pageSize=1");
            return geoOptixSearchResults.Results.AsParallel().Select(x => new SearchSummaryDto(x.Document)).ToList();
        }
    }

    public class GeoOptixSearchResults
    {
        [JsonProperty("value")]
        public List<GeoOptixSearchResult> Results { get; set; }
    }

    public class GeoOptixSearchResult
    {
        public GeoOptixDocument Document { get; set; }
        [JsonProperty("@search.text")]
        public string SearchText { get; set; }
    }

    public class GeoOptixDocument
    {
        public string ObjectType { get; set; }
        public string Name { get; set; }
        public string SiteCanonicalName { get; set; }
        public string Description { get; set; }
    }


    public class SearchSummaryDto
    {
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }
        public string WellID { get; set; }

        public SearchSummaryDto()
        {
        }

        public SearchSummaryDto(GeoOptixDocument geoOptixDocument)
        {
            ObjectName = geoOptixDocument.Name;
            ObjectType = GeoOptixObjectTypeToZybachObjectType(geoOptixDocument.ObjectType);
            WellID = geoOptixDocument.SiteCanonicalName;
        }

        private string GeoOptixObjectTypeToZybachObjectType(string objectType)
        {
            var geoOptixObjectTypeEnum = Enum.Parse<GeoOptixObjectTypeEnum>(objectType);
            switch (geoOptixObjectTypeEnum)
            {
                case GeoOptixObjectTypeEnum.Site:
                    return ZybachObjectTypeEnum.Well.ToString();
                case GeoOptixObjectTypeEnum.Station:
                    return ZybachObjectTypeEnum.Sensor.ToString();
                case GeoOptixObjectTypeEnum.Sample:
                    return ZybachObjectTypeEnum.Installation.ToString();
                default:
                    return "";
            }
        }
    }


    public enum GeoOptixObjectTypeEnum
    {
        Site,
        Station,
        Sample
    }

    public enum ZybachObjectTypeEnum
    {
        Well,
        Sensor,
        Installation
    }

}