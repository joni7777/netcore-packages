using System;
using System.Threading;
using System.Threading.Tasks;
using Bp.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bp.EndPointer
{
	public class RegisterEndpointerHostedService : IHostedService, IDisposable
	{
		private string _serverAddress;
		private EndPointer _endPointer;

		public RegisterEndpointerHostedService(IHostingEnvironment hostingEnvironment, IConfiguration config,
			ILogger<EndPointer> logger)
		{
			var serviceInfo = config.GetSection("Service").Get<ServiceInfo>();
			var endPointerInfo = config.GetSection("EndPointer").Get<EndPointerInfo>();
			_endPointer = new EndPointer(endPointerInfo.Url, hostingEnvironment.EnvironmentName, serviceInfo, logger);
			_serverAddress = Environment.GetEnvironmentVariable("RUNNING_SERVICE_URL") ?? ParseServerAddress(config.GetSection("Kestrel:Endpoints:Http:Url").Value ??
			                                    config.GetSection("Kestrel:Endpoints:Https:Url").Value ?? "");
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			return _endPointer.Register(_serverAddress);
		}

		/// <summary>
		/// Parse the configuration kestrel server listen address
		/// The address in the configuration will include '[::]' or 'localhost' or '*' and replacing it with the hostname
		/// </summary>
		/// <param name="address">Kestrel config listen address. Example: http://*:5000 OR http://[::]:5000 OR http://localhost:5000</param>
		/// <returns>Returns the same address but the '[::]' or 'localhost' or '*' replaced with hostname</returns>
		private string ParseServerAddress(string address)
		{
			return address
				.Replace("[::]", Environment.MachineName)
				.Replace("localhost", Environment.MachineName)
				.Replace("*", Environment.MachineName);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_endPointer.Dispose();
		}
	}
}