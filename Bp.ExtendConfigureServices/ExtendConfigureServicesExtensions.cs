using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.ExtendConfigureServices
{
	public static class ExtendConfigureServicesExtensions
	{
		private const string EXTEND_CONFIGURE_SERVICES_CLASS = "BpConfigureServices";
		private const string EXTEND_CONFIGURE_SERVICES_METHOD = "ExtendConfigureServices";
		private const string EXTEND_CONFIGURE_METHOD = "ExtendConfigure";

		public static void ExtendConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			var entryAssembly = Assembly.GetEntryAssembly();

			entryAssembly
				.GetType($"{entryAssembly.GetName().Name}.{EXTEND_CONFIGURE_SERVICES_CLASS}")?
				.GetMethod(EXTEND_CONFIGURE_SERVICES_METHOD, BindingFlags.Public | BindingFlags.Static)?
				.Invoke(null, new object[] {services, configuration});
		}
		
		public static void UseExtendConfigure(this IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
		{
			var entryAssembly = Assembly.GetEntryAssembly();

			entryAssembly
				.GetType($"{entryAssembly.GetName().Name}.{EXTEND_CONFIGURE_SERVICES_CLASS}")?
				.GetMethod(EXTEND_CONFIGURE_METHOD, BindingFlags.Public | BindingFlags.Static)?
				.Invoke(null, new object[] {app, env, configuration});
		}
	}
}