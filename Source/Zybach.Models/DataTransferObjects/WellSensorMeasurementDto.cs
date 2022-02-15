using System;

namespace Zybach.Models.DataTransferObjects
{
    public partial class WellSensorMeasurementDto
    {
        public WellSensorMeasurementDto()
        {
        }

        public WellSensorMeasurementDto(MeasurementTypeDto measurementType, string sensorName, DateTime measurementDate,
            double measurementValue, string measurementValueString)
        {
            MeasurementType = measurementType;
            SensorName = sensorName;
            MeasurementValue = measurementValue;
            MeasurementDate = measurementDate;
            MeasurementValueString = measurementValueString;
        }

        public DateTime MeasurementDateInPacificTime { get; set; }
        public DateTime MeasurementDate { get; set; }
        public string MeasurementValueString { get; set; }
    }
}