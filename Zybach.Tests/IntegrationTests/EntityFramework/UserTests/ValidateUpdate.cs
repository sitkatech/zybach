using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Models.DataTransferObjects.User;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    public class ValidateUpdate : UserTestClass
    {
        [TestMethod]
        public void CanValidateUpdateAndGetNoMessages()
        {
            var userUpdateDto = new UserUpsertDto()
            {
                RoleID = (int)RoleEnum.Admin,
                ReceiveSupportEmails = false
            };

            var validation = MethodHelper.ProfileMethod<List<ErrorMessage>>(new Func<ZybachDbContext, UserUpsertDto, int, List<ErrorMessage>>(Users.ValidateUpdate), Lap, TestResults, true, AssemblySteps.DbContext, userUpdateDto, NewUser.UserID);
            Assert.AreEqual(0, validation.Count);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanValidateUpdateAndGetNoMessages_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanValidateUpdateAndGetNoMessages();
            }
        }

        [TestMethod]
        public void CanValidateUpdateAndGetRoleIDMessage()
        {
            var userUpdateDto = new UserUpsertDto()
            {
                ReceiveSupportEmails = false
            };

            var validation = MethodHelper.ProfileMethod<List<ErrorMessage>>(new Func<ZybachDbContext, UserUpsertDto, int, List<ErrorMessage>>(Users.ValidateUpdate), Lap, TestResults, true, AssemblySteps.DbContext, userUpdateDto, NewUser.UserID);
            var errorTypes = validation.Select(x => x.Type);
            Assert.IsTrue(errorTypes.Contains("Role ID"));
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanValidateUpdateAndGetRoleIDMessage_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanValidateUpdateAndGetRoleIDMessage();
            }
        }
    }
}
