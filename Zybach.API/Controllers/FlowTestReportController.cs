using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers;

[ApiController]
public class FlowTestReportController : SitkaController<FlowTestReportController>
{
    public FlowTestReportController(ZybachDbContext dbContext, ILogger<FlowTestReportController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) 
        : base(dbContext, logger, keystoneService, zybachConfiguration)
    {
    }

    [HttpGet("wellsRequiringAction")]
    [ZybachViewFeature]
    public ActionResult<List<WellForFlowTestReportPageDto>> GetWellsRequiringAction()
    {
        var wellsRequiringAction = Wells.ListForFlowTestReportPageDtos(_dbContext);
        return Ok(wellsRequiringAction);
    }
}