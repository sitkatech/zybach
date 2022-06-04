//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellParticipation]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellParticipationExtensionMethods
    {
        public static WellParticipationDto AsDto(this WellParticipation wellParticipation)
        {
            var wellParticipationDto = new WellParticipationDto()
            {
                WellParticipationID = wellParticipation.WellParticipationID,
                WellParticipationName = wellParticipation.WellParticipationName,
                WellParticipationDisplayName = wellParticipation.WellParticipationDisplayName
            };
            DoCustomMappings(wellParticipation, wellParticipationDto);
            return wellParticipationDto;
        }

        static partial void DoCustomMappings(WellParticipation wellParticipation, WellParticipationDto wellParticipationDto);

        public static WellParticipationSimpleDto AsSimpleDto(this WellParticipation wellParticipation)
        {
            var wellParticipationSimpleDto = new WellParticipationSimpleDto()
            {
                WellParticipationID = wellParticipation.WellParticipationID,
                WellParticipationName = wellParticipation.WellParticipationName,
                WellParticipationDisplayName = wellParticipation.WellParticipationDisplayName
            };
            DoCustomSimpleDtoMappings(wellParticipation, wellParticipationSimpleDto);
            return wellParticipationSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WellParticipation wellParticipation, WellParticipationSimpleDto wellParticipationSimpleDto);
    }
}