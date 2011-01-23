msbuild.exe src\UnityAutoMoq.sln /p:Configuration=Release;OutDir=%cd%\bin\
tools\NuGet.exe update
tools\NuGet.exe pack UnityAutoMoq.nuspec -o bin\