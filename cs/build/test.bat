cd /d %~dp0
cd ..
C:\Windows\Microsoft.NET\Framework64\v3.5\csc /target:exe /out:lib/test_vectrburner.exe /reference:lib/Atagoal.dll /recurse:*.cs
lib\test_vectrburner.exe
del lib\test_vectrburner.exe
pause