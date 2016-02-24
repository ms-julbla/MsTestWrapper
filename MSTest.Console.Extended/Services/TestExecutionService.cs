using System.Collections.Generic;
using System.Linq;
using log4net;
using MSTest.Console.Extended.Data;
using MSTest.Console.Extended.Interfaces;

namespace MSTest.Console.Extended.Services
{
    public class TestExecutionService
    {
        private readonly ILog log;

        private readonly IMsTestTestRunProvider microsoftTestTestRunProvider;

        private readonly IFileSystemProvider fileSystemProvider;

        private readonly IProcessExecutionProvider processExecutionProvider;

        private readonly IConsoleArgumentsProvider consoleArgumentsProvider;

        public TestExecutionService(
            IMsTestTestRunProvider microsoftTestTestRunProvider,
            IFileSystemProvider fileSystemProvider,
            IProcessExecutionProvider processExecutionProvider,
            IConsoleArgumentsProvider consoleArgumentsProvider,
            ILog log)
        {
            this.microsoftTestTestRunProvider = microsoftTestTestRunProvider;
            this.fileSystemProvider = fileSystemProvider;
            this.processExecutionProvider = processExecutionProvider;
            this.consoleArgumentsProvider = consoleArgumentsProvider;
            this.log = log;
        }
        
        public int ExecuteWithRetry()
        {
            this.fileSystemProvider.DeleteTestResultFiles();

            this.log.InfoFormat("Iteration #1  - First call to MsText.exe");
            this.processExecutionProvider.ExecuteProcessWithAdditionalArguments();
            this.processExecutionProvider.CurrentProcessWaitForExit();

            var masterTestRun = this.fileSystemProvider.DeserializeTestRun();      
      
            var failedTests = new List<string>();
            failedTests = this.microsoftTestTestRunProvider.GetNamesOfNotPassedTests(masterTestRun);

            int failedTestsPercentage = this.microsoftTestTestRunProvider.CalculatedFailedTestsPercentage(masterTestRun);
            if (failedTestsPercentage <= this.consoleArgumentsProvider.FailedTestsThreshold)
            {
                for (int iteration = 0; iteration <= this.consoleArgumentsProvider.RetriesCount + 1; iteration++)
                {
                    // If we made all test succeed, exit!
                    if (failedTests.Count == 0)
                    {
                        break;
                    }

                    this.log.InfoFormat("Iteration #{0,2} - Rerunning {1} failed tests.", iteration + 2, failedTests.Count);

                    string currentTestResultPath = this.fileSystemProvider.GetTempTrxFile();
                    string retryRunArguments = this.microsoftTestTestRunProvider.GenerateAdditionalArgumentsForFailedTestsRun(failedTests, currentTestResultPath);    
                    this.log.InfoFormat("\tMsTest.exe Arguments : {0}", retryRunArguments);

                    this.processExecutionProvider.ExecuteProcessWithAdditionalArguments(retryRunArguments);
                    this.processExecutionProvider.CurrentProcessWaitForExit();
                        
                    var currentTestRun = this.fileSystemProvider.DeserializeTestRun(currentTestResultPath);

                    this.microsoftTestTestRunProvider.UpdateMasterRunWithNewIteration(iteration, masterTestRun, currentTestRun);

                    failedTests = this.microsoftTestTestRunProvider.GetNamesOfNotPassedTests(currentTestRun);
                }
            }

            this.fileSystemProvider.SerializeTestRun(masterTestRun);

            int returnCode = failedTests.Count > 0 ? 1 : 0;
            return returnCode;
        }
    }
}