//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[GeoOptixWell]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class GeoOptixWellExtensionMethods
    {
        public static GeoOptixWellDto AsDto(this GeoOptixWell geoOptixWell)
        {
            var geoOptixWellDto = new GeoOptixWellDto()
            {
                GeoOptixWellID = geoOptixWell.GeoOptixWellID,
                Well = geoOptixWell.Well.AsDto()
            };
            DoCustomMappings(geoOptixWell, geoOptixWellDto);
            return geoOptixWellDto;
        }

        static partial void DoCustomMappings(GeoOptixWell geoOptixWell, GeoOptixWellDto geoOptixWellDto);

        public static GeoOptixWellSimpleDto AsSimpleDto(this GeoOptixWell geoOptixWell)
        {
            var geoOptixWellSimpleDto = new GeoOptixWellSimpleDto()
            {
                GeoOptixWellID = geoOptixWell.GeoOptixWellID,
                WellID = geoOptixWell.WellID
            };
            DoCustomSimpleDtoMappings(geoOptixWell, geoOptixWellSimpleDto);
            return geoOptixWellSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(GeoOptixWell geoOptixWell, GeoOptixWellSimpleDto geoOptixWellSimpleDto);
    }
}