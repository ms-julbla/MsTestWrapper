using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSTest.Console.Extended.TRX
{
    
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010", IsNullable = false)]
    public partial class TestRun
    {
        private TestRunTestSettings testSettingsField;

        private TestRunTimes timesField;

        private TestRunResultSummary resultSummaryField;

        private List<TestRunUnitTest> testDefinitionsField;

        private List<TestRunTestList> testListsField;

        private List<TestRunTestEntry> testEntriesField;

        private List<TestRunUnitTestResult> resultsField;

        private string idField;

        private string nameField;

        private string runUserField;

        
        public TestRunTestSettings TestSettings
        {
            get
            {
                return this.testSettingsField;
            }
            set
            {
                this.testSettingsField = value;
            }
        }

        
        public TestRunTimes Times
        {
            get
            {
                return this.timesField;
            }
            set
            {
                this.timesField = value;
            }
        }

        
        public TestRunResultSummary ResultSummary
        {
            get
            {
                return this.resultSummaryField;
            }
            set
            {
                this.resultSummaryField = value;
            }
        }

        
        [System.Xml.Serialization.XmlArrayItemAttribute("UnitTest", IsNullable = false)]
        public List<TestRunUnitTest> TestDefinitions
        {
            get
            {
                return this.testDefinitionsField;
            }
            set
            {
                this.testDefinitionsField = value;
            }
        }

        
        [System.Xml.Serialization.XmlArrayItemAttribute("TestList", IsNullable = false)]
        public List<TestRunTestList> TestLists
        {
            get
            {
                return this.testListsField;
            }
            set
            {
                this.testListsField = value;
            }
        }

        
        [System.Xml.Serialization.XmlArrayItemAttribute("TestEntry", IsNullable = false)]
        public List<TestRunTestEntry> TestEntries
        {
            get
            {
                return this.testEntriesField;
            }
            set
            {
                this.testEntriesField = value;
            }
        }

        
        [System.Xml.Serialization.XmlArrayItemAttribute("UnitTestResult", IsNullable = false)]
        public List<TestRunUnitTestResult> Results
        {
            get
            {
                return this.resultsField;
            }
            set
            {
                this.resultsField = value;
            }
        }

        
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        
        [XmlAttributeAttribute]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        
        [XmlAttributeAttribute]
        public string runUser
        {
            get
            {
                return this.runUserField;
            }
            set
            {
                this.runUserField = value;
            }
        }
    }
}