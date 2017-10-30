cd /d %~dp0
cd ../src
C:\Windows\Microsoft.NET\Framework64\v3.5\csc /target:library /out:../bin/VectorBurner.dll /recurse:*.cs /reference:../lib/Atagoal.dll
pause