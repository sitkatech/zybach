//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellGroup]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellGroupExtensionMethods
    {
        public static WellGroupDto AsDto(this WellGroup wellGroup)
        {
            var wellGroupDto = new WellGroupDto()
            {
                WellGroupID = wellGroup.WellGroupID,
                WellGroupName = wellGroup.WellGroupName
            };
            DoCustomMappings(wellGroup, wellGroupDto);
            return wellGroupDto;
        }

        static partial void DoCustomMappings(WellGroup wellGroup, WellGroupDto wellGroupDto);

        public static WellGroupSimpleDto AsSimpleDto(this WellGroup wellGroup)
        {
            var wellGroupSimpleDto = new WellGroupSimpleDto()
            {
                WellGroupID = wellGroup.WellGroupID,
                WellGroupName = wellGroup.WellGroupName
            };
            DoCustomSimpleDtoMappings(wellGroup, wellGroupSimpleDto);
            return wellGroupSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WellGroup wellGroup, WellGroupSimpleDto wellGroupSimpleDto);
    }
}