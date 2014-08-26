using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace $AppName$.MSBuildTasks.$MSBuildProjectName$
{
  /// <summary>
	/// Sample task to access SharePoint object model,
	/// use this task in an msbuild file as follows
	/// <MSBuildTask StringParameter="NewTitle1" SiteUrl="http://moss" ContinueOnError="false" />
	/// </summary>
  public class MSBuildTask : Task
  {
    private string mStringParameter = "";
    private string mSiteUrl = "";

    [Required]
    public string StringParameter
    {
      get { return mStringParameter; }
      set { mStringParameter = value; }
    }
    
    [Required]
    public string SiteUrl
    {
        get { return mSiteUrl; }
        set { mSiteUrl = value; }
    }
		
    public override bool Execute()
    {
			try
			{
				using (SPSite siteCollection = new SPSite(mSiteUrl))
				{
					using (SPWeb web = siteCollection.OpenWeb())
					{
						//place code here access SharePoint object model
            //e.g. to rename a root web
            web.Title = mStringParameter;
            web.Update();
					}
				}

				//successfully finished
				Log.LogMessage("Operation completed successfully.");
				return true;
			}
			catch (Exception ex)
			{
				Log.LogMessage(ex.ToString());
			}

			//error happend
			return false;  
    }
  }
}
