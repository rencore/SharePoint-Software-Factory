using System;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE80;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;
using System.Security.Permissions;
using Microsoft.Practices.ComponentModel;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
    [ServiceDependency(typeof(DTE))]
    public class CustomizationReference : SharePointVersionDependendReference, IAttributesConfigurable
    {       
        public CustomizationReference(string template)
            : base(template)
        {
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected CustomizationReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        #endregion ISerializable Members

        public override bool IsEnabledFor(object target)
        {
            Helpers.Log("CustomizationReference");

            if (!(target is Project))
            {
                return false;
            }
            if (!Helpers.IsTargetInCustomizationProject(target))
            {
                return false;
            }
            if (!base.IsEnabledFor(target))
            {
                return false;
            }
            return true;
        }

        public override string AppliesTo
        {
            get { return "Solution projects for SharePoint " + (!string.IsNullOrEmpty(this.SharePointVersions)?"("+this.SharePointVersions+")":""); }
        }

        #region IAttributesConfigurable Members

        //public void Configure(System.Collections.Specialized.StringDictionary attributes)
        //{
        //    base.Configure(attributes);
        //}

        #endregion
    }
}
