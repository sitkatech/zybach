using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DroolTool.API.Services;
using DroolTool.API.Services.Authorization;
using DroolTool.EFModels.Entities;

namespace DroolTool.API.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly DroolToolDbContext _dbContext;
        private readonly ILogger<RoleController> _logger;
        private readonly KeystoneService _keystoneService;

        public RoleController(DroolToolDbContext dbContext, ILogger<RoleController> logger, KeystoneService keystoneService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _keystoneService = keystoneService;
        }

        [HttpGet("roles")]
        [UserManageFeature]
        public IActionResult Get()
        {
            var roleDtos = Role.List(_dbContext);
            return Ok(roleDtos);
        }
    }
}