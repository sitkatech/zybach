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

        public static WellInspectionSummaryDto AsWellInspectionSummaryDto(this Well well,
            WaterLevelInspectionSimpleDto waterLevelInspectionSimpleDto, WaterQualityInspectionSimpleDto waterQualityInspectionSimpleDto)
        {
            var wellInspectionSummaryDto = new WellInspectionSummaryDto()
            {
                Well = well.AsSimpleDto(),

                HasWaterLevelInspections = waterLevelInspectionSimpleDto != null,
                LatestWaterLevelInspectionDate = waterLevelInspectionSimpleDto?.InspectionDate,
                HasWaterQualityInspections = waterQualityInspectionSimpleDto != null,
                LatestWaterQualityInspectionDate = waterQualityInspectionSimpleDto?.InspectionDate
            };

            return wellInspectionSummaryDto;
        }

        public static WellWaterLevelInspectionDetailedDto AsWellWaterLevelInspectionDetailedDto(this Well well,
            List<WaterLevelInspectionSimpleDto> waterLevelInspectionSimpleDtos)
        {
            var wellWaterLevelInspectionDetailedDto = new WellWaterLevelInspectionDetailedDto()
            {
                Well = well.AsSimpleDto(),

                WaterLevelInspections = waterLevelInspectionSimpleDtos
            };

            return wellWaterLevelInspectionDetailedDto;
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
            var sensors = well.Sensors.Select(x => new SensorSummaryDto()
            {
                SensorName = x.SensorName,
                SensorID = x.SensorID,
                SensorTypeID = x.SensorTypeID,
                SensorType = x.SensorType.SensorTypeDisplayName,
                WellRegistrationID = well.WellRegistrationID,
                IsActive = x.IsActive
            }).ToList();

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
    }
}
