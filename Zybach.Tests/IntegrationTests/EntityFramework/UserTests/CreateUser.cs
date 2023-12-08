using System;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Models.DataTransferObjects.User;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    [Ignore]
    public class CreateInvitedUser : UserTestClass
    {
        [TestMethod]
        public void CanCreateNewUser()
        {
            var upsertDto = UserTestHelper.GetTestUserUpsertDto();
            var guid = Guid.NewGuid();
            var loginName = "Test User";
            var newUser = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, UserUpsertDto, string, Guid, UserDto>(Users.CreateNewUser), Lap, TestResults, true, AssemblySteps.DbContext, upsertDto, loginName, guid);
            UserTestHelper.AssertUserDtosAreEqualAndNotNull(upsertDto, newUser);
            UsersToCleanUp.Add(newUser);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanCreateNewUser_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanCreateNewUser();
            }
        }

        [TestMethod]
        public void CanNotCreateNewUserWithoutRoleID()
        {
            var upsertDto = UserTestHelper.GetTestUserUpsertDto(null);
            var guid = Guid.NewGuid();
            var loginName = "Test User";
            var newUser = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, UserUpsertDto, string, Guid, UserDto>(Users.CreateNewUser), Lap, TestResults, true, AssemblySteps.DbContext, upsertDto, loginName, guid);
            Assert.IsNull(newUser);
        }
    }
}