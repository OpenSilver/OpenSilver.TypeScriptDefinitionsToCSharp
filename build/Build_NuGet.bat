@ECHO off

SETLOCAL

IF "%~1" == "--help" (
	GOTO :help
)

IF "%~1" == "-h" (
	GOTO :help
)

SET BUILD_DIR=%~dp0
SET SRC_DIR=%~dp0..\src

REM Define the escape character for colored text
FOR /F %%a IN ('"prompt $E$S & echo on & for %%b in (1) do rem"') DO SET "ESC=%%a"

IF "%~1" == "" (
	SET /P PackageVersion="%ESC%[92mOpenSilver.TypeScriptDefinitionsToCSharp version:%ESC%[0m "
	SET /P OpenSilverPkgVersion="%ESC%[92mOpenSilver version:%ESC%[0m "
) ELSE (
	SET PackageVersion=%1
	IF "%~2" == "" (
		SET OpenSilverPkgVersion=%1
	) ELSE (
		SET OpenSilverPkgVersion=%2
	) 
)

FOR /F "delims=" %%a IN ('powershell -Command "[guid]::NewGuid().ToString('N')"') DO SET BUILD_UUID=%%a

ECHO. 
ECHO %ESC%[95mBuilding %ESC%[0mRelease %ESC%[95mconfiguration%ESC%[0m
ECHO. 
msbuild %SRC_DIR%\OpenSilver.TypeScriptDefinitionsToCSharp.sln -p:Configuration=Release;OpenSilverBuildUUID=%BUILD_UUID%;OpenSilverVersion=%OpenSilverPkgVersion% -verbosity:minimal -restore

ECHO. 
ECHO %ESC%[95mPacking %ESC%[0mOpenSilver.TypeScriptDefinitionsToCSharp %ESC%[95mNuGet package%ESC%[0m
ECHO. 
%BUILD_DIR%\nuget.exe pack %BUILD_DIR%\nuspec\OpenSilver.TypeScriptDefinitionsToCSharp.nuspec -OutputDirectory "%BUILD_DIR%\output" -Properties "PackageVersion=%PackageVersion%;OpenSilverDependencyVersion=%OpenSilverPkgVersion%;Configuration=Release;OpenSilverBuildUUID=%BUILD_UUID%"

EXIT /b

:help
ECHO [1] OpenSilver.TypeScriptDefinitionsToCSharp NuGet package Version
ECHO [2] OpenSilver Version

ENDLOCAL