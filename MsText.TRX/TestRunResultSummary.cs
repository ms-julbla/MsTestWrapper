using System.Xml.Serialization;

namespace MSTest.Console.Extended.TRX
{
    
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunResultSummary
    {
        private TestRunResultSummaryCounters countersField;

        private TestRunResultSummaryOutput outputField;

        private TestRunResultSummaryRunInfos runInfosField;

        private string outcomeField;

        
        public TestRunResultSummaryCounters Counters
        {
            get
            {
                return this.countersField;
            }
            set
            {
                this.countersField = value;
            }
        }

        
        public TestRunResultSummaryOutput Output
        {
            get
            {
                return this.outputField;
            }
            set
            {
                this.outputField = value;
            }
        }

        
        public TestRunResultSummaryRunInfos RunInfos
        {
            get
            {
                return this.runInfosField;
            }
            set
            {
                this.runInfosField = value;
            }
        }

        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string outcome
        {
            get
            {
                return this.outcomeField;
            }
            set
            {
                this.outcomeField = value;
            }
        }
    }
}