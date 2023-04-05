using System;
using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects;

public partial class WellGroupDto
{
    public WellSimpleDto PrimaryWell { get; set; }
    public List<WellGroupWellSimpleDto> WellGroupWells { get; set; }
    public bool HasWaterLevelInspections { get; set; }
    public DateTime? LatestWaterLevelInspectionDate { get; set; }
}

public class WellGroupSummaryDto : WellGroupDto
{
    public string WaterLevelChartVegaSpec { get; set; }
    public List<WaterLevelInspectionSummaryDto> WaterLevelInspections { get; set; }
    public List<SensorMinimalDto> Sensors { get; set; }
    public SensorChartDataDto WellPressureSensorChartData { get; set; }
    public BoundingBoxDto BoundingBox { get; set; }
}