using System.Xml.Serialization;

namespace MSTest.Console.Extended.TRX
{
    
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunUnitTestTestMethod
    {
        private string codeBaseField;

        private string adapterTypeNameField;

        private string classNameField;

        private string nameField;

        
        [XmlAttributeAttribute]
        public string codeBase
        {
            get
            {
                return this.codeBaseField;
            }
            set
            {
                this.codeBaseField = value;
            }
        }

        
        [XmlAttributeAttribute]
        public string adapterTypeName
        {
            get
            {
                return this.adapterTypeNameField;
            }
            set
            {
                this.adapterTypeNameField = value;
            }
        }

        
        [XmlAttributeAttribute]
        public string className
        {
            get
            {
                return this.classNameField;
            }
            set
            {
                this.classNameField = value;
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
    }
}