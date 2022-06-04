//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[GeoOptixSensorStaging]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class GeoOptixSensorStagingDto
    {
        public int GeoOptixSensorStagingID { get; set; }
        public string WellRegistrationID { get; set; }
        public string SensorName { get; set; }
        public string SensorType { get; set; }
    }

    public partial class GeoOptixSensorStagingSimpleDto
    {
        public int GeoOptixSensorStagingID { get; set; }
        public string WellRegistrationID { get; set; }
        public string SensorName { get; set; }
        public string SensorType { get; set; }
    }

}