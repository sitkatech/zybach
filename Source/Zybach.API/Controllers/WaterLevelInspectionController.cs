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
    public class WaterLevelInspectionController : SitkaController<WaterLevelInspectionController>
    {
        public WaterLevelInspectionController(ZybachDbContext dbContext, ILogger<WaterLevelInspectionController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext,
            logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/waterLevelInspections")]
        [ZybachViewFeature]
        public ActionResult<List<WaterLevelInspectionSimpleDto>> GetAllWaterLevelInspections()
        {
            var waterLevelInspectionSimpleDtos = WaterLevelInspections.ListAsSimpleDto(_dbContext);
            return Ok(waterLevelInspectionSimpleDtos);
        }

        [HttpGet("/api/waterLevelInspections/{waterLevelInspectionID}")]
        [ZybachViewFeature]
        public ActionResult<WaterLevelInspectionSimpleDto> GetWaterLevelInspection([FromRoute] int waterLevelInspectionID)
        {
            var waterLevelInspectionSimpleDto = WaterLevelInspections.GetByIDAsSimpleDto(_dbContext, waterLevelInspectionID);
            return Ok(waterLevelInspectionSimpleDto);
        }
    }
}