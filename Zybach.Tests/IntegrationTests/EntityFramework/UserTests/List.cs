using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    public class List : UserTestClass
    {
        private int _userCount;

        [TestInitialize]
        public void TestInitialize()
        {
            _userCount = AssemblySteps.DbContext.Users.AsNoTracking().Count();
        }

        [TestMethod]
        public void CanGetList()
        {
            var users = MethodHelper.ProfileMethod<IEnumerable<UserDto>>(new Func<ZybachDbContext, IEnumerable<UserDto>>(Users.List), Lap, TestResults, true, AssemblySteps.DbContext).ToList();
            Assert.IsNotNull(users);
            Assert.AreEqual(_userCount, users.Count);
        }

        [DataTestMethod]
        [DataRow(100)]
        public void CanGetList_PerformanceLaps(int laps)
        {
            if (!AssemblySteps.RunPerformanceLaps())
            {
                return;
            }

            for (var i = 0; i < laps; i++)
            {
                Lap++;
                CanGetList();
            }
        }
    }
}
