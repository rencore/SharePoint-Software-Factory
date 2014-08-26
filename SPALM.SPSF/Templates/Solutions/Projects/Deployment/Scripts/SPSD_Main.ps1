###############################################################################
# SharePoint Solution Deployer (SPSD)
# Version          : 4.1.2.2805
# Url              : http://spsd.codeplex.com
# Creator          : Matthias Einig
# License          : GPLv2
###############################################################################

param 
(
    #optional parameter
    [ValidateNotNullOrEmpty()]
    [ValidateSet('Deploy', 'Redeploy', 'Retract', 'Update')]
    [string]$Command = "Deploy",

    [ValidateNotNullOrEmpty()]
    [ValidateSet('All', 'Solutions', 'Structure')]
    [string]$Type = "All",          # Makes it possible to only deploy the solutions or the defined site structure

    [ValidateNotNullOrEmpty()]
    [ValidateSet('Error', 'Warning', 'Information', 'Verbose', 'VerboseExtended')]
    [string]$Verbosity = "verbose", # defines how detailed the log is created each level includes the ones above
    [string]$envFile,               # external environment configuration file
    [switch]$saveEnvXml = $true,    # filename of the used environment configuration (merged file of referenced files with replaced variables)
    [string]$solutionDirectory = "" # Optional: specify a custom folder location where the solutions files are stored

)
cls

#region Include External Functions and Configuration
$0 = $myInvocation.MyCommand.Definition
$scriptDir = [System.IO.Path]::GetDirectoryName($0)
. $scriptDir"\SPSD_Base.ps1"
. $scriptDir"\SPSD_Utilities.ps1"
. $scriptDir"\SPSD_Deployment.ps1"
. $scriptDir"\CustomTargets.ps1"
#endregion

try{
    StartUp
	LoadEnvironment
	RunDeployment
}
catch{
	ErrorSummary
}
finally{
	FinishUp
	Pause
}
