#-----------------------------------------------
# make appx: copy publish files + refister appx
#-----------------------------------------------

#Get-AppxPackage -publisher "CN=Franck Gaspoz Software, O=Franck Gaspoz Corporation, C=US"
#Remove-AppxPackage MovieDbAssistant_1.0.0.0_x64__xtrrbsjxvn07w

#xcopy ..\MovieDbAssistant.App\bin\x64\Release\net8.0-windows10.0.22621.0\win-x64\ Content /Q /R /Y

& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\makepri.exe' createconfig /cf priconfig.xml /dq en-US
& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\makepri.exe' new /pr "C:\Users\franc\source\repos\MovieDbAssistant\MovieDbAssistant.App" /cf "C:\Users\franc\source\repos\MovieDbAssistant\package\priconfig.xml" /in MovieDbAssistant
#& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\makeappx.exe' pack 

#add-appxpackage –register AppxManifest.xml
