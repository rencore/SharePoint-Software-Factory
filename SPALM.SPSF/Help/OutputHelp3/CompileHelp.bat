@echo on

del F:\SPALM\SPSF\Dev\Source\SPALM.SPSF\Help\OutputHelp3\spsfhelp.mshc

echo Zippen
cd "F:\SPALM\SPSF\Dev\Source\SPALM.SPSF\Help\OutputHelp3"
"7za.exe" a -tzip spsfhelp.mshc @ziplist.txt
