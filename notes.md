# Notes

## NuGet

**Pack**  

```PowerShell
dotnet pack .\src\{Package} /p:Version={Version} -o .\.nuget -c Release
```

**Push**

```PowerShell
dotnet nuget push .\.nuget\{Package}.nupkg --api-key {key} --source https://api.nuget.org/v3/index.json
```