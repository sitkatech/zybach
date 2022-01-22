using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class SearchController : SitkaController<SearchController>
    {
        private readonly GeoOptixSearchService _geoOptixSearchService;

        public SearchController(ZybachDbContext dbContext, ILogger<SearchController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixSearchService geoOptixSearchService) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
            _geoOptixSearchService = geoOptixSearchService;
        }


        [HttpGet("/api/search/{searchText}")]
        [ZybachViewFeature]
        public async Task<List<SearchSummaryDto>> GetSearchSuggestions([FromRoute] string searchText)
        {
            var geoOptixDocuments = await _geoOptixSearchService.GetSearchSuggestions(searchText);
            var wellResultsByLandowner = Wells.SearchByAghubRegisteredUser(_dbContext, searchText).Select(x => new SearchSummaryDto(x){ObjectType = "Registered User"});
            var wellResultsByField = Wells.SearchByField(_dbContext, searchText).Select(x => new SearchSummaryDto(x){ObjectType = "Field"});
            var wellResults = Wells.SearchByWellRegistrationID(_dbContext, searchText).Select(x => new SearchSummaryDto(x));
            var wellIDsDictionary = _dbContext.GeoOptixWells.Include(x => x.Well)
                .ToDictionary(x => x.Well.WellRegistrationID, x => x.WellID);
            var geoOptixSearchSummaryDtos = geoOptixDocuments.Where(x => wellIDsDictionary.ContainsKey(x.SiteCanonicalName)).Select(x => new SearchSummaryDto(x, wellIDsDictionary[x.SiteCanonicalName]));
            return wellResults
                .Union(wellResultsByField, new SearchSummaryDtoComparer())
                .Union(wellResultsByLandowner, new SearchSummaryDtoComparer())
                .Union(geoOptixSearchSummaryDtos, new SearchSummaryDtoComparer())
                .OrderBy(x => x.ObjectName).ToList();
        }
    }
}