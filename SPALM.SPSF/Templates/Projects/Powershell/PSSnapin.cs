namespace $AppName$.PowerShell.$PowerShellProjectName$
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Management.Automation;
    using System.ComponentModel;

    [RunInstaller(true)]
    public class $PowerShellProjectName$SnapIn : PSSnapIn
    {
        public override string Name
        {
            get { return "$PowerShellProjectName$"; }
        }
        public override string Vendor
        {
            get { return "$CopyrightCompanyName$"; }
        }
        public override string VendorResource
        {
            get { return "$PowerShellProjectName$,$CopyrightCompanyName$"; }
        }
        public override string Description
        {
            get { return "$PowerShellProjectDescription$"; }
        }
        public override string DescriptionResource
        {
            get { return "$PowerShellProjectName$,$PowerShellProjectDescription$"; }
        }
    }
}
