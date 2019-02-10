using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bp.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bp.EndPointer
{
	public class EndPointer : IDisposable
	{
		private readonly string _environment;
		private readonly ServiceInfo _serviceInfo;
		private readonly ILogger<EndPointer> _logger;
		private readonly string _endPointerUrl;
		private readonly HttpClient _httpClient;

		public EndPointer(string endPointerUrl, string environment, ServiceInfo serviceInfo,
			ILogger<EndPointer> logger = null, HttpClient httpClient = null)
		{
			_endPointerUrl = endPointerUrl;
			_environment = environment;
			_serviceInfo = serviceInfo;
			_logger = logger;
			_httpClient = httpClient ?? new HttpClient();
		}

		public async Task Register(string baseUrl)
		{
			var ctx = new CancellationTokenSource();
			ctx.CancelAfter(3000);

			try
			{
				var respone = await _httpClient.PostAsync($"{_endPointerUrl}/{_environment}",
					new StringContent(
						JsonConvert.SerializeObject(new
							{name = _serviceInfo.Name, url = baseUrl + _serviceInfo.SwaggerUrl}), Encoding.UTF8,
						"application/json"), ctx.Token);

				if (!respone.IsSuccessStatusCode)
				{
					_logger?.LogWarning("Failed to register to the endpointer", respone.ReasonPhrase);
				}
				else
				{
					_logger?.LogInformation("Registered successfully to the endpointer", respone.ReasonPhrase);
				}
			}
			catch (Exception e)
				when ((e is HttpRequestException && e.Message == "Connection refused") || e is TaskCanceledException)
			{
				_logger?.LogWarning("Failed to register to the endpointer because of ", e.Message, e);
				throw;
			}
		}

		public void Dispose()
		{
			_httpClient?.Dispose();
		}
	}
}