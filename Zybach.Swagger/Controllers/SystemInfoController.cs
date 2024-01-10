using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.Swagger.Controllers
{
    [ApiController]
    public class SystemInfoController : SitkaApiController<SystemInfoController>
    {
        public SystemInfoController(ZybachDbContext dbContext, ILogger<SystemInfoController> logger)
            : base(dbContext, logger)
        {
        }

        [HttpGet("/", Name = "GetSystemInfo")]
        [AllowAnonymous]
        public IActionResult GetSystemInfo([FromServices] IWebHostEnvironment environment)
        {
            var systemInfo = new SystemInfoDto
            {
                Environment = environment.EnvironmentName,
                CurrentTimeUTC = DateTime.UtcNow.ToString("o")
            };

            return Ok(systemInfo);
        }

    }
}