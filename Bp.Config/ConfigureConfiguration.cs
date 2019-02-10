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
            var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
            config
                .SetBasePath($"{Directory.GetCurrentDirectory()}/Config/")
                .AddJsonFile($"{CONFIG_FILE_BASE_NAME}.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{CONFIG_FILE_BASE_NAME}.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{CONFIG_FILE_BASE_NAME}.{environmentName}.Local.json", optional: true,
                    reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}