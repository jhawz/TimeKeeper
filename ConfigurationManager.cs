using System.IO;
using System.Xml;

namespace TimeKeeper
{
    public static class ConfigurationManager
    {
        public static void CreateConfiguration()
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true
            };
            var xmlWriter = XmlWriter.Create("config.xml", settings);

            // Start
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("config");

            // Credentials section of config.xml
            xmlWriter.WriteStartElement("credentials");
            xmlWriter.WriteElementString("username", "");
            xmlWriter.WriteElementString("password", "");
            xmlWriter.WriteElementString("domain", "");
            xmlWriter.WriteElementString("credentials_saved", "false");
            xmlWriter.WriteEndElement();

            // Sharepoint section of config.xml
            xmlWriter.WriteStartElement("sharepoint");
            xmlWriter.WriteElementString("is_configured", "false");
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

            var xmlReaderSettings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };
            var xmlReader = XmlReader.Create("config.xml", xmlReaderSettings);

            xmlReader.MoveToContent();
            xmlReader.ReadStartElement("config");
            // Load credentials
            xmlReader.ReadStartElement("credentials");
            Configuration.UserName = xmlReader.ReadElementString("username");
            Configuration.Password = xmlReader.ReadElementString("password");
            Configuration.Domain = xmlReader.ReadElementString("domain");
            Configuration.CredentialsSaved = xmlReader.ReadElementString("credentials_saved");
            xmlReader.ReadEndElement();

            // Load sharepoint
            xmlReader.ReadStartElement("sharepoint");
            Configuration.Configured = xmlReader.ReadElementString("is_configured");
            Configuration.SharepointUrl = xmlReader.ReadElementString("sharepoint_url");

            // Close Reader
            xmlReader.ReadEndElement();
            xmlReader.Close();
        }

        public static void SaveCredentials()
        {
            if (!File.Exists("config.xml"))
            {
                CreateConfiguration();
            }
            // XML Document
            var configXmlDocument = new XmlDocument();
            configXmlDocument.Load("config.xml");
            if (configXmlDocument.DocumentElement != null)
            {
                XmlNode credentialsNode = configXmlDocument.DocumentElement["credentials"];
                if (credentialsNode != null)
                    foreach (XmlNode child in credentialsNode.ChildNodes)
                    {
                        switch (child.Name)
                        {
                            case "username":
                                child.InnerText = Configuration.UserName;
                                break;
                            case "password":
                                child.InnerText = Configuration.Password;
                                break;
                            case "domain":
                                child.InnerText = Configuration.Domain;
                                break;
                            case "credentials_saved":
                                child.InnerText = Configuration.CredentialsSaved;
                                break;
                        }
                    }
                // End credentials
                configXmlDocument.Save("config.xml");
            }
        }

        public static void SaveConfiguration()
        {
            if (!File.Exists("config.xml"))
            {
                CreateConfiguration();
            }
            // XML Document
            var configXmlDocument = new XmlDocument();
            configXmlDocument.Load("config.xml");
            if (configXmlDocument.DocumentElement != null)
            {
                XmlNode sharepointNode = configXmlDocument.DocumentElement["sharepoint"];
                if (sharepointNode != null)
                    foreach (XmlNode child in sharepointNode.ChildNodes)
                    {
                        switch (child.Name)
                        {
                            case "sharepoint_url":
                                child.InnerText = Configuration.SharepointUrl;
                                break;
                            case "is_configured":
                                child.InnerText = Configuration.Configured;
                                break;
                        }
                    }
                // End sharepoint
                configXmlDocument.Save("config.xml");
            }
        }
    }
}
