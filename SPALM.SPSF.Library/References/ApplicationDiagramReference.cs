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
    public class ApplicationDiagramReference : UnboundRecipeReference
    {
        public ApplicationDiagramReference( string recipe ) : base( recipe )
        {
        }

        protected ApplicationDiagramReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override bool IsEnabledFor( object target )
        {
					try
					{
            if( target is ProjectItem)
            {
                if (((ProjectItem)target).Name.EndsWith(".spl"))
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
                 return "*.msbuild";
             }
        }
    }
}
