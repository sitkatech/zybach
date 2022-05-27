using System;
using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public partial class SensorSimpleDto
    {
        public string WellRegistrationID { get; set; }
        public string SensorTypeName { get; set; }
        public int? MessageAge { get; set; }
        public DateTime? FirstReadingDate { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public int? WellPageNumber { get; set; }
        public string? WellOwnerName { get; set; }
        public string? WellTownshipRangeSection { get; set; }
        public List<WellSensorMeasurementDto> WellSensorMeasurements { get; set; }
    }
}
