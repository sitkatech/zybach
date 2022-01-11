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
    public class WaterQualityInspectionController : SitkaController<WaterQualityInspectionController>
    {
        public WaterQualityInspectionController(ZybachDbContext dbContext, ILogger<WaterQualityInspectionController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext,
            logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/waterQualityInspectionTypes")]
        [ZybachViewFeature]
        public ActionResult<List<WaterQualityInspectionTypeDto>> GetWaterQualityInspectionTypes()
        {
            var waterQualityInspectionTypeDtos = WaterQualityInspectionTypes.ListAsDto(_dbContext);
            return Ok(waterQualityInspectionTypeDtos);
        }

        [HttpGet("/api/waterQualityInspections")]
        [ZybachViewFeature]
        public ActionResult<List<WaterQualityInspectionSimpleDto>> GetAllWaterQualityInspections()
        {
            var waterQualityInspectionSimpleDtos = WaterQualityInspections.ListAsSimpleDto(_dbContext);
            return Ok(waterQualityInspectionSimpleDtos);
        }

        [HttpGet("/api/clearinghouseWaterQualityInspections")]
        //[ZybachViewFeature]
        public ActionResult<List<ClearinghouseWaterQualityInspectionDto>> GetClearinghouseWaterQualityInspections()
        {
            var clearinghouseWaterQualityInspectionDtos = WaterQualityInspections.ListForClearinghouseAsDto(_dbContext);
            return Ok(clearinghouseWaterQualityInspectionDtos);
        }

        [HttpGet("/api/waterQualityInspections/{waterQualityInspectionID}")]
        [ZybachViewFeature]
        public ActionResult<WaterQualityInspectionSimpleDto> GetWaterQualityInspection([FromRoute] int waterQualityInspectionID)
        {
            var waterQualityInspectionSimpleDto = WaterQualityInspections.GetByIDAsSimpleDto(_dbContext, waterQualityInspectionID);
            return Ok(waterQualityInspectionSimpleDto);
        }

        [HttpPost("/api/waterQualityInspections")]
        [AdminFeature]
        public ActionResult<WaterQualityInspectionSimpleDto> CreateWaterQualityInspection([FromBody] WaterQualityInspectionUpsertDto waterQualityInspectionUpsert)
        {
            var well = Wells.GetByWellRegistrationID(_dbContext, waterQualityInspectionUpsert.WellRegistrationID);
            if (well == null)
            {
                ModelState.AddModelError("Well Registration ID", $"Well with Well Registration ID '{waterQualityInspectionUpsert.WellRegistrationID}' not found!");
                return BadRequest(ModelState);
            }

            var waterQualityInspectionSimpleDto = WaterQualityInspections.CreateWaterQualityInspection(_dbContext, waterQualityInspectionUpsert, well.WellID);
            return Ok(waterQualityInspectionSimpleDto);
        }

        [HttpPut("/api/waterQualityInspections/{waterQualityInspectionID}")]
        [AdminFeature]
        public ActionResult UpdateWaterQualityInspection([FromRoute] int waterQualityInspectionID,
            [FromBody] WaterQualityInspectionUpsertDto waterQualityInspectionUpsert)
        {
            var waterQualityInspection = WaterQualityInspections.GetByID(_dbContext, waterQualityInspectionID);

            if (ThrowNotFound(waterQualityInspection, "WaterQualityInspection",
                waterQualityInspectionID, out var actionResult))
            {
                return actionResult;
            }

            var well = Wells.GetByWellRegistrationID(_dbContext, waterQualityInspectionUpsert.WellRegistrationID);
            if (well == null)
            {
                ModelState.AddModelError("Well Registration ID", $"Well with Well Registration ID '{waterQualityInspectionUpsert.WellRegistrationID}' not found!");
                return BadRequest();
            }

            WaterQualityInspections.UpdateWaterQualityInspection(_dbContext, waterQualityInspection, waterQualityInspectionUpsert, well.WellID);
            return Ok();
        }

        [HttpDelete("/api/waterQualityInspections/{waterQualityInspectionID}")]
        [AdminFeature]
        public ActionResult DeleteWaterQualityInspectionByID([FromRoute] int waterQualityInspectionID)
        {
            var waterQualityInspection = WaterQualityInspections.GetByID(_dbContext, waterQualityInspectionID);

            if (ThrowNotFound(waterQualityInspection, "WaterQualityInspection",
                waterQualityInspectionID, out var actionResult))
            {
                return actionResult;
            }

            _dbContext.WaterQualityInspections.Remove(waterQualityInspection);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}