using System;
using System.Collections.Generic;
using System.Linq;

namespace Zybach.Models.DataTransferObjects;

public class WaterLevelInspectionsChartDataDto
{
    public List<WaterLevelInspectionSummaryDto> WaterLevelInspections { get; set; }
    public DateTime? FirstInspectionDate { get; set; }
    public DateTime? LastInspectionDate { get; set; }
    public string ChartSpec { get; set; }

    public WaterLevelInspectionsChartDataDto(List<WaterLevelInspectionSummaryDto> waterLevelInspections, string chartSpec)
    {
        WaterLevelInspections = waterLevelInspections;
        FirstInspectionDate = waterLevelInspections.Any() ? waterLevelInspections.Min(x => x.InspectionDate) : null;
        LastInspectionDate = waterLevelInspections.Any() ? waterLevelInspections.Max(x => x.InspectionDate) : null;
        ChartSpec = chartSpec;
    }
}