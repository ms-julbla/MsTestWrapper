using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTest.Console.Extended.TRX;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MSTest.Console.Extended.Managers
{
    public class TestRunManager
    {
        #region Public Methods

        /// <summary>
        /// Update the master test run with the results of another iteration
        /// </summary>
        /// <remarks>
        /// Inserting an execution into a .trx file requires touching the file in three places:
        ///    1. An Execution element must be placed in the associated master test's TestDefinition
        ///    2. The UnitTestResult element (and associated sub-elements) must put in the master
        ///       file's Results section
        ///    3. A TestEntry element must be placed in the master file's TestEntries element, to link
        ///       together the testId, exectionId, and defaultListId
        /// </remarks>
        /// <param name="iterationNumber">The number of the current iterationNumber</param>
        /// <param name="masterTestRun">The master test run to update</param>
        /// <param name="iterativeTestRun">The iterative test run to add to the master run</param>
        public void UpdateMasterRunWithNewIteration(int iteration, TestRun masterTestRun, TestRun iterativeTestRun)
        {
            // We'll just use the first TestList id we come across
            //     TODO: smartly see if there is a matching test list name, and use that id on 
            //           a test-by-test basis, so that test lists can be kept intact
            var defaultListId = masterTestRun.TestLists.First().id;

            // Go through all tests in the iterative run
            foreach(var iterativeTestResult in iterativeTestRun.Results)
            {
                // Make a copy, so we don't mess with iterativeTestRun
                var iterativeTestResultCopy = DeepCopy(iterativeTestResult);

                // Set all prior executions of this testId as inconclusive
                SetTestResultsInconclusive(iterativeTestResultCopy.testId, masterTestRun);

                // Add <Execution testId="xxxx"> element
                var masterTestDefinition = masterTestRun.TestDefinitions.Where(x => x.id == iterativeTestResultCopy.testId).First();
                TestRunUnitTestExecution execution = new TestRunUnitTestExecution();
                execution.id = iterativeTestResultCopy.executionId;
                masterTestDefinition.Executions.Add(execution);

                // Add <TestEntry testId="xxxx", executionId="yyyy", defaultListId="zzzz" > element
                var testEntry = new TestRunTestEntry();
                testEntry.executionId = iterativeTestResultCopy.executionId;
                testEntry.testId = masterTestDefinition.id;
                testEntry.testListId = defaultListId;
                masterTestRun.TestEntries.Add(testEntry);

                // Add the <TestResult> element
                iterativeTestResultCopy.testListId = defaultListId;
                masterTestRun.Results.Add(iterativeTestResultCopy);
            }

            // Group test results by name, then by execution time
            masterTestRun.Results.Sort(new TestResultComparer());

            UpdateResultsSummary(masterTestRun);
            UpdateEndTime(masterTestRun);
        }

        
        /// <summary>
        /// Gets the names of all tests in a test run whose executions that didn't pass.
        /// </summary>
        /// <param name="testRun">Test run to examine</param>
        /// <returns>List of distinct failed test names</returns>
        public List<string> GetNamesOfNotPassedTests(TestRun testRun)
        {
            List<string> testNames = new List<string>();

            testNames = testRun.Results.Where(x => !x.outcome.Equals(Constants.TestOutcomes.Passed))
                                       .Select(y => y.testName)
                                       .Distinct()
                                       .ToList();
            return testNames;
        }

        /// <summary>
        /// Calculate the percentage of tests that need to be repeated
        /// </summary>
        /// <param name="run">The test run whose tests to examine</param>
        /// <returns>The percentage of tests that need to be repeated</returns>
        public int CalculateRepeatPercentage(TestRun run)
        {
            double result = 0;

            if (run.Results.Count > 0)
            {
                var notPassedTests = this.GetNamesOfNotPassedTests(run);

                result = ((double)notPassedTests.Count / (double)run.Results.Count) * 100;
            }
            
            return (int)result;
        }

        #endregion

        #region Private Functions

        private void UpdateEndTime(TestRun testRun)
        {
            var lastTestEndTime = testRun.Results.Select(x => x.endTime)
                                                 .Max();

            testRun.Times.finish = lastTestEndTime;
        }

        private void UpdateResultsSummary(TestRun testRun)
        {
            testRun.ResultSummary.Counters.passed = (byte) testRun.Results.Where(x => x.outcome.Equals(Constants.TestOutcomes.Passed)).Count();
            testRun.ResultSummary.Counters.failed = (byte) testRun.Results.Where(x => x.outcome.Equals(Constants.TestOutcomes.Failed)).Count();
            testRun.ResultSummary.outcome = testRun.ResultSummary.Counters.failed == 0 ? Constants.TestOutcomes.Passed : Constants.TestOutcomes.Failed;
        }

        private void SetTestResultsInconclusive(string testId, TestRun testRun)
        {
            testRun.Results.Where(x => x.testId.Equals(testId))
                           .ToList()
                           .ForEach(matchingTestResult =>
                               matchingTestResult.outcome = Constants.TestOutcomes.Inconclusive
                           );
        }

        private static T DeepCopy<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        private class TestResultComparer : IComparer<TestRunUnitTestResult>
        {
            /// <summary>
            /// Compares the two test results, first by test name, then by time of execution start
            /// </summary>
            /// <param name="x">First test result to compare</param>
            /// <param name="y">Second test result to compare</param>
            /// <returns>Comparison int</returns>
            public int Compare(TestRunUnitTestResult x, TestRunUnitTestResult y)
            {
                int result = x.testName.CompareTo(y.testName);

                if (result == 0)
                {
                    result = x.startTime.CompareTo(y.startTime);
                }

                return result;
            }
        }

        #endregion
    }
}