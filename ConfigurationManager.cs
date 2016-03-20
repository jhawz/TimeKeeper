using System.Xml;
using System.IO;

namespace TimeKeeper
{
    public static class ConfigurationManager
    {
        public static void CreateConfiguration()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true
            };
            XmlWriter xmlWriter = XmlWriter.Create("config.xml", settings);

            // Start
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("config");

            // Credentials section of config.xml
            xmlWriter.WriteStartElement("credentials");
            xmlWriter.WriteElementString("username", "");
            xmlWriter.WriteElementString("password", "");
            xmlWriter.WriteElementString("domain", "");
            xmlWriter.WriteEndElement();

            // Sharepoint section of config.xml
            xmlWriter.WriteStartElement("sharepoint");
            xmlWriter.WriteElementString("sharepoint_url", "");
            xmlWriter.WriteEndElement();

            // End
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        public static void LoadConfiguration()
        {
            if (!File.Exists("config.xml"))
            {
                CreateConfiguration();
            }

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };

            XmlReader xmlReader = XmlReader.Create("config.xml",xmlReaderSettings);

            xmlReader.MoveToContent();
            xmlReader.ReadStartElement("config");
            // Load credentials
            xmlReader.ReadStartElement("credentials");
            Configuration.UserName = xmlReader.ReadElementString("username");
            Configuration.Password = xmlReader.ReadElementString("password");
            Configuration.Domain = xmlReader.ReadElementString("domain");
            xmlReader.ReadEndElement();

            // Load sharepoint
            xmlReader.ReadStartElement("sharepoint");
            Configuration.SharepointUrl = xmlReader.ReadElementString("sharepoint_url");
            
            xmlReader.ReadEndElement();
            xmlReader.Close();
        }
    }
}
