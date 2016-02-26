using MSTest.Console.Extended.Managers;
using System;
using System.Configuration;

namespace MSTest.Console.Extended
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string microsoftTestConsoleExePath = ConfigurationManager.AppSettings["MSTestConsoleRunnerPath"]; 
            var consoleArgumentsProvider = new ConsoleArgumentsManager(args);
            var engine = new TestExecutionService(
                new TestRunManager(),
                new MsTestProcessManager(microsoftTestConsoleExePath),
                consoleArgumentsProvider);
            try
            {
                int result = engine.ExecuteWithRetry();

                System.Console.Out.WriteLine("Exiting with code " + result);
                Environment.Exit(result);
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(string.Concat(ex.Message, ex.StackTrace));
            }
        }
    }
}