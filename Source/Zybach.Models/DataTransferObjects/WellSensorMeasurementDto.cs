using System;

namespace Zybach.Models.DataTransferObjects
{
    public partial class WellSensorMeasurementDto
    {
        public DateTime MeasurementDateInPacificTime { get; set; }
        public DateTime MeasurementDate { get; set; }
        public string MeasurementValueString { get; set; }
    }
}