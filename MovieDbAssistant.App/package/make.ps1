#-----------------------------------------------
# make appx: copy publish files + refister appx
#-----------------------------------------------

Get-AppxPackage -publisher "CN=Franck Gaspoz Software, O=Franck Gaspoz Corporation, C=US"
Remove-AppxPackage MovieDbAssistant_1.0.0.0_x64__xtrrbsjxvn07w

xcopy ..\bin\Release\net8.0-windows10.0.22621.0\win-x64\ . /Q /R /Y

add-appxpackage –register AppxManifest.xml
