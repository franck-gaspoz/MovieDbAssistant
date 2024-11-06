#-----------------------------------------------
# make cert: generates a self signed certificate and export to Pfx
#-----------------------------------------------

# for manifest
# <Identity 
# 	Name="MovieDbAssistant" 
# 	Version="1.0.0.0" 
# 	Publisher="CN=Franck Gaspoz Software, O=Franck Gaspoz Corporation, C=US"
# 	ProcessorArchitecture="x64" />

New-SelfSignedCertificate -Type Custom -Subject "CN=Franck Gaspoz Software, O=Franck Gaspoz Corporation, C=US" -KeyUsage DigitalSignature -FriendlyName "Franck Gaspoz" -CertStoreLocation "Cert:\CurrentUser\My" -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")

# fingerprint: FDC7C1469D9328B522B8078744331C18A55171F6

#Set-Location Cert:\CurrentUser\My
#Get-ChildItem | Format-Table Subject, FriendlyName, Thumbprint
dir "Cert:\CurrentUser\My"

# export

$password = ConvertTo-SecureString -String mypassword1234 -Force -AsPlainText 

Get-ChildItem -Path Cert:\CurrentUser\My\FDC7C1469D9328B522B8078744331C18A55171F6 |
    Export-PfxCertificate -FilePath ./franck-gaspoz-software-cert.pfx -Password $password

# sign
#method (AppxBlockMap.xml:BlockMap:HashMethod) : http://www.w3.org/2001/04/xmlenc#sha256

& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\SignTool.exe' sign /fd SHA256 /a /f franck-gaspoz-software-cert.pfx /p mypassword1234 MovieDbAssistant_1.0.0_x64_win.msix

## Done Adding Additional Store
## Successfully signed: MovieDbAssistant_1.0.0_x64_win.msix
