using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.HealthChecks
{
    public static class ConfigureHealthChecksExtensions
    {
        public static void ConfigureBpHealthChecksServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            IHealthChecksBuilder healthChecks = services.AddHealthChecks();

            string sqlConfig = configuration["Data:SqlServer:ConnectionString"];
            if (sqlConfig != null)
            {
                healthChecks.AddSqlServer(sqlConfig, name: "sql-server-health", tags: new[] {HealthCheckTag.DATA});
            }

            string mongoConfig = configuration["Data:MongoDB:ConnectionString"];
            if (mongoConfig != null)
            {
                healthChecks.AddMongoDb(mongoConfig, name: "mongodb-health", tags: new[] {HealthCheckTag.DATA});
            }

            // If server have https, the http endpoint will redirect to the https it and the health check will fail of the redirect
            if (configuration["Kestrel:EndPoints:Https:Url"] == null && configuration["Kestrel:EndPoints:Http:Url"] != null)
            {
                healthChecks.AddUrlGroup(
                    new Uri(
                        $"{configuration["Kestrel:EndPoints:Http:Url"].Replace("*", "localhost")}/swagger/{configuration["Service:Version"]}/swagger.json"),
                    name: "Get swagger.json",
                    tags: new[] {HealthCheckTag.SANITY});
            }
        }

        public static void UseBpHealthChecks(this IApplicationBuilder app)
        {
            app
                .ApplicationServices.GetService<ApplicationPartManager>()
                .ApplicationParts.Add(new AssemblyPart(Assembly.GetExecutingAssembly()));
        }
    }
}