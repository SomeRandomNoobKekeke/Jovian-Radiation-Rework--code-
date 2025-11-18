@REM https://github.com/SomeRandomNoobKekeke/Barotrauma-CS-hybrid-compiler

@REM Put this in your "in memory" mod folder
echo off 

@REM Set those vars
@REM Note: it's space sensitive
SET CompileTo=JovianRadiationRework [Compiled]
SET ModAssemblyName=JovianRadiationRework
SET ModRootNamespace=JovianRadiationRework

@REM Folder paths should end in /
@REM Path to compiler, i store it in LocalMods/[ Compiler ]
SET CompilerDir="../[ Compiler ]/"
@REM This directory
SET SourceModDir="%cd%/"

@REM https://stackoverflow.com/a/60046276
for %%I in ("%~dp0.") do for %%J in ("%%~dpI.") do set ParentFolder=%%~dpnxJ
@REM echo %ParentFolder%

SET ModDeployDir="%ParentFolder%/%CompileTo%/"

@REM you can set -p WarningLevel=4 and remove /clp:ErrorsOnly if you like warnings
cd %CompilerDir%
dotnet build .\Compiler.sln -c Release /clp:ErrorsOnly -p WarningLevel=0 -p:ModAssemblyName=%ModAssemblyName% -p:ModRootNamespace=%ModRootNamespace% -p:SourceModDir=%SourceModDir% -p:ModDeployDir=%ModDeployDir%

pause
