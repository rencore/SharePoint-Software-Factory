using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
    public class SharePointVersionProvider : ValueProvider
    {
        protected string GetBasePath()
        {
            return this.GetService<IConfigurationService>(true).BasePath;
        }

        private string GetTemplateBasePath()
        {
            return new DirectoryInfo(this.GetBasePath() + @"\Templates").FullName;
        }

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue != null)
            {
                // Do not assign a new value, and return false to flag that 
                // we don't want the current value to be changed.
                newValue = null;
                return false;
            }

            DTE service = (DTE)this.GetService(typeof(DTE));

            Helpers.LogMessage(service, this, "Retrieving SharePoint version");
            Version farmVersion = new SharePointBrigdeHelper(service).GetSharePointVersion();

            string versionsXmlDocPath = Path.Combine(GetBasePath(), "SharePointVersions.xml");
            if (File.Exists(versionsXmlDocPath))
            {
                XmlDocument versionsDoc = new XmlDocument();
                versionsDoc.Load(versionsXmlDocPath);

                foreach (XmlNode versionNode in versionsDoc.SelectNodes("SPSD/SharePoint/Versions/Version"))
                {
                    try
                    {
                        Version checkVersion = new Version(versionNode.Attributes["Number"].Value);

                        //check if farmversion is larger than the checkversion
                        if (farmVersion.Equals(checkVersion))
                        {
                            newValue = versionNode.Attributes["Name"].Value + " (" + farmVersion.ToString() + ")";
                            Helpers.LogMessage(service, service, string.Format("Installed SharePoint Version: {0}", newValue));
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            switch (farmVersion.Major)
            {
                case 12:
                    newValue = "SharePoint 2007 (" + farmVersion.ToString() + "), unkown patch level";
                    break;
                case 14:
                    newValue = "SharePoint 2010 (" + farmVersion.ToString() + "), unkown patch level";
                    break;
                case 15:
                    newValue = "SharePoint 2013 (" + farmVersion.ToString() + "), unkown patch level";
                    break;

                default:
                    newValue = "SharePoint ???? (" + farmVersion.ToString() + "), unkown SharePoint release";

                    break;
            }

            Helpers.LogMessage(service, service, string.Format("Installed SharePoint Version: {0}", newValue));
            return true;
        }
    }
}

