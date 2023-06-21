using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers;

[ApiController]
public class WellGroupController : SitkaController<WellGroupController>
{
    public WellGroupController(ZybachDbContext dbContext, ILogger<WellGroupController> logger,
        KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : 
        base(dbContext, logger, keystoneService, zybachConfiguration)
    {}

    [HttpGet("wellGroups")]
    public ActionResult<IEnumerable<WellGroupDto>> GetWellGroups()
    {
        var wellGroupDtos = WellGroups.ListAsDto(_dbContext);
        return Ok(wellGroupDtos);
    }

    [HttpGet("wellGroups/{wellGroupID}")]
    [ZybachViewFeature]
    public ActionResult<WellGroupDto> GetWellGroupByID([FromRoute] int wellGroupID)
    {
        var wellGroupDto = WellGroups.GetByID(_dbContext, wellGroupID)?.AsDto();
        return Ok(wellGroupDto);
    }

    [HttpGet("wellGroups/{wellGroupID}/summary")]
    [ZybachViewFeature]
    public ActionResult<WellGroupSummaryDto> GetWellGroupSummary([FromRoute] int wellGroupID)
    {
        var wellGroup = WellGroups.GetByID(_dbContext, wellGroupID);
        if (wellGroup == null)
        {
            return NotFound();
        }

        var wellIDs = wellGroup.WellGroupWells.Select(x => x.WellID).ToList();
        var waterLevelInspectionSummaryDtos = WaterLevelInspections.ListByWellIDsAsSummaryDto(_dbContext, wellIDs);

        var waterLevelChartVegaSpec = string.Empty;
        if (waterLevelInspectionSummaryDtos.Any())
        {
            var waterLevelInspectionForVegaChartDtos = WaterLevelInspections.ListByWellIDsAsVegaChartDto(_dbContext, wellIDs);
            waterLevelChartVegaSpec = VegaSpecUtilities.GetWaterLevelChartVegaSpec(waterLevelInspectionForVegaChartDtos, true);
        }

        var wellGroupPressureSensors = new List<Sensor>();
        var wellGroupPressureSensorMeasurements = new List<SensorMeasurementDto>();

        foreach (var wellGroupWell in wellGroup.WellGroupWells)
        {
            var wellPressureSensors = wellGroupWell.Well.Sensors.Where(x => x.SensorTypeID == (int)SensorTypeEnum.WellPressure).ToList();
            wellGroupPressureSensors.AddRange(wellPressureSensors);

            var wellSensorMeasurements = WellSensorMeasurements.GetWellSensorMeasurementsForWellAndSensors(_dbContext, 
                wellGroupWell.Well.WellRegistrationID, wellPressureSensors);
            wellGroupPressureSensorMeasurements.AddRange(wellSensorMeasurements);
        }

        var vegaLiteChartSpec = VegaSpecUtilities.GetSensorTypeChartSpec(wellGroupPressureSensors, SensorType.WellPressure);
        var sensorMeasurements = wellGroupPressureSensorMeasurements;
        var sensorChartDataDto = new SensorChartDataDto(sensorMeasurements, vegaLiteChartSpec);

        var wellGroupSummaryDto = wellGroup.AsSummaryDto(waterLevelChartVegaSpec, waterLevelInspectionSummaryDtos, sensorChartDataDto);
        return Ok(wellGroupSummaryDto);
    }

    [HttpPost("wellGroups")]
    [ZybachViewFeature]
    public ActionResult<int> CreateWellGroup([FromBody] WellGroupDto wellGroupDto)
    {
        if (!ValidateWellGroupDto(wellGroupDto))
        {
            return BadRequest(ModelState);
        }

        var wellGroupID = WellGroups.Create(_dbContext, wellGroupDto);
        return Ok(wellGroupID);
    }

    [HttpPut("wellGroups/{wellGroupID}")]
    [ZybachViewFeature]
    public ActionResult UpdateWellGroup([FromRoute] int wellGroupID, [FromBody] WellGroupDto wellGroupDto)
    {
        var wellGroup = WellGroups.GetByID(_dbContext, wellGroupID);
        if (wellGroup == null)
        {
            return NotFound();
        }

        if (!ValidateWellGroupDto(wellGroupDto))
        {
            return BadRequest(ModelState);
        }

        WellGroups.Update(_dbContext, wellGroup, wellGroupDto);
        return Ok();
    }

    [HttpDelete("wellGroups/{wellGroupID}")]
    [ZybachViewFeature]
    public ActionResult DeleteWellGroup([FromRoute] int wellGroupID)
    {
        var wellGroup = WellGroups.GetByID(_dbContext, wellGroupID);
        if (wellGroup == null)
        {
            return NotFound();
        }

        WellGroups.Delete(_dbContext, wellGroup);
        return Ok();
    }

    private bool ValidateWellGroupDto(WellGroupDto wellGroupDto)
    {
        var isValid = true;

        if (string.IsNullOrWhiteSpace(wellGroupDto.WellGroupName))
        {
            ModelState.AddModelError("Well Group Name", $"The Well Group Name field is required.");
            isValid = false;
        }
        else if (_dbContext.WellGroups.Any(x => x.WellGroupName == wellGroupDto.WellGroupName && x.WellGroupID != wellGroupDto.WellGroupID))
        {
            ModelState.AddModelError("Well Group Name", $"There is already a well group named {wellGroupDto.WellGroupName}.");
            isValid = false;
        }

        if (wellGroupDto.WellGroupWells == null || !wellGroupDto.WellGroupWells.Any())
        {
            ModelState.AddModelError("Wells", "Please select at least one well to create a well group.");
            isValid = false;
        }
        else if (wellGroupDto.WellGroupWells.Count(x => x.IsPrimary) > 1)
        {
            ModelState.AddModelError("Primary Well", "Well group cannot have more than one primary well.");
            isValid = false;
        }

        return isValid;
    }

    [HttpGet("wellGroups/{wellGroupID}/waterLevelInspectionChartSpec")]
    public ActionResult<WaterLevelInspectionsChartDataDto> GetWellGroupWaterLevelInspectionChartSpec([FromRoute] int wellGroupID)
    {
        var wellGroup = WellGroups.GetByID(_dbContext, wellGroupID);
        if (wellGroup == null)
        {
            return NotFound();
        }

        var wellIDs = wellGroup.WellGroupWells.Select(x => x.WellID).ToList();
        var waterLevelInspectionSummaryDtos = WaterLevelInspections.ListByWellIDsAsSummaryDto(_dbContext, wellIDs);

        var waterLevelChartVegaSpec = string.Empty;
        if (waterLevelInspectionSummaryDtos.Any())
        {
            var waterLevelInspectionForVegaChartDtos = WaterLevelInspections.ListByWellIDsAsVegaChartDto(_dbContext, wellIDs);
            waterLevelChartVegaSpec = VegaSpecUtilities.GetWaterLevelChartVegaSpec(waterLevelInspectionForVegaChartDtos, true, true);
        }

        var waterLevelInspectionsChartDataDto = new WaterLevelInspectionsChartDataDto(waterLevelInspectionSummaryDtos, waterLevelChartVegaSpec);
        return Ok(waterLevelInspectionsChartDataDto);
    }

    [HttpGet("wellGroups/{wellGroupID}/waterLevelSensorsChartSpec")]
    public ActionResult<SensorChartDataDto> GetWellGroupWaterLevelSensorsChartSpec([FromRoute] int wellGroupID)
    {
        var wellGroup = WellGroups.GetByID(_dbContext, wellGroupID);
        if (wellGroup == null)
        {
            return NotFound();
        }

        var wellGroupPressureSensors = new List<Sensor>();
        var wellGroupPressureSensorMeasurements = new List<SensorMeasurementDto>();

        foreach (var wellGroupWell in wellGroup.WellGroupWells)
        {
            var wellPressureSensors = wellGroupWell.Well.Sensors.Where(x => x.SensorTypeID == (int)SensorTypeEnum.WellPressure).ToList();
            wellGroupPressureSensors.AddRange(wellPressureSensors);

            var wellSensorMeasurements = WellSensorMeasurements.GetWellSensorMeasurementsForWellAndSensors(_dbContext,
                wellGroupWell.Well.WellRegistrationID, wellPressureSensors);
            wellGroupPressureSensorMeasurements.AddRange(wellSensorMeasurements);
        }

        var vegaLiteChartSpec = VegaSpecUtilities.GetSensorTypeChartSpec(wellGroupPressureSensors, SensorType.WellPressure);
        var sensorMeasurements = wellGroupPressureSensorMeasurements;
        var sensorChartDataDto = new SensorChartDataDto(sensorMeasurements, vegaLiteChartSpec);
        
        return Ok(sensorChartDataDto);
    }
}