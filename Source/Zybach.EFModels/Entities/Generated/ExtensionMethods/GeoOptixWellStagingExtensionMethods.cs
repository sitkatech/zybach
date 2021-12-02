//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[GeoOptixWellStaging]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class GeoOptixWellStagingExtensionMethods
    {
        public static GeoOptixWellStagingDto AsDto(this GeoOptixWellStaging geoOptixWellStaging)
        {
            var geoOptixWellStagingDto = new GeoOptixWellStagingDto()
            {
                GeoOptixWellStagingID = geoOptixWellStaging.GeoOptixWellStagingID,
                WellRegistrationID = geoOptixWellStaging.WellRegistrationID
            };
            DoCustomMappings(geoOptixWellStaging, geoOptixWellStagingDto);
            return geoOptixWellStagingDto;
        }

        static partial void DoCustomMappings(GeoOptixWellStaging geoOptixWellStaging, GeoOptixWellStagingDto geoOptixWellStagingDto);

        public static GeoOptixWellStagingSimpleDto AsSimpleDto(this GeoOptixWellStaging geoOptixWellStaging)
        {
            var geoOptixWellStagingSimpleDto = new GeoOptixWellStagingSimpleDto()
            {
                GeoOptixWellStagingID = geoOptixWellStaging.GeoOptixWellStagingID,
                WellRegistrationID = geoOptixWellStaging.WellRegistrationID
            };
            DoCustomSimpleDtoMappings(geoOptixWellStaging, geoOptixWellStagingSimpleDto);
            return geoOptixWellStagingSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(GeoOptixWellStaging geoOptixWellStaging, GeoOptixWellStagingSimpleDto geoOptixWellStagingSimpleDto);
    }
}