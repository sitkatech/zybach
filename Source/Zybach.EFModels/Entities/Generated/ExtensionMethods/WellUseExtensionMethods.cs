//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellUse]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellUseExtensionMethods
    {
        public static WellUseDto AsDto(this WellUse wellUse)
        {
            var wellUseDto = new WellUseDto()
            {
                WellUseID = wellUse.WellUseID,
                WellUseName = wellUse.WellUseName,
                WellUseDisplayName = wellUse.WellUseDisplayName
            };
            DoCustomMappings(wellUse, wellUseDto);
            return wellUseDto;
        }

        static partial void DoCustomMappings(WellUse wellUse, WellUseDto wellUseDto);

        public static WellUseSimpleDto AsSimpleDto(this WellUse wellUse)
        {
            var wellUseSimpleDto = new WellUseSimpleDto()
            {
                WellUseID = wellUse.WellUseID,
                WellUseName = wellUse.WellUseName,
                WellUseDisplayName = wellUse.WellUseDisplayName
            };
            DoCustomSimpleDtoMappings(wellUse, wellUseSimpleDto);
            return wellUseSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WellUse wellUse, WellUseSimpleDto wellUseSimpleDto);
    }
}