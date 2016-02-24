using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using MSTest.Console.Extended.Data;
using MSTest.Console.Extended.Interfaces;

namespace MSTest.Console.Extended.Infrastructure
{
    public class MsTestTestRunProvider : IMsTestTestRunProvider
    {
        private readonly ILog log;
        private readonly IConsoleArgumentsProvider consoleArgumentsProvider;

        public MsTestTestRunProvider(IConsoleArgumentsProvider consoleArgumentsProvider, ILog log)
        {
            this.consoleArgumentsProvider = consoleArgumentsProvider;
            this.log = log;
        }

        /// <summary>
        /// Update the master test run with the results of another iteration
        /// over the failed tests
        /// 
        /// THIS WILL CASUE SIDE EFFECTS ON THE ITERATIVE TEST RUN
        /// </summary>
        /// <remarks>
        /// Inserting an execution into a .trx file requires touching the file in three places:
        ///    1. An Execution element must be placed in the associated master test's TestDefinition
        ///    2. The UnitTestResult element (and associated sub-elements) must put in the master
        ///       file's Results section
        ///    3. A TestEntry element must be placed in the master file's TestEntries element, to link
        ///       together the testId, exectionId, and testListId
        /// </remarks>
        /// <param name="masterTestRun"></param>
        /// <param name="iterativeTestRun"></param>
        void UpdateMasterRunWithNewIteration(TestRun masterTestRun, TestRun iterativeTestRun)
        {
            // We'll just use the first TestList id we come across
            //     TODO: smartly see if there is a matching test list name, and use that id on 
            //           a test-by-test basis, so that test lists can be kept intact
            var testListId = masterTestRun.TestLists.First().id;
            var masterResults = masterTestRun.Results.ToList();
            var masterTestEntries = masterTestRun.TestEntries.ToList();

            // Go through all tests in the iterative run
            foreach(var iterativeTestResult in iterativeTestRun.Results)
            {
                // Add <Execution testId="xxxx"> element
                var masterTestDefinition = masterTestRun.TestDefinitions.Where(x => x.id == iterativeTestResult.testId).First();
                TestRunUnitTestExecution execution = new TestRunUnitTestExecution();
                execution.id = iterativeTestResult.executionId;
                
                var executionList = masterTestDefinition.Executions.ToList();
                executionList.Add(execution);
                masterTestDefinition.Executions = executionList.ToArray();

                // Add the <TestResult> element (first, link up the ids correctly)
                iterativeTestResult.testId = masterTestDefinition.id;
                iterativeTestResult.testListId = testListId;
                masterResults.Add(iterativeTestResult);

                // Add <TestEntry testId="xxxx", executionId="yyyy", testListId="zzzz" > element

                var testEntry = new TestRunTestEntry();
                testEntry.executionId = iterativeTestResult.executionId;
                testEntry.testId = masterTestDefinition.id;
                testEntry.testListId = testListId;
                masterTestEntries.Add(testEntry);
            }

            masterTestRun.TestEntries = masterTestEntries.ToArray();
            masterTestRun.Results = masterResults.ToArray();

            // TODO: Sort this
            // TODO: Change names (or something) to clarify which iteration each test is
            // TODO: Update total success count
        }


        
        public List<string> GetNamesOfNotPassedTests(TestRun testRun)
        {
            List<string> testNames = new List<string>();

            testNames = testRun.Results.Where(x => !x.outcome.Equals("Passed"))
                                       .Select(y => y.testName)
                                       .ToList();
            return testNames;
        }

        public string GenerateAdditionalArgumentsForFailedTestsRun(List<string> failedTestNames, string newTestResultFilePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");
            foreach (var testName in failedTestNames)
            {
                sb.AppendFormat("/test:{0} ", testName);
                System.Console.WriteLine("##### MSTestRetrier: Execute again {0}", testName);
                this.log.InfoFormat("##### MSTestRetrier: Execute again {0}", testName);
            }

            var keptConsoleArguments = this.consoleArgumentsProvider.ConsoleArguments;

            // Don't include original /test parameter (else we'll end up repeating all tests!!)
            const string testsArgRegexPattern = @".*(?<TestArgument>/(?i)test(?-i):(?<TestValue>.*))";
            var testRex = new System.Text.RegularExpressions.Regex(testsArgRegexPattern, System.Text.RegularExpressions.RegexOptions.Singleline);
            var match = testRex.Match(this.consoleArgumentsProvider.ConsoleArguments);
            
            if (match.Success)
            {
                keptConsoleArguments = keptConsoleArguments.Replace(match.Groups["TestArgument"].Value, string.Empty);
            }

            string additionalArgumentsForFailedTestsRun = string.Concat(keptConsoleArguments, sb.ToString());
            additionalArgumentsForFailedTestsRun = additionalArgumentsForFailedTestsRun.Replace(this.consoleArgumentsProvider.TestResultPath, newTestResultFilePath);
            additionalArgumentsForFailedTestsRun = additionalArgumentsForFailedTestsRun.TrimEnd();
            return additionalArgumentsForFailedTestsRun;
        }

        public int CalculatedFailedTestsPercentage(TestRun run)
        {
            double result = 0;

            if (run.Results.Length > 0)
            {
                var failedTests = this.GetNamesOfNotPassedTests(run);

                result = ((double)failedTests.Count / (double)run.Results.Length) * 100;
            }
            
            return (int)result;
        }
    }
}