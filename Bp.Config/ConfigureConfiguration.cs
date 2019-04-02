using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Bp.Config
{
    public static class ConfigureConfiguration
    {
        private const string CONFIG_FILE_BASE_NAME = "appsettings";

        public static void AddConfigurationByEnvironment(WebHostBuilderContext hostingContext,
            IConfigurationBuilder config)
        {
            // To avoid the default environment Production if ASPNETCORE_ENVIRONMENT is empty, setting environment to Development
            var environmentName = Environment.GetEnvironmentVariable($"ASPNETCORE_{WebHostDefaults.EnvironmentKey.ToUpper()}") ?? "Development";
            hostingContext.HostingEnvironment.EnvironmentName = environmentName;
            
            config
                .SetBasePath($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Config{Path.DirectorySeparatorChar}")
                .AddJsonFile($"{CONFIG_FILE_BASE_NAME}.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{CONFIG_FILE_BASE_NAME}.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{CONFIG_FILE_BASE_NAME}.{environmentName}.Local.json", optional: true,
                    reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}