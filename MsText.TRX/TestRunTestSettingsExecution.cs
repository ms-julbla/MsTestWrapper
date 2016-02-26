using System.Xml.Serialization;

namespace MSTest.Console.Extended.TRX
{
    
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunTestSettingsExecution
    {
        private object testTypeSpecificField;

        private TestRunTestSettingsExecutionAgentRule agentRuleField;

        
        public object TestTypeSpecific
        {
            get
            {
                return this.testTypeSpecificField;
            }
            set
            {
                this.testTypeSpecificField = value;
            }
        }

        
        public TestRunTestSettingsExecutionAgentRule AgentRule
        {
            get
            {
                return this.agentRuleField;
            }
            set
            {
                this.agentRuleField = value;
            }
        }
    }
}