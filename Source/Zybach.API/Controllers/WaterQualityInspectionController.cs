using System.Collections.Generic;
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
    public class WaterQualityInspectionController : SitkaController<WaterQualityInspectionController>
    {
        public WaterQualityInspectionController(ZybachDbContext dbContext, ILogger<WaterQualityInspectionController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext,
            logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/waterQualityInspections")]
        [ZybachViewFeature]
        public ActionResult<List<WaterQualityInspectionSimpleDto>> GetAllWaterQualityInspections()
        {
            var waterQualityInspectionSimpleDtos = WaterQualityInspections.ListAsSimpleDto(_dbContext);
            return Ok(waterQualityInspectionSimpleDtos);
        }
    }
}