using System;

namespace Zybach.Models.DataTransferObjects
{
    public partial class WaterQualityInspectionForVegaChartDto
    {
        public int WellID { get; set; }
        public DateTime InspectionDate { get; set; }
        public decimal? LabNitrates { get; set; }
        public decimal MostRecentDateLabNitrates { get; set; }
    }
}