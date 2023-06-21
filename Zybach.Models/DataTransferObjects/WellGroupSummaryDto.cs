using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects;

public class WellGroupSummaryDto : WellGroupDto
{
    public string WaterLevelChartVegaSpec { get; set; }
    public List<WaterLevelInspectionSummaryDto> WaterLevelInspections { get; set; }
    public List<SensorMinimalDto> Sensors { get; set; }
    public SensorChartDataDto WellPressureSensorChartData { get; set; }
    public BoundingBoxDto BoundingBox { get; set; }
}