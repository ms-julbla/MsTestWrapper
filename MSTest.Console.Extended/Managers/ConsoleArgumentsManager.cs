using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace MSTest.Console.Extended.Managers
{
    public class ConsoleArgumentsManager
    {
        private const string testResultFilePathRegexPattern = @".*(?<ResultsFileArgument>/(?i)resultsfile(?-i):(?<ResultsFilePath>[1-9A-Za-z\\:._]{1,}))";
        private const string testNewResultFilePathRegexPattern = @".*(?<NewResultsFilePathArgument>/(?i)newResultsfile(?-i):(?<NewResultsFilePath>[1-9A-Za-z\\:._]{1,}))";
        private const string retriesRegexPattern = @".*(?<RetriesArgument>/(?i)retriesCount(?-i):(?<RetriesCount>[0-9]{1})).*";
        private const string failedTestsThresholdRegexPattern = @".*(?<ThresholdArgument>/(?i)threshold(?-i):(?<ThresholdCount>[0-9]{1,3})).*";
        private const string deleteOldFilesRegexPattern = @".*(?<DeleteOldFilesArgument>/(?i)deleteOldResultsFiles(?-i):(?<DeleteOldFilesValue>[a-zA-Z]{4,5})).*";
        private const string argumentRegexPattern = @".*(?<ArgumentName>/[a-zA-Z]{1,}):(?<ArgumentValue>.*)";

        public ConsoleArgumentsManager(string[] arguments)
        {
            this.InitializeConsoleArguments(arguments);

            this.StoreTestResultsPath();

            // Remove & store wrapper-specific arguments
            this.ExtractNewTestResultsPath();
            this.ExtractRetriesCount();
            this.ExtractFailedTestsThreshold();
            this.ExtractDeleteOldResultFiles(); 
        }

        public string InitialConsoleArguments { get; private set; }

        public string BaseConsoleArguments { get; private set; }
        
        public string TestResultPath { get; private set; }

        public string NewTestResultPath { get; private set; }

        public int RetriesCount { get; private set; }

        public int FailedTestsThreshold { get; private set; }

        public bool ShouldDeleteOldTestResultFiles { get; private set; }

        private void StoreTestResultsPath()
        {
            Regex r1 = new Regex(testResultFilePathRegexPattern, RegexOptions.Singleline);
            Match currentMatch = r1.Match(this.InitialConsoleArguments);
            if (!currentMatch.Success)
            {
                throw new ArgumentException("You need to specify path to test results.");
            }
            this.TestResultPath = currentMatch.Groups["ResultsFilePath"].Value;
        }

        private void ExtractNewTestResultsPath()
        {
            Regex r1 = new Regex(testNewResultFilePathRegexPattern, RegexOptions.Singleline);
            Match currentMatch = r1.Match(this.InitialConsoleArguments);
            if (!currentMatch.Success)
            {
                this.NewTestResultPath = this.TestResultPath;
            }
            else
            {
                this.NewTestResultPath = currentMatch.Groups["NewResultsFilePath"].Value;
                this.InitialConsoleArguments = this.InitialConsoleArguments.Replace(currentMatch.Groups["NewResultsFilePathArgument"].Value, string.Empty);
            }
        }

        private void ExtractRetriesCount()
        {
            Regex r1 = new Regex(retriesRegexPattern, RegexOptions.Singleline);
            Match currentMatch = r1.Match(this.InitialConsoleArguments);
            if (!currentMatch.Success)
            {
                this.RetriesCount = 0;
            }
            else
            {
                this.RetriesCount = int.Parse(currentMatch.Groups["RetriesCount"].Value);
                this.InitialConsoleArguments = this.InitialConsoleArguments.Replace(currentMatch.Groups["RetriesArgument"].Value, string.Empty);
            }
        }

        private void ExtractFailedTestsThreshold()
        {
            Regex r1 = new Regex(failedTestsThresholdRegexPattern, RegexOptions.Singleline);
            Match currentMatch = r1.Match(this.InitialConsoleArguments);
            if (!currentMatch.Success)
            {
                this.FailedTestsThreshold = int.Parse(ConfigurationManager.AppSettings["ThresholdDefaultPercentage"]);
            }
            else
            {
                this.FailedTestsThreshold = int.Parse(currentMatch.Groups["ThresholdCount"].Value);
                this.InitialConsoleArguments = this.InitialConsoleArguments.Replace(currentMatch.Groups["ThresholdArgument"].Value, string.Empty);
            }
        }

        private void ExtractDeleteOldResultFiles()
        {
            Regex r1 = new Regex(deleteOldFilesRegexPattern, RegexOptions.Singleline);
            Match currentMatch = r1.Match(this.InitialConsoleArguments);
            if (!currentMatch.Success)
            {
                this.ShouldDeleteOldTestResultFiles = false;
            }
            else
            {
                this.ShouldDeleteOldTestResultFiles = bool.Parse(currentMatch.Groups["DeleteOldFilesValue"].Value);
                this.InitialConsoleArguments = this.InitialConsoleArguments.Replace(currentMatch.Groups["DeleteOldFilesArgument"].Value, string.Empty);
            }
        }

        /// <summary>
        /// Initialize the initial and base console arg strings
        /// </summary>
        /// <param name="arguments">Input argument array</param>
        private void InitializeConsoleArguments(string[] arguments)
        {
            StringBuilder baseArgs = new StringBuilder();
            StringBuilder testSpecifierArgs = new StringBuilder();

            foreach (var currentArgument in arguments)
            {
                string currentValueToBeAppended = currentArgument;

                // If arg has key & value and value has space, surround value with quotes
                KeyValuePair<string, string> currentArgumentPair = this.SplitArgumentNameAndValue(currentArgument);
                if (currentArgumentPair.Key != null && currentArgumentPair.Value.Contains(" "))
                {
                    currentValueToBeAppended = string.Concat("/", currentArgumentPair.Key, ":", "\"", currentArgumentPair.Value, "\"");
                }

                if ("test".Equals(currentArgumentPair.Key, StringComparison.InvariantCultureIgnoreCase))
                {
                    testSpecifierArgs.AppendFormat("{0} ", currentValueToBeAppended);
                }
                else
                {
                    baseArgs.AppendFormat("{0} ", currentValueToBeAppended);
                }
            }

            this.BaseConsoleArguments = baseArgs.ToString().TrimEnd();
            this.InitialConsoleArguments = baseArgs.Append(testSpecifierArgs.ToString()).ToString();
        }

        private KeyValuePair<string, string> SplitArgumentNameAndValue(string argument)
        {
            KeyValuePair<string, string> argumentPair = new KeyValuePair<string, string>();
            Regex regexPattern = new Regex(argumentRegexPattern, RegexOptions.Singleline);
            Match currentMatch = regexPattern.Match(argument);
            if (currentMatch.Success)
            {
                argumentPair = new KeyValuePair<string, string>(currentMatch.Groups["ArgumentName"].Value, currentMatch.Groups["ArgumentValue"].Value);
            }

            return argumentPair;
        }
    }
}