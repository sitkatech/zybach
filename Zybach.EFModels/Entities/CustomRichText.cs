using Zybach.Models.DataTransferObjects;
using System.Linq;

namespace Zybach.EFModels.Entities
{
    public class CustomRichTexts
    {
        public static CustomRichTextDto GetByCustomRichTextTypeID(ZybachDbContext dbContext, int customRichTextTypeID)
        {
            var customRichText = dbContext.CustomRichTexts
                .SingleOrDefault(x => x.CustomRichTextTypeID == customRichTextTypeID);

            return customRichText?.AsDto() ?? CreateIfNotExists(dbContext, customRichTextTypeID).AsDto();
        }

        public static CustomRichText CreateIfNotExists(ZybachDbContext dbContext, int customRichTextTypeID)
        {
            var newCustomRichText = new CustomRichText()
            {
                CustomRichTextTypeID = customRichTextTypeID,
                CustomRichTextContent = $"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>"
            };

            dbContext.CustomRichTexts.Add(newCustomRichText);
            dbContext.SaveChanges();
            dbContext.Entry(newCustomRichText).Reload();

            return newCustomRichText;
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