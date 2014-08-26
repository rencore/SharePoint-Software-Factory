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
    public class SharePointConfigReference : UnboundRecipeReference
    {
        public SharePointConfigReference( string recipe ) : base( recipe )
        {
        }

        protected SharePointConfigReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override bool IsEnabledFor( object target )
        {
					try
					{
                        if( target is ProjectItem)
                        {
                            if (((ProjectItem)target).Name.Equals("SharepointConfiguration.xml", StringComparison.InvariantCultureIgnoreCase))
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
               return "SharepointConfiguration.xml";
             }
        }
    }
}
