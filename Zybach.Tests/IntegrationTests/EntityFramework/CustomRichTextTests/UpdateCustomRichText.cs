using System;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.CustomRichTextTests
{
    [TestClass]
    public class UpdateCustomRichText : BaseTestClass
    {
        [TestMethod]
        public void CanUpdateCustomRichText()
        {
            var customRichTextTypeID = 1; //(int)CustomRichTextType.CustomRichTextTypeEnum.Homepage; //MK 1/4/2022 - Not sure why the enum doesn't exist here but does in Longbeach... Magic number to move on, 1 should be inserted via lookup scripts.
            var updatedCustomRichText = new CustomRichTextDto()
            {
                CustomRichTextContent = "<p>updated</p>"
            };

            var customRichText = MethodHelper.ProfileMethod<CustomRichTextDto>(new Func<ZybachDbContext, int, CustomRichTextDto, CustomRichTextDto>(CustomRichTexts.UpdateCustomRichText), Lap, TestResults, true, AssemblySteps.DbContext, customRichTextTypeID, updatedCustomRichText);

            Assert.IsNotNull(customRichText);
            Assert.AreEqual(customRichText.CustomRichTextContent, updatedCustomRichText.CustomRichTextContent);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanUpdateCustomRichText_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanUpdateCustomRichText();
            }
        }
    }
}
