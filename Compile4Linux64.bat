@echo off
cls
dotnet publish UbuntuModal\UbuntuModal.csproj -r  linux-x64 -o _PublishLinux
pause
