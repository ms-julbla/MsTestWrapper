using System.Xml.Serialization;

namespace MSTest.Console.Extended.TRX
{
    
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public partial class TestRunTestSettingsDeployment
    {
        private string userDeploymentRootField;

        private bool useDefaultDeploymentRootField;

        private string runDeploymentRootField;

        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string userDeploymentRoot
        {
            get
            {
                return this.userDeploymentRootField;
            }
            set
            {
                this.userDeploymentRootField = value;
            }
        }

        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool useDefaultDeploymentRoot
        {
            get
            {
                return this.useDefaultDeploymentRootField;
            }
            set
            {
                this.useDefaultDeploymentRootField = value;
            }
        }

        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string runDeploymentRoot
        {
            get
            {
                return this.runDeploymentRootField;
            }
            set
            {
                this.runDeploymentRootField = value;
            }
        }
    }
}