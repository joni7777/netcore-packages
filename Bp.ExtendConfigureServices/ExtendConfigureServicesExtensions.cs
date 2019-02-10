using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.ExtendConfigureServices
{
	public static class ExtendConfigureServicesExtensions
	{
		private const string EXTEND_CONFIGURE_SERVICES_CLASS = "BpConfigureServices";
		private const string EXTEND_CONFIGURE_SERVICES_METHOD = "ExtendConfigureServices";

		public static void ExtendConfigureServices(this IServiceCollection services)
		{
			var entryAssembly = Assembly.GetEntryAssembly();

			entryAssembly
				.GetType($"{entryAssembly.GetName().Name}.{EXTEND_CONFIGURE_SERVICES_CLASS}")?
				.GetMethod(EXTEND_CONFIGURE_SERVICES_METHOD, BindingFlags.Public | BindingFlags.Static)?
				.Invoke(null, new object[] {services});
		}
	}
}