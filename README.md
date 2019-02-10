# bp-netcore-packages
Repository to keep all BlackPearl C# Nuget packages.

## How to add new packages
Create new project (keep the `Bp.` namespace at the of the project)

### How to publish packages
By default the package version will be 1.0.0, to change it add to the package `.csproj` file:
```xml
<PropertyGroup>
    ...
    <Version>{{VERSION}}</Version>
    ...
</PropertyGroup>
```
Run using PowerShell the file `build-and-publish-packages.ps1`
and it will be uploaded to the Nuget repo