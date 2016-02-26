using System;
using System.Linq;

namespace MSTest.Console.Extended.TRX
{
    
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunUnitTestResultOutput
    {
        private string stdOutField;

        private string debugTraceField;

        private TestRunUnitTestResultOutputErrorInfo errorInfoField;

        
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

        
        public string DebugTrace
        {
            get
            {
                return this.debugTraceField;
            }
            set
            {
                this.debugTraceField = value;
            }
        }

        
        public TestRunUnitTestResultOutputErrorInfo ErrorInfo
        {
            get
            {
                return this.errorInfoField;
            }
            set
            {
                this.errorInfoField = value;
            }
        }
    }
}