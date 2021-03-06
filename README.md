# bp-netcore-packages
Repository to keep all BlackPearl C# Nuget packages.

## How to add new packages
Create new project (keep the `Bp.` namespace at the of the project)
In the new project `.csproj` file make sure the following props exists
```xml
<PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <PackageId>{{PACKAGE_NAME}}</PackageId>
    <Version>{{VERSION}}</Version>
    <Authors>{{AUTHOR}}</Authors>
    <Description>{{DESCRIPTION}}</Description>
    <RepositoryUrl>https://github.com/joni7777/netcore-packages.git</RepositoryUrl>
    <RepositoryType>git</RepositoryType>
</PropertyGroup>
```

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
Don't forget to add to the script arguments the nuget api key and nuget api source.
Example: `pwsh build-and-publish-packages.ps1 -nugetApiKey {{NUGET_API_KEY}} -nugetApiSource https://api.nuget.org/v3/index.json
`

## Packages

### Bp.ApiRunner
Init the Kestrel server, and run the startup with all of the packages commonly used in the BpSeed

### Bp.Common
Add common Functions, Dtos, Models, Constants and such

### Bp.Config
Add Config to the project using environment
It assumes the config will be in directory in the main seed project
and it will loads the configs by the order:

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. `appsettings.{Environment}.local.json`

and then the environment variables

### Bp.EndPointer
Register the service in the EndPointer using configuration
```json
{
    ...
    "EndPointer": {
        "Url": "{URL}"
    },
    ...
}
```
The service will be registered by the service name from the config service info section
and with the address of the RUNNING_SERVICE_URL environment variable OR Kestrel config endpoints and the computer hostname 

### Bp.ExtendConfigureServices
Allow extending configure services of the ApiRunner startup
Using reflection:
1. loads the EntryAssembly
2. looks for Class with the name: `BpConfigureServices`
3. look for A static public method: `ExtendConfigureServices` AND OR `ExtendConfigure` 
4. invoke the method(s) from before with the default startup arguments (`IServiceCollection`, `IConfiguration` for `ExtendConfigureServices` and `IApplicationBuilder`, `IHostingEnvironment`, `IConfiguration` for `ExtendConfigure`) 
If the class or one of the methods are null it just skip the null class or method

### Bp.HealthChecks
Add health checks for the application
If the app is not in production add the default endpoint swagger.json under system tag
Auto add data health checks using the config paths:
1. SqlServer: Data:SqlServer:ConnectionString
2. Mongo: Data:MongoDB:ConnectionString

### Bp.Logging
Add default logger sinks for mattermost and splunk and console
For splunk logger have in the config:
```json
{
    "Serilog": {
        ...
        "CustomLoggers": {
            ...
            "SplunkLogger": {
                "Protocol": "http",
                "Host": "localhost",
                "Port": "3000",
                "Username": "Username",
                "Password": "Password",
                "SourceType": "SourceType",
                "Index": "Index"
            },
            ...
        },
        ...
    }
}
```

For mattermost logger have in the config:
```json
{
    "Serilog": {
        ...
        "CustomLoggers": {
            ...
            "MattermostLogger": {
                "Protocol": "http",
                "Host": "localhost",
                "Port": "3000",
                "Path": "bla/6"
            },
            ...
        },
        ...
    }
}
```

Mattermost by default will get only logs from Error level
Splunk by default will get only logs from Debug level

## Bp.RouterAliases
Add Router aliases instead of controller from NetCore

## Bp.Cache
Provide singleton cache service for the application
The service expects to have cache interval config in the config location `Data:CacheInterval`