using System;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.CustomRichTextTests
{
    [TestClass]
    public class GetByCustomRichTextTypeID : BaseTestClass
    {
        [TestInitialize]


        [TestMethod]
        public void CanGetByCustomRichTextTypeID()
        {
            var customRichTextTypeID = 1; //(int)CustomRichTextType.CustomRichTextTypeEnum.Homepage; //MK 1/4/2022 - Not sure why the enum doesn't exist here but does in Longbeach... Magic number to move on, 1 should be inserted via lookup scripts.
            var customRichText = MethodHelper.ProfileMethod<CustomRichTextDto>(new Func<ZybachDbContext, int, CustomRichTextDto>((dbContext, customRichTextTypeID1) => CustomRichTexts.GetByCustomRichTextTypeID(dbContext, customRichTextTypeID1)), Lap, TestResults, true, AssemblySteps.DbContext, customRichTextTypeID);

            Assert.IsNotNull(customRichText);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanGetByCustomRichTextTypeID_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanGetByCustomRichTextTypeID();
            }
        }

        [TestMethod]
        public void CanNotGetByCustomRichTextTypeIDWithBogusID()
        {
            var customRichText = MethodHelper.ProfileMethod<CustomRichTextDto>(new Func<ZybachDbContext, int, CustomRichTextDto>((dbContext, customRichTextTypeID) => CustomRichTexts.GetByCustomRichTextTypeID(dbContext, customRichTextTypeID)), Lap, TestResults, true, AssemblySteps.DbContext, -100);
            Assert.IsNull(customRichText);
        }
    }
}
