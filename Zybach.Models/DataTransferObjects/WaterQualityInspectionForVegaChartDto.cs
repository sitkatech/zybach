using System;

namespace Zybach.Models.DataTransferObjects
{
    public class WaterQualityInspectionForVegaChartDto
    {
        public int WellID { get; set; }
        public DateTime InspectionDate { get; set; }
        public int InspectionYear => InspectionDate.Year;
        public decimal? LabNitrates { get; set; }
    }
}
