using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers;

[ApiController]
public class SystemInfoController : SitkaController<SystemInfoController>
{
    private readonly IOptions<ZybachConfiguration> _configuration;

    public SystemInfoController(ZybachDbContext dbContext, ILogger<SystemInfoController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> configuration)
        : base(dbContext, logger, keystoneService, configuration)
    {
        _configuration = configuration;
    }

    [Route("/")] // Default Route
    [HttpGet]
    public ActionResult<SystemInfoDto> GetSystemInfo([FromServices] IWebHostEnvironment environment)
    {
        var systemInfo = new SystemInfoDto
        {
            Environment = environment.EnvironmentName,
            CurrentTimeUTC = DateTime.UtcNow.ToString("o"),
        };

        return Ok(systemInfo);
    }

}