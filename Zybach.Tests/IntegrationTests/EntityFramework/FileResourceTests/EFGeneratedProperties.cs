using System.Linq;
using Zybach.EFModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.FileResourceTests
{
    [TestClass]
    public class EFGeneratedProperties : FileResourceTestClass
    {
        [TestMethod]
        public void CanAccessEFGeneratedProperties()
        {
            var fileResourceID = NewFileResource.FileResourceID;
            //MK 12/21/2021 - This is mostly useless but it does get full coverage on the generated properties.
            var fileResource = AssemblySteps.DbContext.FileResources
                .Include(x => x.CreateUser)
                .AsNoTracking()
                .FirstOrDefault(x => x.FileResourceID == fileResourceID);

            Assert.IsNotNull(fileResource);
            Assert.IsNotNull(fileResource.FileResourceID);
            Assert.IsNotNull(fileResource.FileResourceGUID);

            fileResource.FileResourceID = fileResource.FileResourceID;
            fileResource.CreateUser = fileResource.CreateUser;

            var asDto = fileResource.AsDto();
            Assert.IsNotNull(asDto);

            var asSimpleDto = fileResource.AsSimpleDto();
            Assert.IsNotNull(asSimpleDto);
        }
    }
}
