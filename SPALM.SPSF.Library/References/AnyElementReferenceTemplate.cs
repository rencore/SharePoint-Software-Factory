using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
  public class AnyElementReferenceTemplate : UnboundTemplateReference
    {
        public AnyElementReferenceTemplate(string recipe) : base(recipe) { }

        protected AnyElementReferenceTemplate(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override bool IsEnabledFor(object target)
        {
            Helpers.Log("AnyElementReferenceTemplate");

					try
					{
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
