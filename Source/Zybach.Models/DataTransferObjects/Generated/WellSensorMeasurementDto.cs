//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurement]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class WellSensorMeasurementDto
    {
        public int WellSensorMeasurementID { get; set; }
        public string WellRegistrationID { get; set; }
        public MeasurementTypeDto MeasurementType { get; set; }
        public DateTime ReadingDate { get; set; }
        public string SensorName { get; set; }
        public double MeasurementValue { get; set; }
    }
}