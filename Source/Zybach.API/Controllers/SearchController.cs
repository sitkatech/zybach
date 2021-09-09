using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;

namespace Zybach.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SearchController : SitkaController<SearchController>
    {
        private readonly GeoOptixSearchService _geoOptixSearchService;

        public SearchController(ZybachDbContext dbContext, ILogger<SearchController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, GeoOptixService geoOptixService, GeoOptixSearchService geoOptixSearchService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _geoOptixSearchService = geoOptixSearchService;
        }


        [HttpGet("/api/search/{searchText}")]
        [ZybachViewFeature]
        public async Task<List<SearchSummaryDto>> GetSearchSuggestions([FromRoute] string searchText)
        {
            var searchSummaryDtos = await _geoOptixSearchService.GetSearchSuggestions(searchText);
            var agHubResultsByLandowner = AgHubWell.SearchByLandowner(_dbContext, searchText).Select(x => new SearchSummaryDto(x){ObjectType = "Landowner"});
            var agHubResultsByField = AgHubWell.SearchByField(_dbContext, searchText).Select(x => new SearchSummaryDto(x){ObjectType = "Field"});
            var aghubResults = AgHubWell.SearchByWellRegistrationID(_dbContext, searchText).Select(x => new SearchSummaryDto(x));
            return aghubResults
                .Union(searchSummaryDtos, new SearchSummaryDtoComparer())
                .Union(agHubResultsByField, new SearchSummaryDtoComparer())
                .Union(agHubResultsByLandowner, new SearchSummaryDtoComparer())
                .OrderBy(x => x.ObjectName).ToList();
        }
    }
}