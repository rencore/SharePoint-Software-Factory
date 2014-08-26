using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.Runtime.Serialization;
using EnvDTE;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
    public class SCCFileReference : UnboundRecipeReference
    {
        public SCCFileReference(string recipe) : base(recipe) { }

        protected SCCFileReference(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override bool IsEnabledFor(object target)
        {
            try
            {
                if (target is ProjectItem)
                {
                    ProjectItem item = target as ProjectItem;
                    if (item.Properties != null)
                    {
                        if (item.DTE.SourceControl.IsItemUnderSCC(Helpers.GetFullPathOfProjectItem(item)))
                        {
                            return true;
                        }
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
            get { return "Any file which is under source control"; }
        }
    }
}
