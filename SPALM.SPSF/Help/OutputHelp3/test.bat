@echo off
echo.

set WORKING_DIRECTORY=%~dp0

For /F "tokens=3-7" %%i in ('REG QUERY "HKLM\SOFTWARE\Microsoft\Help\v1.0" /v AppRoot') Do (SET thepath=%%i %%j %%k %%l)

"%thepath%HelpLibManager.exe" /product VS /version 100 /locale en-US /install /sourceMedia "%WORKING_DIRECTORY%HelpContentSetup.msha"

