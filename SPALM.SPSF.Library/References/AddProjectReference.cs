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
    public class AddProjectReference : SharePointVersionDependendReference
    {
        public AddProjectReference(string template)
            : base(template)
        {
        }

        public override bool IsEnabledFor(object target)
        {
            Helpers.Log("AddProjectReference");

            //gilt nicht für projectItems
            if (target is ProjectItem)
            {
                return false;
            }

            try
            {
                if (target is SolutionFolder || target is Solution)
                {
                    if(base.IsEnabledFor(target))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (target is Project)
                {
                    if ((target as Project).Object is SolutionFolder)
                    {
                        if (base.IsEnabledFor(target))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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
          get { return "Solution or solution folders"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
				protected AddProjectReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
