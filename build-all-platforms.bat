call "./gen-kos-safe-core.bat"
start "win-x64" /B /WAIT dotnet publish -r win-x64 -c Release -o ./win-x64
start "win-x86" /B /WAIT dotnet.exe publish -r win-x86 -c Release -o ./win-x86
start "linux-x64" /B /WAIT dotnet.exe publish -r linux-x64 -c Release -o ./linux-x64
start "osx-x64" /B /WAIT dotnet.exe publish -r osx-x64 -c Release -o ./osx-x64