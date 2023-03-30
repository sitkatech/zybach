#nullable enable
using System;

namespace Zybach.Models.DataTransferObjects
{
    public class WellWaterQualityInspectionSummaryDto
    {
        public WellSimpleDto? Well { get; set; }
        public bool HasWaterQualityInspections { get; set; }
        public DateTime? LatestWaterQualityInspectionDate { get; set; }
    }
}
