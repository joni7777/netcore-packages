using System;
using System.Net.Http;
using Serilog.Core;
using Serilog.Events;

namespace Bp.Logging.Sinks.Http
{
    public abstract class BpHttpSink : ILogEventSink, IDisposable
    {
        private readonly HttpClient _httpClient;

        protected virtual HttpMethod HttpMethod { get; } = HttpMethod.Post;

        protected BpHttpSink(HttpClient httpClient) => _httpClient = httpClient;

        public virtual void Emit(LogEvent logEvent)
        {
            var request = new HttpRequestMessage(HttpMethod, GenerateRequestUri(logEvent))
            {
                Content = GenerateRequestBody(logEvent)
            };
            ConfigureHttpRequest(request);
            _httpClient.SendAsync(request);
        }

        protected virtual void ConfigureHttpRequest(HttpRequestMessage request)
        {
        }

        protected abstract string GenerateRequestUri(LogEvent logEvent);
        protected abstract HttpContent GenerateRequestBody(LogEvent logEvent);

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}