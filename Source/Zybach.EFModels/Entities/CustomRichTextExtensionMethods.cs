using System;
using System.Collections.Generic;
using System.Text;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class CustomRichTextExtensionMethods
    {
        public static CustomRichTextDto AsDto(this CustomRichText customRichText)
        {
            return new CustomRichTextDto
            {
                CustomRichTextContent = customRichText.CustomRichTextContent,
                IsEmptyContent = string.IsNullOrWhiteSpace(customRichText.CustomRichTextContent)
            };
        }
    }
}