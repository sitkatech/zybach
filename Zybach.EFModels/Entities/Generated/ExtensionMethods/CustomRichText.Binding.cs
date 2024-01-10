//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[CustomRichText]
namespace Zybach.EFModels.Entities
{
    public partial class CustomRichText
    {
        public int PrimaryKey => CustomRichTextID;
        public CustomRichTextType CustomRichTextType => CustomRichTextType.AllLookupDictionary[CustomRichTextTypeID];

        public static class FieldLengths
        {

        }
    }
}