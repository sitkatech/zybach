using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class IrrigationUnitController : SitkaController<IrrigationUnitController>
    {
        public IrrigationUnitController(ZybachDbContext dbContext, ILogger<IrrigationUnitController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("irrigationUnits/summary/{startDateMonth}/{startDateYear}/{endDateMonth}/{endDateYear}")]
        [ZybachViewFeature]
        public ActionResult<List<AgHubIrrigationUnitSummaryDto>> GetIrrigationUnitSummariesForDateRange([FromRoute] int startDateMonth, [FromRoute] int startDateYear, 
            int endDateMonth, int endDateYear)
        {
            var ahiuSummaryDtos = AgHubIrrigationUnitSummary.GetForDateRange(_dbContext, startDateYear, startDateMonth, endDateYear, endDateMonth);
            return Ok(ahiuSummaryDtos);
        }

        [HttpGet("/irrigationUnits")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<AgHubIrrigationUnitSimpleDto>> ListIrrigationUnits()
        {
            var irrigationUnits = AgHubIrrigationUnits.ListAsSimpleDto(_dbContext);
            return Ok(irrigationUnits);
        }

        [HttpGet("/irrigationUnits/{irrigationUnitID}")]
        [ZybachViewFeature]
        public ActionResult<AgHubIrrigationUnitDetailDto> GetIrrigationUnitByID([FromRoute] int irrigationUnitID)
        {
            var irrigationUnit = AgHubIrrigationUnits.GetAgHubIrrigationUnitImpl(_dbContext).SingleOrDefault(x => x.AgHubIrrigationUnitID == irrigationUnitID);

            if (irrigationUnit != null)
            {
                return Ok(AgHubIrrigationUnits.AgHubIrrigationUnitAsDetailDto(irrigationUnit));
            }

            return NotFound();
        }

        [HttpGet("irrigationUnits/farmingPractices")]
        [ZybachViewFeature]
        public ActionResult<List<AgHubIrrigationUnitFarmingPracticeDto>> ListIrrigationUnitIrrigatedAcres()
        {
            var agHubIrrigationUnitWellIrrigatedAcreDtos =
                AgHubIrrigationUnits.ListAsAgHubIrrigationUnitWellIrrigatedAcreDtos(_dbContext);

            return Ok(agHubIrrigationUnitWellIrrigatedAcreDtos);
        }
    }
}
