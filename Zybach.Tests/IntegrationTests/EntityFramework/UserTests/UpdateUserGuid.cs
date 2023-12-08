using System;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    public class UpdateUserGuid : UserTestClass
    {
        [TestMethod]
        public void CanUpdateUserGuid()
        {
            var updatedUserGuid = Guid.NewGuid();
            var updatedUser = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, int, Guid, UserDto>(User.UpdateUserGuid), Lap, TestResults, true, AssemblySteps.DbContext, NewUser.UserID, updatedUserGuid);
            Assert.AreEqual(updatedUserGuid, updatedUser.UserGuid);
            Assert.IsNotNull(updatedUser.UpdateDate);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanUpdateUserGuid_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanUpdateUserGuid();
            }
        }
    }
}
