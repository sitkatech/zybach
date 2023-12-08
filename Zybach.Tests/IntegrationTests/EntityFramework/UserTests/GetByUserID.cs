using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    public class GetByUserID : UserTestClass
    {
        [TestMethod]
        public void CanGetUserByID()
        {
            var user = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, int, UserDto>(Users.GetByUserID), Lap, TestResults, true, AssemblySteps.DbContext, NewUser.UserID);
            UserTestHelper.AssertUserDtosAreEqualAndNotNull(NewUser, user);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanGetUserByID_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanGetUserByID();
            }
        }

        [TestMethod]
        public void CanNotGetUserByIDWithBogusID()
        {
            var user = MethodHelper.ProfileMethod<UserDto>(new Func<ZybachDbContext, int, UserDto>(Users.GetByUserID), Lap, TestResults, true, AssemblySteps.DbContext, -100);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void CanGetUserByID_ListOverload()
        {
            var userIDs = new List<int>()
            {
                NewUser.UserID
            };

            var users = MethodHelper.ProfileMethod<List<UserDto>>(new Func<ZybachDbContext, List<int>, List<UserDto>>(Users.GetByUserID), Lap, TestResults, true, AssemblySteps.DbContext, userIDs);
            Assert.AreEqual(userIDs.Count, users.Count);
            UserTestHelper.AssertUserDtosAreEqualAndNotNull(NewUser, users.First());
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanGetUserByID_ListOverload_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanGetUserByID_ListOverload();
            }
        }
    }
}
