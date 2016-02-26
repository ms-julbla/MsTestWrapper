using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSTest.Console.Extended.TRX
{
    
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunUnitTest
    {
        private List<TestRunUnitTestExecution> executionsField;

        private TestRunUnitTestTestMethod testMethodField;

        private TestRunUnitTestOwners ownersField;

        private TestRunUnitTestTestCategory testCategoryField;

        private string nameField;

        private string storageField;

        private string idField;

        
        public TestRunUnitTestOwners Owners
        {
            get
            {
                return this.ownersField;
            }
            set
            {
                this.ownersField = value;
            }
        }

        
        public TestRunUnitTestTestCategory TestCategory
        {
            get
            {
                return this.testCategoryField;
            }
            set
            {
                this.testCategoryField = value;
            }
        }

        
        [XmlElement(ElementName="Execution")]
        public List<TestRunUnitTestExecution> Executions
        {
            get
            {
                return this.executionsField;
            }
            set
            {
                this.executionsField = value;
            }
        }

        
        public TestRunUnitTestTestMethod TestMethod
        {
            get
            {
                return this.testMethodField;
            }
            set
            {
                this.testMethodField = value;
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
        public string storage
        {
            get
            {
                return this.storageField;
            }
            set
            {
                this.storageField = value;
            }
        }

        
        [XmlAttributeAttribute]
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
    }
}