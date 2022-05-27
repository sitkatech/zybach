using System;

namespace Zybach.Models.DataTransferObjects
{
    public class WaterLevelInspectionForVegaChartDto
    {
        public int WellID { get; set; }
        public DateTime InspectionDate { get; set; }
        public decimal? Measurement { get; set; }
        public decimal MostRecentMeasurement { get; set; }
    }
}
