//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurementStaging]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class WellSensorMeasurementStagingDto
    {
        public int WellSensorMeasurementStagingID { get; set; }
        public string WellRegistrationID { get; set; }
        public MeasurementTypeDto MeasurementType { get; set; }
        public int ReadingYear { get; set; }
        public int ReadingMonth { get; set; }
        public int ReadingDay { get; set; }
        public string SensorName { get; set; }
        public double MeasurementValue { get; set; }
        public bool? IsElectricSource { get; set; }
    }

    public partial class WellSensorMeasurementStagingSimpleDto
    {
        public int WellSensorMeasurementStagingID { get; set; }
        public string WellRegistrationID { get; set; }
        public int MeasurementTypeID { get; set; }
        public int ReadingYear { get; set; }
        public int ReadingMonth { get; set; }
        public int ReadingDay { get; set; }
        public string SensorName { get; set; }
        public double MeasurementValue { get; set; }
        public bool? IsElectricSource { get; set; }
    }

}