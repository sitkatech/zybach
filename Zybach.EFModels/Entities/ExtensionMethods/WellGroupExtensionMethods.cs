using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static partial class WellGroupExtensionMethods
{
    static partial void DoCustomMappings(WellGroup wellGroup, WellGroupDto wellGroupDto)
    {
        var waterLevelInspections = wellGroup.WellGroupWells.SelectMany(x => x.Well.WaterLevelInspections).ToList();

        wellGroupDto.PrimaryWell = wellGroup.WellGroupWells.SingleOrDefault(x => x.IsPrimary)?.Well.AsSimpleDto();
        wellGroupDto.WellGroupWells = wellGroup.WellGroupWells.Select(x => x.AsSimpleDto()).ToList();

        if (!waterLevelInspections.Any()) return;

        wellGroupDto.HasWaterLevelInspections = true;
        wellGroupDto.LatestWaterLevelInspectionDate = waterLevelInspections.Any() ? 
            waterLevelInspections.MaxBy(x => x.InspectionDate)?.InspectionDate : null;
    }

    public static WellGroupSummaryDto AsSummaryDto(this WellGroup wellGroup, string waterLevelChartVegaSpec, 
        List<WaterLevelInspectionSummaryDto> waterLevelInspectionSummaryDtos, SensorChartDataDto wellPressureSensorChartDataDto)
    {
        return new WellGroupSummaryDto()
        {
            WellGroupID = wellGroup.WellGroupID,
            WellGroupName = wellGroup.WellGroupName,
            PrimaryWell = wellGroup.WellGroupWells.SingleOrDefault(x => x.IsPrimary)?.Well.AsSimpleDto(),
            WellGroupWells = wellGroup.WellGroupWells.Select(x => x.AsSimpleDto()).ToList(),
            WaterLevelChartVegaSpec = waterLevelChartVegaSpec,
            WaterLevelInspections = waterLevelInspectionSummaryDtos,
            Sensors = wellGroup.WellGroupWells.SelectMany(x => x.Well.Sensors?.Select(x => x.AsMinimalDto())).ToList(),
            WellPressureSensorChartData = wellPressureSensorChartDataDto,
            BoundingBox = new BoundingBoxDto(wellGroup.WellGroupWells.Select(x => x.Well.WellGeometry))
        };
    }

    public static WellGroupWaterLevelInspectionDto AsWellGroupWaterLevelInspectionDto(this WellGroup wellGroup)
    {
        return new WellGroupWaterLevelInspectionDto()
        {
            WellGroup = wellGroup.AsDto(),
            WaterLevelInspections = wellGroup.WellGroupWells.SelectMany(x => x.Well.WaterLevelInspections)
                .Select(x => x.AsSimpleDto()).ToList()
        };
    }
}