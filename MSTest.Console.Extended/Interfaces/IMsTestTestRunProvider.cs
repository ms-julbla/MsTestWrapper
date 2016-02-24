using System.Collections.Generic;
using MSTest.Console.Extended.Data;

namespace MSTest.Console.Extended.Interfaces
{
    public interface IMsTestTestRunProvider
    {
        List<string> GetNamesOfNotPassedTests(TestRun testRun);

        void UpdateMasterRunWithNewIteration(int iterationNumber, TestRun masterTestRun, TestRun interativeTestRun);

        string GenerateAdditionalArgumentsForFailedTestsRun(List<string> failedTests, string newTestResultFilePath);

        int CalculatedFailedTestsPercentage(TestRun run);
    }
}