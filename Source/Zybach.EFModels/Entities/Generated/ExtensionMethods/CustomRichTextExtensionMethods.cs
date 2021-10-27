//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[CustomRichText]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class CustomRichTextExtensionMethods
    {
        public static CustomRichTextDto AsDto(this CustomRichText customRichText)
        {
            var customRichTextDto = new CustomRichTextDto()
            {
                CustomRichTextID = customRichText.CustomRichTextID,
                CustomRichTextType = customRichText.CustomRichTextType.AsDto(),
                CustomRichTextContent = customRichText.CustomRichTextContent
            };
            DoCustomMappings(customRichText, customRichTextDto);
            return customRichTextDto;
        }

        static partial void DoCustomMappings(CustomRichText customRichText, CustomRichTextDto customRichTextDto);

    }
}