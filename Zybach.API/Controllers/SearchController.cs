using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class SearchController : SitkaController<SearchController>
    {
        public SearchController(ZybachDbContext dbContext, ILogger<SearchController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }


        [HttpGet("/search/{searchText}")]
        [ZybachViewFeature]
        public async Task<List<SearchSummaryDto>> GetSearchSuggestions([FromRoute] string searchText)
        {
            var wellResults = Wells.SearchByWellRegistrationID(_dbContext, searchText);

            wellResults.AddRange(Wells.SearchBySensorName(_dbContext, searchText));
            wellResults.AddRange(Wells.SearchByField(_dbContext, searchText));
            wellResults.AddRange(Wells.SearchByAghubRegisteredUser(_dbContext, searchText));
            wellResults.AddRange(Wells.SearchByChemigationPermit(_dbContext, searchText));

            return wellResults.DistinctBy(x => x.WellID).OrderBy(x => x.ObjectName).ToList();
        }
    }
}