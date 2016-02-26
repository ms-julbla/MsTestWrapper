using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Console.Extended.TRX;
using MSTest.Console.Extended.Managers;
using Telerik.JustMock;

namespace MSTest.Console.Extended.UnitTests.MsTestTestRunProviderTests
{
    [TestClass]
    public class MsTestTestRunProvider_GetFailedTestsPercent_Should
    {
        [TestMethod]
        public void ReturnZero_WhenNoFailedTestsPresent()
        {
            //var log = Mock.Create<ILog>();
            //Mock.Arrange(() => log.Info(Arg.AnyString));
            //var consoleArgumentsProvider = Mock.Create<IConsoleArgumentsProvider>();

            //var microsoftTestTestRunProvider = new MsTestTestRunProvider(consoleArgumentsProvider, log);
            //List<TestRunUnitTestResult> failedTests = new List<TestRunUnitTestResult>();
            //List<TestRunUnitTestResult> allTests = new List<TestRunUnitTestResult>()
            //{
            //    new TestRunUnitTestResult()
            //};
            //var failedTestsPercentage = microsoftTestTestRunProvider.CalculatedFailedTestsPercentage(failedTests, allTests);

            //Assert.AreEqual<int>(0, failedTestsPercentage);
        }

        [TestMethod]
        public void Return50Percent_WhenOneFailedTestPresentOfTwo()
        {
            //var log = Mock.Create<ILog>();
            //Mock.Arrange(() => log.Info(Arg.AnyString));
            //var consoleArgumentsProvider = Mock.Create<IConsoleArgumentsProvider>();
            
            //var microsoftTestTestRunProvider = new MsTestTestRunProvider(consoleArgumentsProvider, log);
            //List<TestRunUnitTestResult> failedTests = new List<TestRunUnitTestResult>()
            //{
            //    new TestRunUnitTestResult()
            //};
            //List<TestRunUnitTestResult> allTests = new List<TestRunUnitTestResult>()
            //{
            //    new TestRunUnitTestResult(),
            //    new TestRunUnitTestResult()
            //};
            //var failedTestsPercentage = microsoftTestTestRunProvider.CalculatedFailedTestsPercentage(failedTests, allTests);

            //Assert.AreEqual<int>(50, failedTestsPercentage);
        }

        [TestMethod]
        public void ReturnZeroPercent_WhenNoTestsPresent()
        {
            //var log = Mock.Create<ILog>();
            //Mock.Arrange(() => log.Info(Arg.AnyString));
            //var consoleArgumentsProvider = Mock.Create<IConsoleArgumentsProvider>();
            
            //var microsoftTestTestRunProvider = new MsTestTestRunProvider(consoleArgumentsProvider, log);
            //List<TestRunUnitTestResult> failedTests = new List<TestRunUnitTestResult>();
            //List<TestRunUnitTestResult> allTests = new List<TestRunUnitTestResult>();
            //var failedTestsPercentage = microsoftTestTestRunProvider.CalculatedFailedTestsPercentage(failedTests, allTests);

            //Assert.AreEqual<int>(0, failedTestsPercentage);
        }
    }
}