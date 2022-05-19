//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETDataType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class OpenETDataTypeExtensionMethods
    {
        public static OpenETDataTypeDto AsDto(this OpenETDataType openETDataType)
        {
            var openETDataTypeDto = new OpenETDataTypeDto()
            {
                OpenETDataTypeID = openETDataType.OpenETDataTypeID,
                OpenETDataTypeName = openETDataType.OpenETDataTypeName,
                OpenETDataTypeDisplayName = openETDataType.OpenETDataTypeDisplayName,
                OpenETDataTypeVariableName = openETDataType.OpenETDataTypeVariableName
            };
            DoCustomMappings(openETDataType, openETDataTypeDto);
            return openETDataTypeDto;
        }

        static partial void DoCustomMappings(OpenETDataType openETDataType, OpenETDataTypeDto openETDataTypeDto);

        public static OpenETDataTypeSimpleDto AsSimpleDto(this OpenETDataType openETDataType)
        {
            var openETDataTypeSimpleDto = new OpenETDataTypeSimpleDto()
            {
                OpenETDataTypeID = openETDataType.OpenETDataTypeID,
                OpenETDataTypeName = openETDataType.OpenETDataTypeName,
                OpenETDataTypeDisplayName = openETDataType.OpenETDataTypeDisplayName,
                OpenETDataTypeVariableName = openETDataType.OpenETDataTypeVariableName
            };
            DoCustomSimpleDtoMappings(openETDataType, openETDataTypeSimpleDto);
            return openETDataTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(OpenETDataType openETDataType, OpenETDataTypeSimpleDto openETDataTypeSimpleDto);
    }
}