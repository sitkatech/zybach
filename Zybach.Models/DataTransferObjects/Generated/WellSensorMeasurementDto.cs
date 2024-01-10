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
        public int ReadingYear { get; set; }
        public int ReadingMonth { get; set; }
        public int ReadingDay { get; set; }
        public string SensorName { get; set; }
        public double MeasurementValue { get; set; }
        public bool? IsAnomalous { get; set; }
        public bool? IsElectricSource { get; set; }
    }

    public partial class WellSensorMeasurementSimpleDto
    {
        public int WellSensorMeasurementID { get; set; }
        public string WellRegistrationID { get; set; }
        public System.Int32 MeasurementTypeID { get; set; }
        public int ReadingYear { get; set; }
        public int ReadingMonth { get; set; }
        public int ReadingDay { get; set; }
        public string SensorName { get; set; }
        public double MeasurementValue { get; set; }
        public bool? IsAnomalous { get; set; }
        public bool? IsElectricSource { get; set; }
    }

}