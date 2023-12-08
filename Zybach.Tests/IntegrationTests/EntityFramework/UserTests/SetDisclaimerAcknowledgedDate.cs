using System;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    public class SetDisclaimerAcknowledgedDate : UserTestClass
    {
        [TestMethod]
        public void CanSetDisclaimerAcknowledgedDate()
        {
            var updatedUser = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, int, UserDto>(User.SetDisclaimerAcknowledgedDate), Lap, TestResults, true, AssemblySteps.DbContext, NewUser.UserID);
            Assert.IsNotNull(updatedUser.UpdateDate);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanSetDisclaimerAcknowledgedDate_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanSetDisclaimerAcknowledgedDate();
            }
        }
    }
}
