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

namespace Zybach.API.Controllers
{
    [ApiController]
    public class IrrigationUnitController : SitkaController<IrrigationUnitController>
    {
        public IrrigationUnitController(ZybachDbContext dbContext, ILogger<IrrigationUnitController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        //[HttpGet("/api/irrigationUnits")]
        //[ZybachViewFeature]
        //public ActionResult<IEnumerable<>> ListIrrigationUnits()
        //{
        //    var  = AgHubIrrigationUnits.ListAsDto(_dbContext);
        //    return Ok();
        //}

        //[HttpGet("/api/irrigationUnits")]
        //[ZybachViewFeature]
        //public ActionResult<IEnumerable<>> ListIrrigationUnits()
        //{
        //    var = AgHubIrrigationUnits.ListAsDto(_dbContext);
        //    return Ok();
        //}
    }
}
