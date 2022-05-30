using System.Collections.Generic;
using System.Linq;
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
    public class CountyController : SitkaController<CountyController>
    {
        public CountyController(ZybachDbContext dbContext, ILogger<CountyController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/counties")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<CountyDto>> GetCounties()
        {
            var counties = County.AllAsDto.OrderBy(x => x.CountyDisplayName);
            return Ok(counties);
        }
    }
}