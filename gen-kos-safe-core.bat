xcopy /s .\kOS\src\kOS.Safe .\kOS.Safe.Core
rd /s/q .\kOS.Safe.Core\bin
rd /s/q .\kOS.Safe.Core\obj
del /s /f /q .\kOS.Safe.Core\Properties\AssemblyInfo.cs
del /s /f /q .\kOS.Safe.Core\kOS.Safe.csproj