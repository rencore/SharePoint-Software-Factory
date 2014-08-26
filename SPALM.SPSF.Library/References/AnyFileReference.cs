using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.Runtime.Serialization;
using EnvDTE;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
    public class AnyFileReference : UnboundRecipeReference
    {
        public AnyFileReference(string recipe) : base(recipe) { }

        protected AnyFileReference(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override bool IsEnabledFor(object target)
        {
            return true;
        }

        public override string AppliesTo
        {
            get { return "Any solution element"; }
        }
    }
}
