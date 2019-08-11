REM Deleting all current files in kOS.Safe.Core...
mkdir temp
copy .\kOS.Safe.Core\kOS.Safe.Core.csproj .\temp\kOS.Safe.Core.csproj
del /s /f /q .\kOS.Safe.Core\*.*
copy .\temp\kOS.Safe.Core.csproj .\kOS.Safe.Core\kOS.Safe.Core.csproj
rd /s /q temp

REM Generating a new kOS.Safe.Core...
xcopy /s .\kOS\src\kOS.Safe .\kOS.Safe.Core
rd /s /q .\kOS.Safe.Core\bin
rd /s /q .\kOS.Safe.Core\obj
del /s /f /q .\kOS.Safe.Core\Properties\AssemblyInfo.cs
del /s /f /q .\kOS.Safe.Core\kOS.Safe.csproj