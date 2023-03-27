//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellGroupWell]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellGroupWellExtensionMethods
    {
        public static WellGroupWellDto AsDto(this WellGroupWell wellGroupWell)
        {
            var wellGroupWellDto = new WellGroupWellDto()
            {
                WellGroupWellID = wellGroupWell.WellGroupWellID,
                WellGroup = wellGroupWell.WellGroup.AsDto(),
                Well = wellGroupWell.Well.AsDto(),
                IsPrimary = wellGroupWell.IsPrimary
            };
            DoCustomMappings(wellGroupWell, wellGroupWellDto);
            return wellGroupWellDto;
        }

        static partial void DoCustomMappings(WellGroupWell wellGroupWell, WellGroupWellDto wellGroupWellDto);

        public static WellGroupWellSimpleDto AsSimpleDto(this WellGroupWell wellGroupWell)
        {
            var wellGroupWellSimpleDto = new WellGroupWellSimpleDto()
            {
                WellGroupWellID = wellGroupWell.WellGroupWellID,
                WellGroupID = wellGroupWell.WellGroupID,
                WellID = wellGroupWell.WellID,
                IsPrimary = wellGroupWell.IsPrimary
            };
            DoCustomSimpleDtoMappings(wellGroupWell, wellGroupWellSimpleDto);
            return wellGroupWellSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WellGroupWell wellGroupWell, WellGroupWellSimpleDto wellGroupWellSimpleDto);
    }
}