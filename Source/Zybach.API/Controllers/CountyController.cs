using System.Collections.Generic;
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

        [HttpGet("/api/counties")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<CountyDto>> GetCounties()
        {
            var counties = Counties.ListAsDto(_dbContext);
            return Ok(counties);
        }
    }
}