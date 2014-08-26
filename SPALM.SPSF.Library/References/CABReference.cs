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
    public class CABReference : UnboundRecipeReference
    {
        public CABReference( string recipe ) : base( recipe )
        {
        }

        protected CABReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override bool IsEnabledFor( object target )
        {
					try
					{
            if( target is ProjectItem)
            {
                if (((ProjectItem)target).Name.EndsWith(".cab"))
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
                 return "*.cab";
             }
        }
    }
}
