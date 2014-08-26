@echo on

del F:\SPALM\SPSF\Dev\Source\SPALM.SPSF\Help\OutputHelp3\spsfhelp.mshc

echo Zippen
cd "F:\SPALM\SPSF\Dev\Source\SPALM.SPSF\Help\OutputHelp3"
"7za.exe" a -tzip spsfhelp.mshc @ziplist.txt

"c:\Program Files\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-US /uninstall /productName "SPSF SharePoint Software Factory"

"c:\Program Files\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-US /install /sourceMedia "HelpContentSetup.msha"
