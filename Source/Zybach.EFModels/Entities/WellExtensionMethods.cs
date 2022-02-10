using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

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
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                WellNickname = well.WellNickname,
                WellParticipationID = well.WellParticipation?.WellParticipationID,
                WellParticipationName = well.WellParticipation?.WellParticipationDisplayName,
                HasWaterLevelInspections = waterLevelInspectionSimpleDto != null,
                LatestWaterLevelInspectionDate = waterLevelInspectionSimpleDto?.InspectionDate,
                HasWaterQualityInspections = waterQualityInspectionSimpleDto != null,
                LatestWaterQualityInspectionDate = waterQualityInspectionSimpleDto?.InspectionDate
            };

            return wellInspectionSummaryDto;
        }

    }
}
