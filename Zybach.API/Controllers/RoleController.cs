using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class RoleController : SitkaController<RoleController>
    {
        public RoleController(ZybachDbContext dbContext, ILogger<RoleController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/roles")]
        [ZybachViewFeature]
        public IActionResult Get()
        {
            var roleDtos = Roles.List(_dbContext);
            return Ok(roleDtos);
        }
    }
}