using System;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE80;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
  public class SolutionFolderWSPReference : UnboundRecipeReference, IAttributesConfigurable
    {
        public SolutionFolderWSPReference(string template)
            : base(template)
        {
        }

        public override bool IsEnabledFor(object target)
        {
            Helpers.Log("SolutionFolderWSPReference");

            try
            {
                //return Helpers.IsCustomizationProject(project);
                return Helpers.IsTargetInCustomizationProject(target);        
            }
            catch (Exception)
            {              
            }

            return false;
        }


        public override string AppliesTo
        {
            get { return "All WSP Solutions"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected SolutionFolderWSPReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
          if (this.AssetName == "PageLayout")
          {
          }
        }

        #endregion ISerializable Members

        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
          if (this.AssetName == "PageLayout")
          {
          }
        }

        #endregion
    }
}
