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
    public class WebFileReference : UnboundRecipeReference
    {
        public WebFileReference( string recipe ) : base( recipe )
        {
        }

				protected WebFileReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override bool IsEnabledFor( object target )
        {
					try
					{
						if (target is ProjectItem)
						{
							if (((ProjectItem)target).Name.EndsWith(".html"))
							{
								return true;
							}
							if (((ProjectItem)target).Name.EndsWith(".htm"))
							{
								return true;
							}
							if (((ProjectItem)target).Name.EndsWith(".aspx"))
							{
								return true;
							}
							if (((ProjectItem)target).Name.EndsWith(".asmx"))
							{
								return true;
							}
							if (((ProjectItem)target).Name.EndsWith(".asp"))
							{
								return true;
							}
							if (((ProjectItem)target).Name.EndsWith(".jpg"))
							{
								return true;
							}
							if (((ProjectItem)target).Name.EndsWith(".gif"))
							{
								return true;
							}
							if (((ProjectItem)target).Name.EndsWith(".png"))
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
                 return "*.bat";
             }
        }
    }
}
