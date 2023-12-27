dotnet build
Copy-Item -Path .\bin\Debug\netstandard2.1\BlissScrap.dll -Destination "./thunderstore"
Copy-Item -Path .\bin\Debug\netstandard2.1\BlissScrap.pdb -Destination "./thunderstore"
