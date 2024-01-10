using System;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Models.DataTransferObjects.User;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    public class UpdateUserEntity : UserTestClass
    {
        [TestMethod]
        public void CanUpdateUserEntity()
        {
            var userUpdateDto = new UserUpsertDto()
            {
                RoleID = (int)RoleEnum.Normal,
                ReceiveSupportEmails = false
            };

            var updatedUser = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, int, UserUpsertDto, UserDto>(Users.UpdateUserEntity), Lap, TestResults, true, AssemblySteps.DbContext, NewUser.UserID, userUpdateDto);
            Assert.AreEqual(userUpdateDto.RoleID, updatedUser.Role.RoleID);
            Assert.AreEqual(userUpdateDto.ReceiveSupportEmails, updatedUser.ReceiveSupportEmails);
            Assert.IsNotNull(updatedUser.UpdateDate);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanUpdateUserEntity_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanUpdateUserEntity();
            }
        }

        [TestMethod]
        public void CanNotUpdateUserEntityWithoutRoleID()
        {
            var userUpdateDto = new UserUpsertDto()
            {
                ReceiveSupportEmails = false
            };

            var updatedUser = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, int, UserUpsertDto, UserDto>(Users.UpdateUserEntity), Lap, TestResults, true, AssemblySteps.DbContext, NewUser.UserID, userUpdateDto);
            Assert.IsNull(updatedUser);
        }
    }
}
