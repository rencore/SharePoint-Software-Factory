using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.Collections;
using System.Xml;
using System.IO;
using System.Resources;

namespace SPALM.SPSF.Library
{
    public class SharePointConfigurationHelper
    {
        static string resourcefolder = "";
        static string sharepointpath = "";
        static string sharepointwebtemppath = "";
        //static string defaultlocal = "en-US";

        static Dictionary<string, CustomResXResourceReader> resourcesDictionary;

        /// <summary>
        /// Creates a new sharepoint configuration file in the current solution
        /// </summary>
        /// <param name="dte"></param>
        internal static void CreateSharePointConfigurationFile(DTE dte)
        {
            resourcefolder = Helpers.GetSharePointHive();
            //defaultlocal = "en-US";

            sharepointpath = Path.Combine(Helpers.GetSharePointHive(), @"TEMPLATE\FEATURES");
            sharepointwebtemppath = Path.Combine(Helpers.GetSharePointHive(), @"TEMPLATE\1033\XML");

            //reloading sharepoint configuration

            //load resouces
            resourcesDictionary = new Dictionary<string, CustomResXResourceReader>();
            Helpers.LoadResources(resourcefolder, resourcesDictionary);

            //open the existing sharepoint configuration file
            string solutionDirectory = Path.GetDirectoryName((string)dte.Solution.Properties.Item("Path").Value);
            string toPath = Path.Combine(solutionDirectory, "SharepointConfiguration.xml");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineHandling = NewLineHandling.Entitize;
            settings.Encoding = Encoding.UTF8;

            MemoryStream memStream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(memStream, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("SharePointConfiguration");

            UpdateContentTypes(writer);

            UpdateSiteColumns(writer);

            UpdateListTemplates(writer);

            UpdateWebTemplates(writer);

            UpdateFeatures(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();

            StreamReader reader = new StreamReader(memStream, Encoding.UTF8, true);
            memStream.Seek(0, SeekOrigin.Begin);

            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            doc.Save(toPath);

        }


        private static void UpdateFeatures(XmlWriter writer)
        {
            writer.WriteStartElement("Features");

            if (Directory.Exists(sharepointpath))
            {
                foreach (string s in Directory.GetFiles(sharepointpath, "Feature.xml", SearchOption.AllDirectories))
                {
                    if (File.Exists(s))
                    {
                        try
                        {
                            XmlDocument featurexml = new XmlDocument();
                            featurexml.Load(s);

                            string featurename = Directory.GetParent(s).Name;

                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(featurexml.NameTable);
                            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");
                            XmlNode featurenode = featurexml.SelectSingleNode("/ns:Feature", nsmgr);
                            if (featurenode != null)
                            {
                                writer.WriteStartElement("Feature");

                                writer.WriteAttributeString("Id", featurenode.Attributes["Id"].Value);
                                writer.WriteAttributeString("Title", Helpers.GetResourceString(GetAttribute(featurenode, "Title"), featurename, resourcesDictionary));
                                writer.WriteAttributeString("Description", Helpers.GetResourceString(GetAttribute(featurenode, "Description"), featurename, resourcesDictionary));
                                writer.WriteAttributeString("Scope", featurenode.Attributes["Scope"].Value);

                                writer.WriteEndElement();
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            writer.WriteEndElement();
        }

        private static void UpdateWebTemplates(XmlWriter writer)
        {
            writer.WriteStartElement("SiteTemplates");

            if (Directory.Exists(sharepointwebtemppath))
            {
                foreach (string s in Directory.GetFiles(sharepointwebtemppath, "webtemp*.xml", SearchOption.TopDirectoryOnly))
                {
                    if (File.Exists(s))
                    {
                        try
                        {
                            XmlDocument webtempxml = new XmlDocument();
                            webtempxml.Load(s);

                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(webtempxml.NameTable);
                            nsmgr.AddNamespace("ows", "Microsoft SharePoint");

                            foreach (XmlNode webnode in webtempxml.SelectNodes("/Templates/Template"))
                            {
                                //jetzt alle configuration nodes
                                foreach (XmlNode confignode in webnode.SelectNodes("Configuration"))
                                {
                                    writer.WriteStartElement("SiteTemplate");

                                    writer.WriteAttributeString("Id", GetAttribute(webnode, "Name") + "#" + GetAttribute(confignode, "ID"));
                                    writer.WriteAttributeString("Title", GetAttribute(confignode, "Title"));
                                    writer.WriteAttributeString("Description", GetAttribute(confignode, "Description"));
                                    writer.WriteAttributeString("DisplayCategory", GetAttribute(confignode, "DisplayCategory"));

                                    writer.WriteEndElement();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            writer.WriteEndElement();
        }

        private static string GetAttribute(XmlNode node, string attribute)
        {
            if (node.Attributes[attribute] != null)
            {
                return node.Attributes[attribute].Value;
            }
            return "";
        }

        private static void UpdateListTemplates(XmlWriter writer)
        {
            writer.WriteStartElement("ListTemplates");

            if (Directory.Exists(sharepointpath))
            {
                foreach (string s in Directory.GetFiles(sharepointpath, "*.xml", SearchOption.AllDirectories))
                {
                    if (File.Exists(s))
                    {
                        try
                        {
                            XmlDocument featurexml = new XmlDocument();
                            featurexml.Load(s);

                            string featurename = Directory.GetParent(s).Name;

                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(featurexml.NameTable);
                            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");
                            foreach (XmlNode webnode in featurexml.SelectNodes("/ns:Elements/ns:ListTemplate", nsmgr))
                            {
                                writer.WriteStartElement("ListTemplate");

                                writer.WriteAttributeString("Type", GetAttribute(webnode, "Type"));
                                writer.WriteAttributeString("DisplayName", Helpers.GetResourceString(GetAttribute(webnode, "DisplayName"), featurename, resourcesDictionary));
                                writer.WriteAttributeString("Description", Helpers.GetResourceString(GetAttribute(webnode, "Description"), featurename, resourcesDictionary));

                                //get the parent feature id
                                string featureId = GetFeatureId(Directory.GetParent(s));
                                writer.WriteAttributeString("FeatureId", featureId);


                                writer.WriteEndElement();
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            writer.WriteEndElement();
        }

        private static string GetFeatureId(DirectoryInfo currentDir)
        {
            string featureId = "";

            //is the parent directory "FEATURES"?
            if (currentDir.Parent.Name.Equals("features", StringComparison.InvariantCultureIgnoreCase))
            {
                //currentDir is a feature directory
                string featureXml = Path.Combine(currentDir.FullName, "feature.xml");
                if (File.Exists(featureXml))
                {
                    featureId = GetFeatureFromFeatureXml(featureXml);
                }
            }
            else if (currentDir.Parent != null)
            {
                featureId = GetFeatureId(currentDir.Parent);
            }
            return featureId;
        }

        private static string GetFeatureFromFeatureXml(string featureXml)
        {
            try
            {
                XmlDocument featurexml = new XmlDocument();
                featurexml.Load(featureXml);


                XmlNamespaceManager nsmgr = new XmlNamespaceManager(featurexml.NameTable);
                nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");
                XmlNode featurenode = featurexml.SelectSingleNode("/ns:Feature", nsmgr);
                if (featurenode != null)
                {
                    return featurenode.Attributes["Id"].Value;
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        private static void UpdateSiteColumns(XmlWriter writer)
        {
            writer.WriteStartElement("Fields");

            if (Directory.Exists(sharepointpath))
            {
                foreach (string s in Directory.GetFiles(sharepointpath, "*.xml", SearchOption.AllDirectories))
                {
                    if (File.Exists(s))
                    {
                        try
                        {
                            XmlDocument featurexml = new XmlDocument();
                            featurexml.Load(s);

                            string featurename = Directory.GetParent(s).Name;

                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(featurexml.NameTable);
                            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");
                            foreach (XmlNode webnode in featurexml.SelectNodes("/ns:Elements/ns:Field", nsmgr))
                            {
                                writer.WriteStartElement("Field");

                                writer.WriteAttributeString("ID", GetAttribute(webnode, "ID"));
                                writer.WriteAttributeString("DisplayName", Helpers.GetResourceString(GetAttribute(webnode, "DisplayName"), featurename, resourcesDictionary));
                                writer.WriteAttributeString("Description", Helpers.GetResourceString(GetAttribute(webnode, "Description"), featurename, resourcesDictionary));
                                writer.WriteAttributeString("Group", Helpers.GetResourceString(GetAttribute(webnode, "Group"), featurename, resourcesDictionary));

                                writer.WriteEndElement();
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            writer.WriteEndElement();
        }

        private static void UpdateContentTypes(XmlWriter writer)
        {
            writer.WriteStartElement("ContentTypes");

            if (Directory.Exists(sharepointpath))
            {
                foreach (string s in Directory.GetFiles(sharepointpath, "*.xml", SearchOption.AllDirectories))
                {
                    if (File.Exists(s))
                    {
                        try
                        {
                            XmlDocument featurexml = new XmlDocument();
                            featurexml.Load(s);

                            string featurename = Directory.GetParent(s).Name;

                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(featurexml.NameTable);
                            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");
                            foreach (XmlNode webnode in featurexml.SelectNodes("/ns:Elements/ns:ContentType", nsmgr))
                            {
                                writer.WriteStartElement("ContentType");

                                writer.WriteAttributeString("ID", GetAttribute(webnode, "ID"));
                                writer.WriteAttributeString("Name", Helpers.GetResourceString(GetAttribute(webnode, "Name"), featurename, resourcesDictionary));
                                writer.WriteAttributeString("Description", Helpers.GetResourceString(GetAttribute(webnode, "Description"), featurename, resourcesDictionary));
                                writer.WriteAttributeString("Group", Helpers.GetResourceString(GetAttribute(webnode, "Group"), featurename, resourcesDictionary));

                                writer.WriteEndElement();
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            writer.WriteEndElement();
        }


    }
}
