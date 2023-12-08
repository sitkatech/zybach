using System;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    public class GetByUserGuid : UserTestClass
    {
        [TestMethod]
        public void CanGetUserByUserGuid()
        {
            var user = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, Guid, UserDto>(User.GetByUserGuid), Lap, TestResults, true, AssemblySteps.DbContext, NewUser.UserGuid.GetValueOrDefault());
            UserTestHelper.AssertUserDtosAreEqualAndNotNull(NewUser, user);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanGetUserByUserGuid_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanGetUserByUserGuid();
            }
        }

        [TestMethod]
        public void CanNotGetUserByUserGuidWithUserGuid()
        {
            var user = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, Guid, UserDto>(User.GetByUserGuid), Lap, TestResults, true, AssemblySteps.DbContext, Guid.NewGuid());
            Assert.IsNull(user);
        }
    }
}
