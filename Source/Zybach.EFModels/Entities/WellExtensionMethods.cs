using System.Collections.Generic;
using System.Linq;
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
    }
}
