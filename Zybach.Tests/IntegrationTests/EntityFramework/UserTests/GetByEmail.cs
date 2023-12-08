using System;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    public class GetByEmail : UserTestClass
    {
        [TestMethod]
        public void CanGetUserByEmail()
        {
            var user = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, string, UserDto>(User.GetByEmail), Lap, TestResults, true, AssemblySteps.DbContext, NewUser.Email);
            UserTestHelper.AssertUserDtosAreEqualAndNotNull(NewUser, user);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanGetUserByEmail_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanGetUserByEmail();
            }
        }

        [TestMethod]
        public void CanNotGetUserByEmailWithBogusEmail()
        {
            var user = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, string, UserDto>(User.GetByEmail), Lap, TestResults, true, AssemblySteps.DbContext, Guid.NewGuid().ToString());
            Assert.IsNull(user);
        }
    }
}
