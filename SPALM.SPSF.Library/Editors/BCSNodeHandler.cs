using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using EnvDTE;

namespace SPALM.SPSF.Library
{
    /// <summary>
    /// used to read the sharePoint configuration xml. For web templates a special
    /// processing is needed.
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    public class BCSNodeHandler : XmlNodeHandler
    {
        public BCSNodeHandler()
            : base("", "", "", "")
        {
        }

        /*
         * 
         * <Model Name="BCSModel1" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://schemas.microsoft.com/windows/2007/BusinessDataCatalog">
        <AccessControlList>
            <AccessControlEntry Principal="CONTOSO\Administrator">
                <Right BdcRight="Edit" />
                <Right BdcRight="Execute" />
                <Right BdcRight="SetPermissions" />
                <Right BdcRight="SelectableInClients" />
            </AccessControlEntry>
        </AccessControlList>
        <LobSystems>
            <LobSystem Name="BCSModel1" DefaultDisplayName="BCSModel1" Type="DotNetAssembly">
                <LobSystemInstances>
                    <LobSystemInstance Name="BCSModel1" DefaultDisplayName="BCSModel1" />
                </LobSystemInstances>
                <Entities>
                    <Entity Name="Department" Namespace="EventReceiverProject1.BCSModel1" Version="1.0.0.0" EstimatedInstanceCount="10000" DefaultDisplayName="Department">
                        <Properties>
         * 
         * */

        private XmlNode GetFirstChildOfType(XmlNode node, string nodeName)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == nodeName)
                {
                    return childNode;
                }
            }
            return null;
        }

        private List<XmlNode> GetChildsOfType(XmlNode node, string nodeName)
        {
            List<XmlNode> result = new List<XmlNode>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == nodeName)
                {
                    result.Add(childNode);
                }
            }
            return result;
        }

        public override NameValueItem[] GetNameValueItems(XmlNode node, XmlDocument resdoc, ProjectItem item)
        {
            try
            {
                List<NameValueItem> nvitems = new List<NameValueItem>();
                foreach (XmlNode lobInstance in GetChildsOfType(GetFirstChildOfType(node.ParentNode.ParentNode, "LobSystemInstances"), "LobSystemInstance"))
                {
                    //we have the Entity in our hand
                    string entityName = node.Attributes["Name"].Value;
                    string entityValue = node.Attributes["Name"].Value;
                    string entityGroup = lobInstance.Attributes["Name"].Value; //LobSystemInstance
                    string entityDescription = node.Attributes["Namespace"].Value; //LobSystemInstance

                    NameValueItem nvitem = new NameValueItem();
                    nvitem.ItemType = "BCSEntity";
                    nvitem.Name = entityName;
                    nvitem.Value = entityValue;
                    nvitem.Group = entityGroup;
                    nvitem.Description = entityDescription;

                    nvitems.Add(nvitem);
                }               

                return nvitems.ToArray();
            }
            catch
            {
                
            }
            return new NameValueItem[0];
        }
    }
}
