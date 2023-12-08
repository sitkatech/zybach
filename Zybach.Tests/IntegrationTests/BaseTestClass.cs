using System.Collections.Generic;
using Zybach.Tests.Helpers;
using Zybach.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework
{
    [Ignore]
    public class BaseTestClass
    {
        public TestContext TestContext { get; set; }

        protected readonly List<TestRunResult> TestResults = new();
        protected int Lap;

        [TestCleanup]
        public void TestCleanup()
        {
            foreach (var result in TestResults)
            {
                result.Success = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            }

            ConsoleHelper.OutputTestResultsToConsole(TestResults);
        }
    }
}