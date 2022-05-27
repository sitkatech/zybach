//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellWaterQualityInspectionType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellWaterQualityInspectionTypeExtensionMethods
    {
        public static WellWaterQualityInspectionTypeDto AsDto(this WellWaterQualityInspectionType wellWaterQualityInspectionType)
        {
            var wellWaterQualityInspectionTypeDto = new WellWaterQualityInspectionTypeDto()
            {
                WellWaterQualityInspectionTypeID = wellWaterQualityInspectionType.WellWaterQualityInspectionTypeID,
                Well = wellWaterQualityInspectionType.Well.AsDto(),
                WaterQualityInspectionType = wellWaterQualityInspectionType.WaterQualityInspectionType.AsDto()
            };
            DoCustomMappings(wellWaterQualityInspectionType, wellWaterQualityInspectionTypeDto);
            return wellWaterQualityInspectionTypeDto;
        }

        static partial void DoCustomMappings(WellWaterQualityInspectionType wellWaterQualityInspectionType, WellWaterQualityInspectionTypeDto wellWaterQualityInspectionTypeDto);

        public static WellWaterQualityInspectionTypeSimpleDto AsSimpleDto(this WellWaterQualityInspectionType wellWaterQualityInspectionType)
        {
            var wellWaterQualityInspectionTypeSimpleDto = new WellWaterQualityInspectionTypeSimpleDto()
            {
                WellWaterQualityInspectionTypeID = wellWaterQualityInspectionType.WellWaterQualityInspectionTypeID,
                WellID = wellWaterQualityInspectionType.WellID,
                WaterQualityInspectionTypeID = wellWaterQualityInspectionType.WaterQualityInspectionTypeID
            };
            DoCustomSimpleDtoMappings(wellWaterQualityInspectionType, wellWaterQualityInspectionTypeSimpleDto);
            return wellWaterQualityInspectionTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WellWaterQualityInspectionType wellWaterQualityInspectionType, WellWaterQualityInspectionTypeSimpleDto wellWaterQualityInspectionTypeSimpleDto);
    }
}