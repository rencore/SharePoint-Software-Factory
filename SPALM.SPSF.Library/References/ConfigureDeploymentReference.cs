using System;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE80;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
    public class ConfigureDeploymentReference : UnboundRecipeReference
    {
        public ConfigureDeploymentReference(string template)
            : base(template)
        {
        }

        public override bool IsEnabledFor(object target)
        {
            try
            {             
                if (target is Project)
                {
                  if(Helpers.IsCustomizationProject((Project)target))
                  {
                    return true;
                  }             
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public override string AppliesTo
        {
            get { return "All Customization projects or solution folders with customization projects"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected ConfigureDeploymentReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
