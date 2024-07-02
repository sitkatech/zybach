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
using Zybach.Models.Prism;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Zybach.API.Controllers;

[ApiController]
public class PrismAPIController : SitkaController<PrismAPIController>
{
    private PrismAPIService _prismAPIService;

    public PrismAPIController(ZybachDbContext dbContext, ILogger<PrismAPIController> logger,
                              KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
    {
        _prismAPIService = new PrismAPIService();
    }

    [HttpGet("prism-api/element-type/{element-types}/dates/{date}")]
    [ZybachViewFeature]
    public ActionResult<List<PrismRecordDto>> GetDataForDate([FromRoute] string elementType, [FromRoute] DateTime date)
    {
        var element = PrismDataElement.FromString(elementType);

        if (element == null)
        {
            return RequireNotNullThrowNotFound(null, "Element", elementType);
        }

        var hasData = _prismAPIService.GetDataForDate(element, date);

        return Ok(new List<PrismRecordDto>());
    }

    [HttpGet("prism-api/element-type/{element-types}/dates/{startDate}/{endDate}")]
    [ZybachViewFeature]
    public ActionResult<List<PrismRecordDto>> GetDataForDateRange([FromRoute] string elementType, [FromRoute] DateTime startDate, [FromRoute] DateTime endDate)
    {
        var element = PrismDataElement.FromString(elementType);

        if (element == null)
        {
            return RequireNotNullThrowNotFound(null, "Element", elementType);
        }

        if (startDate > endDate)
        {
            return BadRequest("Start date must be before end date.");
        }


        var hasData = _prismAPIService.GetDataForDateRange(element, startDate, endDate);

        return Ok(new List<PrismRecordDto>());
    }
}