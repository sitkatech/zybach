using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.EFModels.Entities;
using Zybach.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    public class GetEmailAddressesForAdminsThatReceiveSupportEmails : UserTestClass
    {
        private int _emailCount;

        [TestInitialize]
        public void TestInitialize()
        {
            _emailCount = AssemblySteps.DbContext.Users.AsNoTracking().Count(x => x.RoleID == (int)RoleEnum.Admin && x.ReceiveSupportEmails);
        }

        [TestMethod]
        public void CanGetEmailAddressesForAdminsThatReceiveSupportEmails()
        {
            var emails = MethodHelper.ProfileMethod<IEnumerable<string>>(new Func<ZybachDbContext, IEnumerable<string>>(Users.GetEmailAddressesForAdminsThatReceiveSupportEmails), Lap, TestResults, true, AssemblySteps.DbContext);
            Assert.IsNotNull(emails);
            Assert.AreEqual(_emailCount, emails.Count());
        }
    }
}
