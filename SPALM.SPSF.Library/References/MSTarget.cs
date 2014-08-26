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
    public class MSTarget : UnboundRecipeReference
    {
        public MSTarget( string recipe ) : base( recipe )
        {
        }

        protected MSTarget(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override bool IsEnabledFor( object target )
        {
					try
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
					}
					catch (Exception)
					{
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
