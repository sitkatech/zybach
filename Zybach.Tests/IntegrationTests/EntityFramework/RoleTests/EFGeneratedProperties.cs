using System.Linq;
using Zybach.EFModels.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.RoleTests
{
    [TestClass]
    [Ignore]
    public class EFGeneratedProperties
    {
        [TestMethod]
        public void CanAccessEFGeneratedProperties()
        {
            //MK 12/21/2021 - This is mostly useless but it does get full coverage on the generated properties.
            var roles = Role.All;

            var role = roles.FirstOrDefault();

            Assert.IsNotNull(role);

            var asSimpleDto = role.AsSimpleDto();
            Assert.IsNotNull(asSimpleDto);
        }
    }
}
