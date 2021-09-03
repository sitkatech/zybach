//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[CustomRichTextType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class CustomRichTextTypeExtensionMethods
    {
        public static CustomRichTextTypeDto AsDto(this CustomRichTextType customRichTextType)
        {
            var customRichTextTypeDto = new CustomRichTextTypeDto()
            {
                CustomRichTextTypeID = customRichTextType.CustomRichTextTypeID,
                CustomRichTextTypeName = customRichTextType.CustomRichTextTypeName,
                CustomRichTextTypeDisplayName = customRichTextType.CustomRichTextTypeDisplayName
            };
            DoCustomMappings(customRichTextType, customRichTextTypeDto);
            return customRichTextTypeDto;
        }

        static partial void DoCustomMappings(CustomRichTextType customRichTextType, CustomRichTextTypeDto customRichTextTypeDto);

    }
}