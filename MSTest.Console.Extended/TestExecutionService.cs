using MSTest.Console.Extended.Utilities;
using System.Collections.Generic;
using System.IO;

namespace MSTest.Console.Extended.Managers
{
    public class TestExecutionService
    {
        private readonly TestRunManager testRunProvider;

        private readonly MsTestProcessManager testProcessManager;

        private readonly ConsoleArgumentsManager consoleArgumentsProvider;

        public TestExecutionService(
            TestRunManager microsoftTestTestRunProvider,
            MsTestProcessManager processExecutionProvider,
            ConsoleArgumentsManager consoleArgumentsProvider)
        {
            this.testRunProvider = microsoftTestTestRunProvider;
            this.testProcessManager = processExecutionProvider;
            this.consoleArgumentsProvider = consoleArgumentsProvider;
        }
        
        public int ExecuteWithRetry()
        {
            if (this.consoleArgumentsProvider.ShouldDeleteOldTestResultFiles && File.Exists(consoleArgumentsProvider.TestResultPath))
            {
                File.Delete(consoleArgumentsProvider.TestResultPath);
            }
            

            System.Console.Out.WriteLine("Iteration #1  - First call to MsText.exe");
            this.testProcessManager.ExecuteInitialRun(consoleArgumentsProvider);

            var masterTestRun = FileSystemTools.DeserializeTestRun(consoleArgumentsProvider.TestResultPath);      
      
            var testsToRepeat = new List<string>();
            testsToRepeat = this.testRunProvider.GetNamesOfNotPassedTests(masterTestRun);

            if (testRunProvider.CalculateRepeatPercentage(masterTestRun) <= this.consoleArgumentsProvider.FailedTestsThreshold)
            {
                // We've already done the first iteration. This loop performs the rest
                for (int iterationNumber = 2; iterationNumber <= this.consoleArgumentsProvider.RetriesCount + 1; iterationNumber++)
                {
                    // If we made all test succeed, exit!
                    if (testsToRepeat.Count == 0)
                    {
                        break;
                    }

                    System.Console.Out.WriteLine("Iteration #{0,2} - Rerunning {1} failed tests.", iterationNumber, testsToRepeat.Count);

                    string tempTestResultsPath = FileSystemTools.GetTempTrxFile();
                    testProcessManager.ExecuteIterativeRun(testsToRepeat, tempTestResultsPath, consoleArgumentsProvider);

                    var currentIterativeTestRun = FileSystemTools.DeserializeTestRun(tempTestResultsPath);

                    // Update master results file
                    this.testRunProvider.UpdateMasterRunWithNewIteration(iterationNumber, masterTestRun, currentIterativeTestRun);
                    
                    if (File.Exists(this.consoleArgumentsProvider.NewTestResultPath))
                    {
                        File.Delete(this.consoleArgumentsProvider.NewTestResultPath); 
                    }
                    FileSystemTools.SerializeTestRun(masterTestRun, consoleArgumentsProvider.NewTestResultPath);

                    testsToRepeat = this.testRunProvider.GetNamesOfNotPassedTests(currentIterativeTestRun);
                }
            }

            FileSystemTools.SerializeTestRun(masterTestRun, consoleArgumentsProvider.NewTestResultPath);

            int returnCode = testsToRepeat.Count > 0 ? 1 : 0;
            return returnCode;
        }
    }
}