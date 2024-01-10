using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.RoleTests
{
    [TestClass]
    public class List : RoleTestClass
    {
        private int _roleCount;

        [TestInitialize]
        public void TestInitialize()
        {
            _roleCount = Role.All.Count;
        }

        [TestMethod]
        public void CanGetList()
        {
            //var roles = MethodHelper.ProfileMethod<IEnumerable<RoleDto>>(new Func<IEnumerable<RoleDto>>(Roles.List), Lap, TestResults, true).ToList();
            //Assert.IsNotNull(roles);
            //Assert.AreEqual(_roleCount, roles.Count);
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
