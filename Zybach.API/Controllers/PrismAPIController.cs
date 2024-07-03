using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Models.Prism;

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
        if (!hasData)
        {
            return NotFound(); //Might not be the right status code to throw here.
        }

        var prismRecordDtos = _dbContext.PrismRecords.AsNoTracking().Where(x => x.Date == date).Select(x => x.AsDto()).ToList();
        return Ok(prismRecordDtos);
    }

    [HttpGet("prism-api/element-type/{element-types}/dates/{startDate}/{endDate}")]
    [ZybachViewFeature]
    public async Task<ActionResult<List<PrismRecordDto>>> GetDataForDateRange([FromRoute] string elementType, [FromRoute] DateTime startDate, [FromRoute] DateTime endDate)
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
        
        var hasData = await _prismAPIService.GetDataForDateRange(element, startDate, endDate);
        if (!hasData)
        {
            return NotFound(); //Might not be the right status code to throw here.
        }

        var prismRecordDtos = await _dbContext.PrismRecords.AsNoTracking().Where(x => x.Date >= startDate && x.Date <= endDate).Select(x => x.AsDto()).ToListAsync();
        return Ok(prismRecordDtos);
    }
}