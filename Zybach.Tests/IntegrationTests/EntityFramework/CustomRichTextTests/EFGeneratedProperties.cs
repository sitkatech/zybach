using System.Linq;
using Zybach.EFModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.CustomRichTextTests
{
    [TestClass]
    [Ignore]
    public class EFGeneratedProperties
    {
        [TestMethod]
        public void CanAccessEFGeneratedProperties()
        {
            //MK 12/21/2021 - This is mostly useless but it does get full coverage on the generated properties.
            var customRichTexts = AssemblySteps.DbContext.CustomRichTexts.AsNoTracking();

            var customRichText = customRichTexts.FirstOrDefault();

            Assert.IsNotNull(customRichText);
            Assert.IsNotNull(customRichText.CustomRichTextID);
            Assert.IsNotNull(customRichText.CustomRichTextTypeID);
            Assert.IsNotNull(customRichText.CustomRichTextContent);

            customRichText.CustomRichTextID = customRichText.CustomRichTextID;
            customRichText.CustomRichTextTypeID = customRichText.CustomRichTextTypeID;
            customRichText.CustomRichTextContent = customRichText.CustomRichTextContent;

            Assert.IsNotNull(customRichText.CustomRichTextType);
            Assert.IsNotNull(customRichText.CustomRichTextType.CustomRichTextTypeID);
            Assert.IsNotNull(customRichText.CustomRichTextType.CustomRichTextTypeName);
            Assert.IsNotNull(customRichText.CustomRichTextType.CustomRichTextTypeDisplayName);

            var asDto = customRichText.AsDto();
            Assert.IsNotNull(asDto);

            var asSimpleDto = customRichText.AsSimpleDto();
            Assert.IsNotNull(asSimpleDto);

            var typeAsSimpleDto = customRichText.CustomRichTextType.AsSimpleDto();
            Assert.IsNotNull(typeAsSimpleDto);
        }
    }
}
