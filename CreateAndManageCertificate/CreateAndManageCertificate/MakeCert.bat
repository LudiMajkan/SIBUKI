REM Creating a Newline variable (the two blank lines are required!)
set NLM=^


set NL=^^^%NLM%%NLM%^%NLM%%NLM%
REM Example Usage:
echo There should be a newline%NL%inserted here.
C:
cd..
cd..
cd..
cd..
cd..
cd..

cd \Users\NenadPC\Desktop\sibuki\CreateAndManageCertificate\CreateAndManageCertificate

@echo off
echo Enter name:
set /p name=
echo Enter issuer name:
set /p issuerName=
makecert.exe -n "CN=%issuerName%" -r -pe -cy authority -sv %name%.pvk %name%.cer
@echo on
pause