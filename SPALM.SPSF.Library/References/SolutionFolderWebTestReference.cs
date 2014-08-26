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
    public class SolutionFolderWebTestReference : UnboundRecipeReference
    {
        public SolutionFolderWebTestReference(string template)
            : base(template)
        {
        }

        public override bool IsEnabledFor(object target)
        {

            return target is Project && ((Project)target).FullName.Contains(".WebTests.");
        }

        public override string AppliesTo
        {
            get { return "All WebTest Projects (project name contains '.WebTests.')"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected SolutionFolderWebTestReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
