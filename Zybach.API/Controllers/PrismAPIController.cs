using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers;

[ApiController]
public class PrismAPIController : SitkaController<PrismAPIController>
{
    public PrismAPIController(ZybachDbContext dbContext, ILogger<PrismAPIController> logger,
                              KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
    {
    }

    [HttpGet("prism-api/element-type/{element-types}/dates/{date}")]
    [ZybachViewFeature]
    public ActionResult<List<JObject>> GetDataForDate([FromRoute] string elementType, [FromRoute] DateTime date)
    {
        return Ok(new List<JObject>());
    }
}