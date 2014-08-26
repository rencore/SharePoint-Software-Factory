###############################################################################
# SharePoint Solution Deployer (SPSD)
# Version          : 4.1.2.2805
# Url              : http://spsd.codeplex.com
# Creator          : Matthias Einig
# License          : GPLv2
###############################################################################

# EDIT THIS FILE to perform actions before or after a deploy/update/retract command

#region BeforeDeploy
# Desc: use this target to perform operations before a deployment
function BeforeDeploy($vars){

    # add action here

}
#endregion

#region AfterDeploy
# Desc: use this target to perform operations after a successful deployment
function AfterDeploy($vars){

	# Sample PowerShell commands
    # New-SPSite -Url '$vars["SiteUrl"]' -OwnerAlias '$env:USERDOMAIN\$env:USERNAME' -Name 'Test Site' -Template 'STS#0'
    # Install-SPFeature -Path '[feature name]' -Force
    # Enable-SPFeature -Identity '[feature name]' -Url '$vars["SiteUrl"]' -Force
    # Enable-SPFeature -Identity [feature guid] -Url '$vars["SiteUrl"]' -Force
   
}
#endregion

#region BeforeRetract
# Desc: use this target to perform operations before retraction
function BeforeRetract($vars){
   
    # Sample PowerShell commands
    # Disable-SPFeature -Identity '[feature name]' -Url '$vars["SiteUrl"]' -Confirm:$false -Force 
    # Uninstall-SPFeature -Identity '[feature name]' -Confirm:$false -Force 
    # Remove-SPSite -Identity '$vars["SiteUrl"]' -Confirm:$false 
   
}
#endregion

#region AfterRetract
# Desc: use this target to perform operations after a successful retraction
function AfterRetract($vars){

    # add action here

}
#endregion

#region BeforeUpdate
# Desc: use this target to perform operations before update
function BeforeUpdate($vars){

    # add action here

}
#endregion

#region AfterUpdate
# Desc: use this target to perform operations after a successful update
function AfterUpdate($vars){

    # add action here

}
#endregion
