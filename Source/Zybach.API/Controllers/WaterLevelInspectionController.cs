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

        [HttpGet("/api/waterLevelInspections/measuringEquipment")]
        [ZybachViewFeature]
        public ActionResult<List<WaterLevelMeasuringEquipmentDto>> GetAllMeasuringEquipments()
        {
            var waterLevelMeasuringEquipmentDtos = WaterLevelMeasuringEquipments.ListAsDto(_dbContext);
            return Ok(waterLevelMeasuringEquipmentDtos);
        }

        [HttpPost("/api/waterLevelInspections")]
        [ZybachEditFeature]
        public ActionResult<WaterLevelInspectionSimpleDto> Create([FromBody] WaterLevelInspectionUpsertDto waterLevelInspectionUpsertDto)
        {
            var well = Wells.GetByWellRegistrationIDWithTracking(_dbContext, waterLevelInspectionUpsertDto.WellRegistrationID);
            if (well == null)
            {
                ModelState.AddModelError("Well Registration ID", $"Well with Well Registration ID '{waterLevelInspectionUpsertDto.WellRegistrationID}' not found!");
                return BadRequest(ModelState);
            }

            var waterLevelInspectionSimpleDto = WaterLevelInspections.Create(_dbContext, waterLevelInspectionUpsertDto, well.WellID);
            return Ok(waterLevelInspectionSimpleDto);
        }

        [HttpGet("/api/waterLevelInspections/{waterLevelInspectionID}")]
        [ZybachViewFeature]
        public ActionResult<WaterLevelInspectionSimpleDto> GetWaterLevelInspection([FromRoute] int waterLevelInspectionID)
        {
            var waterLevelInspectionSimpleDto = WaterLevelInspections.GetByIDAsSimpleDto(_dbContext, waterLevelInspectionID);
            return Ok(waterLevelInspectionSimpleDto);
        }

        [HttpPut("/api/waterLevelInspections/{waterLevelInspectionID}")]
        [ZybachEditFeature]
        public ActionResult Update([FromRoute] int waterLevelInspectionID, [FromBody] WaterLevelInspectionUpsertDto waterLevelInspectionUpsertDto)
        {
            var waterLevelInspection = WaterLevelInspections.GetByID(_dbContext, waterLevelInspectionID);

            if (ThrowNotFound(waterLevelInspection, "WaterLevelInspection",
                waterLevelInspectionID, out var actionResult))
            {
                return actionResult;
            }

            var well = Wells.GetByWellRegistrationIDWithTracking(_dbContext, waterLevelInspectionUpsertDto.WellRegistrationID);
            if (well == null)
            {
                ModelState.AddModelError("Well Registration ID", $"Well with Well Registration ID '{waterLevelInspectionUpsertDto.WellRegistrationID}' not found!");
                return BadRequest();
            }

            WaterLevelInspections.Update(_dbContext, waterLevelInspection, waterLevelInspectionUpsertDto, well.WellID);
            return Ok();
        }

        [HttpDelete("/api/waterLevelInspections/{waterLevelInspectionID}")]
        [ZybachEditFeature]
        public ActionResult DeleteWaterLevelInspectionByID([FromRoute] int waterLevelInspectionID)
        {
            var waterLevelInspection = WaterLevelInspections.GetByID(_dbContext, waterLevelInspectionID);

            if (ThrowNotFound(waterLevelInspection, "WaterLevelInspection",
                waterLevelInspectionID, out var actionResult))
            {
                return actionResult;
            }

            _dbContext.WaterLevelInspections.Remove(waterLevelInspection);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}