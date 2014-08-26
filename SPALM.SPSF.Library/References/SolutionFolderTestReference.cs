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
    public class SolutionFolderTestReference : UnboundRecipeReference
    {
        public SolutionFolderTestReference(string template)
            : base(template)
        {
        }

        public override bool IsEnabledFor(object target)
        {

            return target is Project && ((Project)target).FullName.Contains(".Test.");
        }

        public override string AppliesTo
        {
            get { return "All Test Projects (project name contains '.Test.')"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected SolutionFolderTestReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
