//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[CropType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class CropTypeExtensionMethods
    {
        public static CropTypeDto AsDto(this CropType cropType)
        {
            var cropTypeDto = new CropTypeDto()
            {
                CropTypeID = cropType.CropTypeID,
                CropTypeName = cropType.CropTypeName,
                CropTypeDisplayName = cropType.CropTypeDisplayName
            };
            DoCustomMappings(cropType, cropTypeDto);
            return cropTypeDto;
        }

        static partial void DoCustomMappings(CropType cropType, CropTypeDto cropTypeDto);

        public static CropTypeSimpleDto AsSimpleDto(this CropType cropType)
        {
            var cropTypeSimpleDto = new CropTypeSimpleDto()
            {
                CropTypeID = cropType.CropTypeID,
                CropTypeName = cropType.CropTypeName,
                CropTypeDisplayName = cropType.CropTypeDisplayName
            };
            DoCustomSimpleDtoMappings(cropType, cropTypeSimpleDto);
            return cropTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(CropType cropType, CropTypeSimpleDto cropTypeSimpleDto);
    }
}