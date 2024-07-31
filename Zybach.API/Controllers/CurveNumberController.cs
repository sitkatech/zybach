using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers;

[ApiController]
public class CurveNumberController : SitkaController<CurveNumberController>
{

    public CurveNumberController(ZybachDbContext dbContext, ILogger<CurveNumberController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
    {
    }

    [HttpGet("irrigation-unit-curve-numbers")]
    [ZybachViewFeature]
    public async Task<ActionResult<List<AgHubIrrigationUnitCurveNumberSimpleDto>>> GetIrrigationUnitCurveNumbers()
    {
        var curveNumbers = await AgHubIrrigationUnitCurveNumbers.ListAsSimpleDtoAsync(_dbContext);
        return curveNumbers;
    }

    [HttpGet("irrigation-units/{irrigationUnitID}/curve-numbers")]
    [ZybachViewFeature]
    public async Task<ActionResult<AgHubIrrigationUnitCurveNumberSimpleDto>> GetIrrigationUnitCurveNumber(int irrigationUnitID)
    {
        var irrigationUnit = AgHubIrrigationUnits.GetAgHubIrrigationUnitImpl(_dbContext).SingleOrDefault(x => x.AgHubIrrigationUnitID == irrigationUnitID);
        if (irrigationUnit == null)
        {
            return NotFound();
        }

        var curveNumber = await AgHubIrrigationUnitCurveNumbers.GetSimpleByIrrigationUnitIDAsync(_dbContext, irrigationUnitID);
        return curveNumber;
    }

    [HttpPut("irrigation-units/{irrigationUnitID}/curve-numbers")]
    [ZybachEditFeature]
    public async Task<ActionResult<AgHubIrrigationUnitCurveNumberSimpleDto>> UpsertIrrigationUnitCurveNumber(int irrigationUnitID, AgHubIrrigationUnitCurveNumberUpsertDto curveNumberUpsertDto)
    {
        var irrigationUnit = AgHubIrrigationUnits.GetAgHubIrrigationUnitImpl(_dbContext).SingleOrDefault(x => x.AgHubIrrigationUnitID == irrigationUnitID);
        if (irrigationUnit == null)
        {
            return NotFound();
        }

        var validationMessages = await AgHubIrrigationUnitCurveNumbers.ValidateUpsertAsync(_dbContext, curveNumberUpsertDto);
        validationMessages.ForEach(vm => { ModelState.AddModelError(vm.Type, vm.Message); });

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var curveNumber = await AgHubIrrigationUnitCurveNumbers.GetSimpleByIrrigationUnitIDAsync(_dbContext, irrigationUnitID);
        var updatedCurveNumber = await AgHubIrrigationUnitCurveNumbers.UpsertAsync(_dbContext, curveNumberUpsertDto, curveNumber?.AgHubIrrigationUnitCurveNumberID);

        return updatedCurveNumber;
    }
}