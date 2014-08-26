using System;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE80;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;
using System.Security.Permissions;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
    public class SharePointVersionDependendReferenceTemplate : UnboundTemplateReference, IAttributesConfigurable
    {
        private string sharePointVersions;
        public string SharePointVersions
        {
            get
            {
                return sharePointVersions;
            }
        }

        private bool notSandboxSupported = false;
        public bool NotSandboxSupported
        {
            get
            {
                return notSandboxSupported;
            }
        }

        public SharePointVersionDependendReferenceTemplate(string template)
            : base(template)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SharePointVersions", this.sharePointVersions);
            info.AddValue("NotSandboxSupported", this.notSandboxSupported);
            base.GetObjectData(info, context);
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected SharePointVersionDependendReferenceTemplate(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            try
            {
                this.sharePointVersions = info.GetString("SharePointVersions");
            }
            catch { }

            try
            {
                this.notSandboxSupported = info.GetBoolean("NotSandboxSupported");
            }
            catch { }
        }


        #endregion ISerializable Members

        public override bool IsEnabledFor(object target)
        {
            try
            {


                if (string.IsNullOrEmpty(sharePointVersions))
                {
                    if (notSandboxSupported == false)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {  }
            try
            {
                //if this current asset supports no sandbox disable the recipe
                if (notSandboxSupported == true)
                {
                    Project checkProject = Helpers.GetParentProject(target);
                    if (Helpers.GetIsSandboxedSolution(checkProject))
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            { }

 
            try
            {
                if (!Helpers.SolutionHasVersion(Helpers.GetDTEFromTarget(target), SharePointVersions))
                {
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return true;
        }

        public override string AppliesTo
        {
            get { return "Solutions for SharePoint version " + sharePointVersions; }
        }

        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes.ContainsKey("SharePointVersions"))
            {
                sharePointVersions = attributes["SharePointVersions"];
            }
            if (attributes.ContainsKey("NotSandboxSupported"))
            {
                try
                {
                    notSandboxSupported = Boolean.Parse(attributes["NotSandboxSupported"]);
                }
                catch { }
            }
        }

        #endregion
    }
}
