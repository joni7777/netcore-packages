using Bp.Config;
using Bp.Logging;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Bp.ApiRunner
{
	public class Runner
	{
		public static void Run()
		{
			new WebHostBuilder()
				.ConfigureAppConfiguration(ConfigureConfiguration.AddConfigurationByEnvironment)
				.UseKestrel((builderContext, options) =>
				{
					options.Configure(builderContext.Configuration.GetSection("Kestrel"));
				})
				.UseSerilog(SerilogInit.ConfigureLogger)
				.UseStartup<Startup>()
				.Build()
				.Run();
		}
	}
}