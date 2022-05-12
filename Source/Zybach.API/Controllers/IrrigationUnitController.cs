using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("/api/irrigationUnits")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<AgHubIrrigationUnitSimpleDto>> ListIrrigationUnits()
        {
            var irrigationUnits = AgHubIrrigationUnits.ListAsSimpleDto(_dbContext);
            return Ok(irrigationUnits);
        }

        [HttpGet("/api/irrigationUnits/{irrigationUnitID}")]
        //[ZybachViewFeature]
        public ActionResult<AgHubIrrigationUnitDetailDto> GetIrrigationUnitByID([FromRoute] int irrigationUnitID)
        {
            var irrigationUnit = AgHubIrrigationUnits.GetAgHubIrrigationUnitImpl(_dbContext).SingleOrDefault(x => x.AgHubIrrigationUnitID == irrigationUnitID);

            if (irrigationUnit != null)
            {
                return Ok(AgHubIrrigationUnits.AgHubIrrigationUnitAsDetailDto(irrigationUnit));
            }

            return NotFound();
        }
    }
}
