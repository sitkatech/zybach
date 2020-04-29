﻿using System;
using System.Collections.Generic;
using System.Text;
using DroolTool.Models.DataTransferObjects;

namespace DroolTool.EFModels.Entities
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