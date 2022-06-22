using System;
using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public partial class SensorSimpleDto
    {
        public string WellRegistrationID { get; set; }
        public string SensorTypeName { get; set; }
        public int? MessageAge { get; set; }
        public double? LastVoltageReading { get; set; }
        public DateTime? FirstReadingDate { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public int? WellPageNumber { get; set; }
        public string WellOwnerName { get; set; }
        public string WellTownshipRangeSection { get; set; }
        public string ChartDataSourceName { get; set; }
        public string ChartAnomaliesDataSourceName { get; set; }
        public List<string> ChartDomains { get; set; }
        public List<string> ChartColors { get; set; }
        public List<string> ChartTooltipFields { get; set; }
    }
}
