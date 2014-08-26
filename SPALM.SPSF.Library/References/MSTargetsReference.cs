using System;
using EnvDTE;
using EnvDTE80;
using VSLangProj;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework;
using System.Xml;

namespace SPALM.SPSF.Library.References
{

    [Serializable]
    public class MSTargetsReference : UnboundRecipeReference
    {
        public MSTargetsReference( string recipe ) : base( recipe )
        {
        }

        protected MSTargetsReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override bool IsEnabledFor( object target )
        {
            if( target is ProjectItem)
            {
                if (((ProjectItem)target).Name.EndsWith(".msbuild"))
                {
                    return true;
                }
                if (((ProjectItem)target).Name.EndsWith(".targets"))
                {
                    return true;
                }
            }
            return false;
         }
        
        public override string AppliesTo
        {
             get
             {
                 return "*.msbuild or *.targets";
             }
        }
    }
}
