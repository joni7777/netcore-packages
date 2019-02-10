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
and with the address of the Kestrel config endpoints and the computer hostname 

### Bp.ExtendConfigureServices
Allow extending configure services of the ApiRunner
Using reflection:
1. loads the EntryAssembly
2. looks for Class with the name: `BpConfigureServices`
3. look for A static public method: `ExtendConfigureServices`
4. invoke the method from before with the services collection
If the class or the method are null it just continue

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