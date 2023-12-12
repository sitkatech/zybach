using System.Linq;
using Zybach.EFModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    [TestClass]
    [Ignore]
    public class EFGeneratedProperties
    {
        [TestMethod]
        public void CanAccessEFGeneratedProperties()
        {
            //MK 12/21/2021 - This is mostly useless but it does get exercise and get full coverage on the generated properties.
            var users = AssemblySteps.DbContext.Users.AsNoTracking();

            var user = users.FirstOrDefault();

            Assert.IsNotNull(user);
            Assert.IsNotNull(user.UserID);
            Assert.IsNotNull(user.RoleID);
            Assert.IsNotNull(user.Role);

            user.UserID = user.UserID;
            user.Phone = user.Phone;
            user.LastActivityDate = user.LastActivityDate;
            user.Company = user.Company;

            var asSimpleDto = user.AsSimpleDto();
            Assert.IsNotNull(asSimpleDto);
        }
    }
}
