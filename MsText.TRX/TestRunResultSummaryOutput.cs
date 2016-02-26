using System;
using System.Linq;

namespace MSTest.Console.Extended.TRX
{
    
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunResultSummaryOutput
    {
        private string stdOutField;

        
        public string StdOut
        {
            get
            {
                return this.stdOutField;
            }
            set
            {
                this.stdOutField = value;
            }
        }
    }
}