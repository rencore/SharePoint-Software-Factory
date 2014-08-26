using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.Runtime.Serialization;
using EnvDTE;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
    public class TTFileReference : UnboundRecipeReference
    {
        public TTFileReference(string recipe) : base(recipe) { }

        protected TTFileReference(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override bool IsEnabledFor(object target)
        {
            try
            {
                if (target is ProjectItem)
                {
                    if (((ProjectItem)target).Name.EndsWith(".TT", StringComparison.InvariantCultureIgnoreCase))
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
            get { return "TextTemplate files (*.tt)"; }
        }
    }
}
