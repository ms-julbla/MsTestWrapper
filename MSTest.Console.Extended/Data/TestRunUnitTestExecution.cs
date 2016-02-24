using System.Xml.Serialization;

namespace MSTest.Console.Extended.Data
{
    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunUnitTestExecution
    {
        private string idField;
        private byte timeoutField;

        /// <remarks/>
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

        [XmlAttributeAttribute]
        public byte timeout
        {
            get
            {
                return this.timeoutField;
            }
            set
            {
                this.timeoutField = value;
            }
        }
    }
}