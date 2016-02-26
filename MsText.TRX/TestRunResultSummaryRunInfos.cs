using System;
using System.Linq;

namespace MSTest.Console.Extended.TRX
{
    
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunResultSummaryRunInfos
    {
        private TestRunResultSummaryRunInfosRunInfo runInfoField;

        
        public TestRunResultSummaryRunInfosRunInfo RunInfo
        {
            get
            {
                return this.runInfoField;
            }
            set
            {
                this.runInfoField = value;
            }
        }
    }
}