using System;
using System.Linq;
using Bp.Config;
using Bp.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

namespace Bp.ApiRunner
{
	public class Runner
	{
		public static void Run()
		{
			new WebHostBuilder()
				.ConfigureAppConfiguration(ConfigureConfiguration.AddConfigurationByEnvironment)
				.UseKestrel(ConfigureKestrel)
				.UseSerilog(SerilogInit.ConfigureLogger)
				.UseStartup<Startup>()
				.Build()
				.Run();
		}

		private static void ConfigureKestrel(WebHostBuilderContext builderContext, KestrelServerOptions options)
		{
			var kestrelConfiguration = builderContext.Configuration.GetSection("Kestrel");

			// If the environment variable ASPNETCORE_URL is available, override the config urls
			var environmentUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';');
			if (environmentUrls != null && environmentUrls.Length > 0)
			{
				Console.WriteLine("Using environment variable ASPNETCORE_URLS urls instead of appsettings Kestrel config");
				var httpEndpoint = environmentUrls.FirstOrDefault(url => url.Contains("http://"));
				if (httpEndpoint != null)
				{
					kestrelConfiguration["EndPoints:Http:Url"] = httpEndpoint;
				}
				var httpsEndpoint = environmentUrls.FirstOrDefault(url => url.Contains("https://"));
				if (httpsEndpoint != null)
				{
					kestrelConfiguration["EndPoints:Http:Url"] = httpsEndpoint;
				}
			}

			options.Configure(kestrelConfiguration);
		}
	}
}