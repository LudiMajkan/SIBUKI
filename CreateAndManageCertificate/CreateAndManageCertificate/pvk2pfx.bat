
C:
cd..
cd..
cd..
cd..
cd..
cd..

cd \Users\NenadPC\Desktop\sibuki\CreateAndManageCertificate\CreateAndManageCertificate
@echo off
echo Enter name for .pvk and .cer files:
set /p namePvkAndCer=
echo Enter name for .pfx file:
set /p namePfx=

echo Enter password:
set "psCommand=powershell -Command "$pword = read-host 'Enter Password' -AsSecureString ; ^
    $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword); ^
        [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)""
for /f "usebackq delims=" %%p in (`%psCommand%`) do set password=%%p
pvk2pfx.exe -pvk %namePvkAndCer%.pvk -pi %password% -spc %namePvkAndCer%.cer -pfx %namePfx%.pfx -f
@echo on

pause