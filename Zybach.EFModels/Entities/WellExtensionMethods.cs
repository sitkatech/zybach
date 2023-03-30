using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;
using Newtonsoft.Json;

namespace Zybach.EFModels.Entities
{
    public partial class WellExtensionMethods
    {
        static partial void DoCustomMappings(Well well, WellDto wellDto)
        {
            wellDto.Longitude = well.Longitude;
            wellDto.Latitude = well.Latitude;
        }

        static partial void DoCustomSimpleDtoMappings(Well well, WellSimpleDto wellSimpleDto)
        {
            wellSimpleDto.WellParticipationName = well.WellParticipation?.WellParticipationDisplayName;
        }

        public static WellWaterQualityInspectionSummaryDto AsWellWaterQualityInspectionSummaryDto(this Well well)
        {
            return new WellWaterQualityInspectionSummaryDto()
            {
                Well = well.AsSimpleDto(),
                HasWaterQualityInspections = well.WaterQualityInspections.Any(),
                LatestWaterQualityInspectionDate = well.WaterQualityInspections.MaxBy(x => x.InspectionDate)?.InspectionDate
            };
        }

        public static WellWaterQualityInspectionDetailedDto AsWellWaterQualityInspectionDetailedDto(this Well well,
            List<WaterQualityInspectionSimpleDto> waterQualityInspectionSimpleDtos)
        {
            var wellWaterQualityInspectionDetailedDto = new WellWaterQualityInspectionDetailedDto()
            {
                Well = well.AsSimpleDto(),

                WaterQualityInspections = waterQualityInspectionSimpleDtos
            };

            return wellWaterQualityInspectionDetailedDto;
        }

        public static WellWaterLevelMapSummaryDto AsWaterLevelMapSummaryDto(this Well well)
        {
            var sensors = well.Sensors.Select(x => x.AsSimpleDto()).ToList();

            var wellWaterLevelMapSummaryDto = new WellWaterLevelMapSummaryDto()
            {
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                Location = new Feature(new Point(new Position(well.WellGeometry.Coordinate.Y, well.WellGeometry.Coordinate.X))),
                WellNickname = well.WellNickname,
                TownshipRangeSection = well.TownshipRangeSection,
                Sensors = sensors
            };

            return wellWaterLevelMapSummaryDto;
        }

        public static WellMinimalDto AsMinimalDto(this Well well)
        {
            return new WellMinimalDto
            {
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                WellNickname = well.WellNickname,
                Location = new Feature(new Point(new Position(well.WellGeometry.Coordinate.Y, well.WellGeometry.Coordinate.X))),
                Notes = well.Notes,
                Sensors = well.Sensors.Select(x => x.AsSimpleDto()).ToList()
            };
        }
    }
}
