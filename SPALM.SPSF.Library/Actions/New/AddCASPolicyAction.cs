#region Using Directives

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using Microsoft.Practices.Common.Services;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Xml;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    // <summary>
    // Takes a xml file in the following format and adds it to the project
    // <PermissionSet class="NamedPermissionSet" Name="PermissionSet_<#= AppName #>_<#= System.Guid.NewGuid().ToString() #>" Description="Minimal Permission Set" version="1">
    //  <IPermission class="SharePointPermission" version="1" ObjectModel="True" />
    //  <IPermission class="WebPartPermission" version="1" Connections="True" />
    //  <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SkipVerification" />
    //  <IPermission class="SmtpPermission" version="1" Access="Connect" />
    //  <IPermission class="AspNetHostingPermission" version="1" Level="Minimal" />
    //  <IPermission class="SecurityPermission" version="1" Flags="Execution" />
    // </PermissionSet>
    /// finally we need:
    /// 
    /// <Solution>
    /// <CodeAccessSecurity>
    //  <PolicyItem>  
    //    <PermissionSet class="NamedPermissionSet" version="1">
    //      <IPermission class="SecurityPermission" version="1" Flags="Execution" />
    //      <IPermission class="AspNetHostingPermission" version="1" Level="Minimal" />
    //      <IPermission class="Microsoft.SharePoint.Security.SharePointPermission, Microsoft.SharePoint.Security, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" version="1" ObjectModel="True" />
    //    </PermissionSet>
    //    <Assemblies>
    //      <Assembly Name="$SharePoint.Project.AssemblyName$" Version="$SharePoint.Project.AssemblyVersion$" PublicKeyBlob="$SharePoint.Project.AssemblyPublicKeyBlob$"/>
    //    </Assemblies>
    //  </PolicyItem>
    //</CodeAccessSecurity></Solution>

    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddCASPolicyAction : BaseTemplateAction
    {
        public AddCASPolicyAction()
            : base()
        {

        }

        public override void Execute()
        {
            if (ExcludeCondition)
            {
                return;
            }
            if (!AdditionalCondition)
            {
                return;
            }

            DTE dte = (DTE)this.GetService(typeof(DTE));
            Project project = this.GetTargetProject(dte);

            string evaluatedTemplateFileName = EvaluateParameterAsString(dte, TemplateFileName);
            string templateContent = GenerateContent(evaluatedTemplateFileName, "CASPolicy.txt");
            if (Helpers2.TemplateContentIsEmpty(templateContent))
            {
                //do nothing if no content has been generated
                return;
            }

            if (Helpers2.IsSharePointVSTemplate(dte, project))
            {
                Helpers.LogMessage(dte, this, "Adding CAS Policy to file 'Package/Package.Template.xml");

                //merge the contents with the cas policy in the package
                //find the package/package.Template.xml
                try
                {
                    ProjectItem packageItem = Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(project.ProjectItems, "Package").ProjectItems, "Package.Template.xml");
                    if (packageItem == null)
                    {
                        packageItem = Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(project.ProjectItems, "Package").ProjectItems, "Package.package").ProjectItems, "Package.Template.xml");
                    }

                    if (packageItem != null)
                    {
                        Helpers.EnsureCheckout(dte, packageItem);

                        string packageFilepath = Helpers.GetFullPathOfProjectItem(packageItem);
                        XmlDocument packageXml = new XmlDocument();
                        packageXml.Load(packageFilepath);

                        XmlNodeList permissionSetNodes = null;

                        //1. if there is no node Solution/CodeAccessSecurity then we add a new one automatically
                        XmlNamespaceManager featurensmgr = new XmlNamespaceManager(packageXml.NameTable);
                        featurensmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

                        XmlNodeList allCodeAccessSecurityNodes = packageXml.SelectNodes("ns:Solution/ns:CodeAccessSecurity", featurensmgr);
                        if (allCodeAccessSecurityNodes.Count == 0)
                        {
                            //add default cas policy element
                            string templateFilepath = Path.Combine(GetTemplateBasePath(), "Text\\CodeAccessSecurity.xml");
                            XmlDocument resdoc = new XmlDocument();
                            resdoc.Load(templateFilepath);

                            //select the solution node
                            XmlNode solutionNode = packageXml.SelectSingleNode("ns:Solution", featurensmgr);

                            XmlDocumentFragment docFrag = packageXml.CreateDocumentFragment();
                            docFrag.InnerXml = "<dummy xmlns='http://schemas.microsoft.com/sharepoint/'>" + resdoc.OuterXml + "</dummy>";
                            solutionNode.AppendChild(docFrag.FirstChild.FirstChild);

                            //CleanNamespace(solutionNode);

                            //select the nodes again
                            allCodeAccessSecurityNodes = packageXml.SelectNodes("ns:Solution/ns:CodeAccessSecurity", featurensmgr);
                        }

                        permissionSetNodes = packageXml.SelectNodes("ns:Solution/ns:CodeAccessSecurity/ns:PolicyItem/ns:PermissionSet", featurensmgr);

                        //now append the IPolicies to all existing PermissionSets
                        XmlDocument generatedCASPolicy = new XmlDocument();
                        generatedCASPolicy.LoadXml(templateContent);

                        XmlNodeList generatedIPermissions = generatedCASPolicy.SelectNodes("PermissionSet/IPermission");

                        //remove duplicate class
                        foreach (XmlNode permissionSetNode in permissionSetNodes)
                        {
                            foreach (XmlNode generatedIPermission in generatedIPermissions)
                            {
                                //compare new IPermission with existing permission
                                RemoveDuplicateClass(permissionSetNode, generatedIPermission);                                
                            }
                            //CleanNamespace(permissionSetNode);
                        }

                        foreach (XmlNode permissionSetNode in permissionSetNodes)
                        {
                            foreach (XmlNode generatedIPermission in generatedIPermissions)
                            {
                                XmlDocumentFragment docFrag = packageXml.CreateDocumentFragment();
                                docFrag.InnerXml = "<dummy xmlns='http://schemas.microsoft.com/sharepoint/'>" + generatedIPermission.OuterXml + "</dummy>";
                                permissionSetNode.AppendChild(docFrag.FirstChild.FirstChild);
                            }
                        }

                        XmlWriter xw2 = XmlWriter.Create(packageFilepath, Helpers.GetXmlWriterSettingsAttributesInOneLine());
                        packageXml.Save(xw2);
                        xw2.Flush();
                        xw2.Close();

                        Window window = packageItem.Open("{00000000-0000-0000-0000-000000000000}");
                        window.Visible = true;
                        window.Activate();
                    }
                }
                catch
                {
                    Helpers.LogMessage(dte, this, "Could not add CAS policy to package");
                }
            }
            else
            {
                //add the file directly into the root
                this.TargetFileName = "CASPolicy.txt";
                base.Execute();
            }
        }

        private bool RemoveDuplicateClass(XmlNode permissionSetNode, XmlNode generatedIPermission)
        {
            //search for differences
            foreach (XmlNode existingIPermissionNode in permissionSetNode)
            {
                if (existingIPermissionNode.Name == generatedIPermission.Name)
                {
                    if (existingIPermissionNode.Attributes["class"].Value == generatedIPermission.Attributes["class"].Value)
                    {
                        permissionSetNode.RemoveChild(existingIPermissionNode);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool PermissionSetHasAlreadyThisIPermission(XmlNode permissionSetNode, XmlNode generatedIPermission)
        {
            //search for differences
            foreach (XmlNode existingIPermissionNode in permissionSetNode)
            {
                if (existingIPermissionNode.Name == generatedIPermission.Name)
                {
                    //element with same name found
                    //are the attributes the same?
                    //if (NodesHaveSameAttributes(existingIPermissionNode, generatedIPermission))
                    //{
                    //  return true;
                    //}
                    if (existingIPermissionNode.Attributes["class"].Value == generatedIPermission.Attributes["class"].Value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool NodesHaveSameAttributes(XmlNode existingIPermissionNode, XmlNode generatedIPermission)
        {
            //search for differences
            foreach (XmlAttribute newAttrib in generatedIPermission.Attributes)
            {
                //is the attribute already
                if (existingIPermissionNode.Attributes[newAttrib.Name] == null)
                {
                    return false;
                }
                else if (existingIPermissionNode.Attributes[newAttrib.Name].Value != newAttrib.Value)
                {
                    //same attribefo
                    return false;
                }
            }
            return true;
        }

        private void CleanNamespace(XmlNode solutionNode)
        {
            foreach (XmlAttribute attrib in solutionNode.Attributes)
            {
                if (attrib.Name == "xmlns")
                {
                    if (attrib.Value == "")
                    {
                        if (attrib.Value == solutionNode.NamespaceURI)
                        {
                            solutionNode.Attributes.Remove(attrib);
                        }
                    }
                }
            }
            foreach (XmlNode childNode in solutionNode.ChildNodes)
            {
                CleanNamespace(childNode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Undo()
        {
            //TODO: Delete the created projectitems
        }
    }
}