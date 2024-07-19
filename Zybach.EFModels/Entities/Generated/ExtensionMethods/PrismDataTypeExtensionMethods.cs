//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismDataType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class PrismDataTypeExtensionMethods
    {
        public static PrismDataTypeDto AsDto(this PrismDataType prismDataType)
        {
            var prismDataTypeDto = new PrismDataTypeDto()
            {
                PrismDataTypeID = prismDataType.PrismDataTypeID,
                PrismDataTypeName = prismDataType.PrismDataTypeName,
                PrismDataTypeDisplayName = prismDataType.PrismDataTypeDisplayName
            };
            DoCustomMappings(prismDataType, prismDataTypeDto);
            return prismDataTypeDto;
        }

        static partial void DoCustomMappings(PrismDataType prismDataType, PrismDataTypeDto prismDataTypeDto);

        public static PrismDataTypeSimpleDto AsSimpleDto(this PrismDataType prismDataType)
        {
            var prismDataTypeSimpleDto = new PrismDataTypeSimpleDto()
            {
                PrismDataTypeID = prismDataType.PrismDataTypeID,
                PrismDataTypeName = prismDataType.PrismDataTypeName,
                PrismDataTypeDisplayName = prismDataType.PrismDataTypeDisplayName
            };
            DoCustomSimpleDtoMappings(prismDataType, prismDataTypeSimpleDto);
            return prismDataTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(PrismDataType prismDataType, PrismDataTypeSimpleDto prismDataTypeSimpleDto);
    }
}