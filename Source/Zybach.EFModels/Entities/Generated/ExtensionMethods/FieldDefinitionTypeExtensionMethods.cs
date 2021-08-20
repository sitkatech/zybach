//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[FieldDefinitionType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class FieldDefinitionTypeExtensionMethods
    {
        public static FieldDefinitionTypeDto AsDto(this FieldDefinitionType fieldDefinitionType)
        {
            var fieldDefinitionTypeDto = new FieldDefinitionTypeDto()
            {
                FieldDefinitionTypeID = fieldDefinitionType.FieldDefinitionTypeID,
                FieldDefinitionTypeName = fieldDefinitionType.FieldDefinitionTypeName,
                FieldDefinitionTypeDisplayName = fieldDefinitionType.FieldDefinitionTypeDisplayName
            };
            DoCustomMappings(fieldDefinitionType, fieldDefinitionTypeDto);
            return fieldDefinitionTypeDto;
        }

        static partial void DoCustomMappings(FieldDefinitionType fieldDefinitionType, FieldDefinitionTypeDto fieldDefinitionTypeDto);

    }
}