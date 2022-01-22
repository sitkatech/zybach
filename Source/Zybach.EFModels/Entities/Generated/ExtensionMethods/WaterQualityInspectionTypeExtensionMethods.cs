//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WaterQualityInspectionType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WaterQualityInspectionTypeExtensionMethods
    {
        public static WaterQualityInspectionTypeDto AsDto(this WaterQualityInspectionType waterQualityInspectionType)
        {
            var waterQualityInspectionTypeDto = new WaterQualityInspectionTypeDto()
            {
                WaterQualityInspectionTypeID = waterQualityInspectionType.WaterQualityInspectionTypeID,
                WaterQualityInspectionTypeName = waterQualityInspectionType.WaterQualityInspectionTypeName,
                WaterQualityInspectionTypeDisplayName = waterQualityInspectionType.WaterQualityInspectionTypeDisplayName
            };
            DoCustomMappings(waterQualityInspectionType, waterQualityInspectionTypeDto);
            return waterQualityInspectionTypeDto;
        }

        static partial void DoCustomMappings(WaterQualityInspectionType waterQualityInspectionType, WaterQualityInspectionTypeDto waterQualityInspectionTypeDto);

        public static WaterQualityInspectionTypeSimpleDto AsSimpleDto(this WaterQualityInspectionType waterQualityInspectionType)
        {
            var waterQualityInspectionTypeSimpleDto = new WaterQualityInspectionTypeSimpleDto()
            {
                WaterQualityInspectionTypeID = waterQualityInspectionType.WaterQualityInspectionTypeID,
                WaterQualityInspectionTypeName = waterQualityInspectionType.WaterQualityInspectionTypeName,
                WaterQualityInspectionTypeDisplayName = waterQualityInspectionType.WaterQualityInspectionTypeDisplayName
            };
            DoCustomSimpleDtoMappings(waterQualityInspectionType, waterQualityInspectionTypeSimpleDto);
            return waterQualityInspectionTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WaterQualityInspectionType waterQualityInspectionType, WaterQualityInspectionTypeSimpleDto waterQualityInspectionTypeSimpleDto);
    }
}