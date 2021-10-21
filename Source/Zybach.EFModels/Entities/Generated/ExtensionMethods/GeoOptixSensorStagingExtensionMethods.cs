//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[GeoOptixSensorStaging]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class GeoOptixSensorStagingExtensionMethods
    {
        public static GeoOptixSensorStagingDto AsDto(this GeoOptixSensorStaging geoOptixSensorStaging)
        {
            var geoOptixSensorStagingDto = new GeoOptixSensorStagingDto()
            {
                GeoOptixSensorStagingID = geoOptixSensorStaging.GeoOptixSensorStagingID,
                WellRegistrationID = geoOptixSensorStaging.WellRegistrationID,
                SensorName = geoOptixSensorStaging.SensorName,
                SensorType = geoOptixSensorStaging.SensorType
            };
            DoCustomMappings(geoOptixSensorStaging, geoOptixSensorStagingDto);
            return geoOptixSensorStagingDto;
        }

        static partial void DoCustomMappings(GeoOptixSensorStaging geoOptixSensorStaging, GeoOptixSensorStagingDto geoOptixSensorStagingDto);

    }
}