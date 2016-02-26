using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MSTest.Console.Extended.Managers
{
    public class MsTestProcessManager
    {
        private readonly string MicrosoftTestConsoleExePath;

        public MsTestProcessManager(string microsoftTestConsoleExePath)
        {
            this.MicrosoftTestConsoleExePath = microsoftTestConsoleExePath;
        }

        public void ExecuteInitialRun(ConsoleArgumentsManager consoleArgumentProvider)
        {
            ExecuteWithArguments(consoleArgumentProvider.InitialConsoleArguments);
        }

        /// <summary>
        /// Execute an test run of the specified tests
        /// </summary>
        /// <param name="repeatTestNames">The tests to execute</param>
        /// <param name="resultsTrxPath">Path to save the .trx results file</param>
        /// <param name="consoleArgumentsProvider"></param>
        public void ExecuteIterativeRun(List<string> repeatTestNames, string resultsTrxPath, ConsoleArgumentsManager consoleArgumentsProvider)
        {
            ExecuteWithArguments(GenerateArgumentsForRepeatTestRun(repeatTestNames, resultsTrxPath, consoleArgumentsProvider));
        }

        #region Private Methods

        /// <summary>
        /// Execute MsTest.exe with the given arguments
        /// </summary>
        /// <param name="arguments">Arguments for MsTest.exe</param>
        private void ExecuteWithArguments(string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(this.MicrosoftTestConsoleExePath, arguments);
            processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();

            var MsTestProcess = new Process();
            MsTestProcess.StartInfo = processStartInfo;
            MsTestProcess.OutputDataReceived += (sender, args) =>
            {
                System.Console.WriteLine(args.Data);
            };

            MsTestProcess.Start();
            
            MsTestProcess.BeginErrorReadLine();
            MsTestProcess.BeginOutputReadLine();

            MsTestProcess.WaitForExit();
        }

        /// <summary>
        /// Generate MsTest arguments for repeat test run
        /// </summary>
        /// <param name="repeatTestNames">Names of tests to execute</param>
        /// <param name="resultsTrxPath">Where to save the trx file</param>
        /// <returns>MsTest arguments for performing the repeat test run</returns>
        private string GenerateArgumentsForRepeatTestRun(List<string> repeatTestNames, string newTestResultsFilePath, ConsoleArgumentsManager consoleArgumentsProvider)
        {
            StringBuilder argBuilder = new StringBuilder(consoleArgumentsProvider.BaseConsoleArguments);

            foreach (var testName in repeatTestNames)
            {
                argBuilder.AppendFormat("/test:{0} ", testName);
            }
            
            argBuilder.Replace(consoleArgumentsProvider.TestResultPath, newTestResultsFilePath);
            
            return argBuilder.ToString().TrimEnd();
        }

        #endregion
    }
}