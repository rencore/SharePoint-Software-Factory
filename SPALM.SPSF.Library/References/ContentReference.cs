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
    public class ContentReference : UnboundRecipeReference
    {
        public ContentReference(string template)
            : base(template)
        {
        }

        public override bool IsEnabledFor(object target)
        {
            try
            {
              if (target is SolutionFolder || target is Solution)
              {
                return true;
              }
              if (((Project)target).Object is SolutionFolder)
              {
                //any solution folder with name deployment
                if (((SolutionFolder)((Project)target).Object).Parent.Name == "Contents")
                {
                  return true;
                }
              }
              return false;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public override string AppliesTo
        {
          get { return "Content folders"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected ContentReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
