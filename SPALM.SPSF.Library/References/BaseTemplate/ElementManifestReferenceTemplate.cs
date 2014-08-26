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
    public class ElementManifestReferenceTemplate : CustomizationReferenceTemplate, IAttributesConfigurable
    {
        public string FeatureScopes { get; set; }

        public ElementManifestReferenceTemplate(string template)
            : base(template)
        {
        }

        #region ISerializable Members

        protected ElementManifestReferenceTemplate(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            try
            {
                this.FeatureScopes = info.GetString("FeatureScopes");
            }
            catch { }
        }


        #endregion ISerializable Members

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FeatureScopes", this.FeatureScopes);
            base.GetObjectData(info, context);
        }

        public override bool IsEnabledFor(object target)
        {
            Helpers.Log("ElementManifestReferenceTemplate");

            //elments can only be added to a project or to a feature
            if (target is Project)
            {
                if (!base.IsEnabledFor(target))
                {
                    return false;
                }
                return true;
            }
            else if (target is ProjectItem)
            {

                //check if we are in a feature
                try
                {
                    if (FeatureScopes != "")
                    {
                        //recipe is for features, then we check if we are within a feature
                        if (target is ProjectItem)
                        {
                            return Helpers.IsFeatureScope((ProjectItem)target, FeatureScopes);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return false;
        }

        public override string AppliesTo
        {
            get { return "Feature Elements of scope " + this.FeatureScopes; }
        }

        #region IAttributesConfigurable Members

        public new void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            base.Configure(attributes);

            if (attributes.ContainsKey("FeatureScopes"))
            {
                FeatureScopes = attributes["FeatureScopes"];
            }            
        }

        #endregion
    }
}
