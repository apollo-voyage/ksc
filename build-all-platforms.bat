call "./gen-kos-safe-core.bat"

cd ./kOS.Cli
start "win-x64" /B /WAIT dotnet publish -r win-x64 -c Release -o ../win-x64 /p:UseAppHost=true /p:PublishSingleFile=true /p:PublishTrimmed=true
start "win-x86" /B /WAIT dotnet.exe publish -r win-x86 -c Release -o ../win-x86 /p:UseAppHost=true /p:PublishSingleFile=true /p:PublishTrimmed=true
start "linux-x64" /B /WAIT dotnet.exe publish -r linux-x64 -c Release -o ../linux-x64 /p:UseAppHost=true /p:PublishSingleFile=true /p:PublishTrimmed=true
start "osx-x64" /B /WAIT dotnet.exe publish -r osx-x64 -c Release -o ../osx-x64 /p:UseAppHost=true /p:PublishSingleFile=true /p:PublishTrimmed=true