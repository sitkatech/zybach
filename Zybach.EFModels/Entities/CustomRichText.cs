using Zybach.Models.DataTransferObjects;
using System.Linq;

namespace Zybach.EFModels.Entities
{
    public partial class CustomRichText
    {
        public static CustomRichTextDto GetByCustomRichTextTypeID(ZybachDbContext dbContext, int customRichTextTypeID)
        {
            var customRichText = dbContext.CustomRichTexts
                .SingleOrDefault(x => x.CustomRichTextTypeID == customRichTextTypeID);

            return customRichText?.AsDto();
        }

        public static CustomRichTextDto UpdateCustomRichText(ZybachDbContext dbContext, int customRichTextTypeID,
            CustomRichTextDto customRichTextUpdateDto)
        {
            var customRichText = dbContext.CustomRichTexts
                .SingleOrDefault(x => x.CustomRichTextTypeID == customRichTextTypeID);

            // null check occurs in calling endpoint method.
            customRichText.CustomRichTextContent = customRichTextUpdateDto.CustomRichTextContent;

            dbContext.SaveChanges();

            return customRichText.AsDto();
        }
    }
}