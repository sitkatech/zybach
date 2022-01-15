using System;

namespace Zybach.Models.DataTransferObjects
{
    public class WaterLevelInspectionSummaryDto
    {
        public int WaterLevelInspectionID { get; set; }
        public DateTime InspectionDate { get; set; }
        public decimal? Measurement { get; set; }
        public string? MeasuringEquipment { get; set; }
    }
}
