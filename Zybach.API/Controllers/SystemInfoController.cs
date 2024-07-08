using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Logging;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers;

[ApiController]
public class SystemInfoController : SitkaController<SystemInfoController>
{
    public SystemInfoController(ZybachDbContext dbContext, ILogger<SystemInfoController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration)
        : base(dbContext, logger, keystoneService, zybachConfiguration)
    { }

    [HttpGet("/", Name = "GetSystemInfo")]  // MCS: the pattern seems to be to allow anonymous access to this endpoint
    [AllowAnonymous]
    [LogIgnore]
    public ActionResult<SystemInfoDto> GetSystemInfo([FromServices] IWebHostEnvironment environment)
    {
        SystemInfoDto systemInfo = new SystemInfoDto
        {
            Environment = environment.EnvironmentName,
            CurrentTimeUTC = DateTime.UtcNow.ToString("o"),
            PodName = _zybachConfiguration.HostName
        };
        return Ok(systemInfo);
    }

}