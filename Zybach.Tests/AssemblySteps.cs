using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Zybach.EFModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests
{
    [TestClass]
    public static class AssemblySteps
    {
        public static IConfigurationRoot Configuration => new ConfigurationBuilder().AddJsonFile(@"environment.json").Build();
        public static ZybachDbContext DbContext;

        public static bool Idempotent = true;
        private static bool _runPerformanceLaps = false;

        [AssemblyInitialize]
        public static async Task AssemblyInitialize(TestContext testContext)
        {
        }

        //[AssemblyInitialize]
        public static async Task AssemblyInitialize_NotWorkingRightNow(TestContext testContext)
        {
            _runPerformanceLaps = Configuration["runPerformanceLaps"] == "True";

            var dbCS = Configuration["sqlConnectionString"];
            var dbOptions = new DbContextOptionsBuilder<ZybachDbContext>();
            dbOptions.UseSqlServer(dbCS, x => x.UseNetTopologySuite());
            var dbContext = new ZybachDbContext(dbOptions.Options);

            var created = await dbContext.Database.EnsureCreatedAsync();

            DbContext = dbContext;

            var customRichTextType = CustomRichTextType.All.First(); //MK 1/4/2022 Assumed to be at least one after running the lookup scripts above.
            var customRichText = DbContext.CustomRichTexts.FirstOrDefault(x => x.CustomRichTextTypeID == customRichTextType.CustomRichTextTypeID);
            if (customRichText == null)
            {
                DbContext.CustomRichTexts.Add(new CustomRichText()
                {
                    CustomRichTextTypeID = customRichTextType.CustomRichTextTypeID,
                    CustomRichTextContent = "Test"
                });

                await DbContext.SaveChangesAsync();
            }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
        }

        public static bool RunPerformanceLaps()
        {
            if (_runPerformanceLaps)
            {
                return true;
            }

            Assert.Inconclusive("Performance laps are turned off for this test run. Turn them on in AssemblySteps.");
            return false;
        }
    }
}

