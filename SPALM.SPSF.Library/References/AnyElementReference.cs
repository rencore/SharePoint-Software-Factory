using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.Runtime.Serialization;
using EnvDTE;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
    public class AnyElementReference : UnboundRecipeReference
    {
        public AnyElementReference(string recipe) : base(recipe) { }

        protected AnyElementReference(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override bool IsEnabledFor(object target)
        {
            Helpers.Log("AnyElementReference");

					try
					{
						//not enabled for files
						if (target is ProjectItem)
						{
							if ((target as ProjectItem).Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
							{
								return false;
							}
						}
					}
					catch (Exception)
					{
					}
          return true;
        }

        public override string AppliesTo
        {
            get { return "Any solution element"; }
        }
    }
}
