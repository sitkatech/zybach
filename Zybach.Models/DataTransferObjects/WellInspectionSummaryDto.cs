#nullable enable
using System;

namespace Zybach.Models.DataTransferObjects
{
    public class WellInspectionSummaryDto
    {
        public WellSimpleDto? Well { get; set; }

        public bool HasWaterLevelInspections { get; set; }
        public DateTime? LatestWaterLevelInspectionDate { get; set; }

        public bool HasWaterQualityInspections { get; set; }
        public DateTime? LatestWaterQualityInspectionDate { get; set; }
    }
}
