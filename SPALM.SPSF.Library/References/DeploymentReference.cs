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
    public class DeploymentReference : UnboundRecipeReference
    {
        public DeploymentReference(string template)
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
                if (((SolutionFolder)((Project)target).Object).Parent.Name == "Deployment")
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
            get { return "Deployment folders"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected DeploymentReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
