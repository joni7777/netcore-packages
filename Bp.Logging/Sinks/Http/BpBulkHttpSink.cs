using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using Serilog.Core;
using Serilog.Events;

namespace Bp.Logging.Sinks.Http
{
    public abstract class BpBulkHttpSink : ILogEventSink, IDisposable
    {
        private string _requestUrl;
        private readonly Timer _timer;
        private readonly HttpClient _httpClient;
        private readonly ConcurrentBag<LogEvent> _logs;
        private const int DEFAULT_INTERVAL_PERIOD = 1000;
        private const int START_NOW_DUE_TIME = 0;
        protected virtual HttpMethod HttpMethod { get; } = HttpMethod.Post;

        protected BpBulkHttpSink(HttpClient httpClient, Timer timer = null, ConcurrentBag<LogEvent> logs = null)
        {
            _httpClient = httpClient;
            _logs = logs ?? new ConcurrentBag<LogEvent>();
            _timer = timer ?? new Timer(BulkEmit, null, START_NOW_DUE_TIME, DEFAULT_INTERVAL_PERIOD);
        }

        public virtual void Emit(LogEvent logEvent)
        {
            _requestUrl = GenerateRequestUri(logEvent);
            _logs.Add(logEvent);
        }

        public virtual void BulkEmit(object timerState)
        {
            if (_logs.IsEmpty)
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod, _requestUrl)
            {
                Content = GenerateRequestBody(EmptyLogsBag(_logs))
            };
            ConfigureHttpRequest(request);
            _httpClient.SendAsync(request);
        }

        protected virtual void ConfigureHttpRequest(HttpRequestMessage request)
        {
        }

        protected abstract string GenerateRequestUri(LogEvent logEvent);
        protected abstract HttpContent GenerateRequestBody(ConcurrentBag<LogEvent> logs);

        public void Dispose()
        {
            _httpClient?.Dispose();
            _timer?.Dispose();
        }

        private ConcurrentBag<LogEvent> EmptyLogsBag(ConcurrentBag<LogEvent> logs)
        {
            ConcurrentBag<LogEvent> existingLogs = new ConcurrentBag<LogEvent>();

            while (!logs.IsEmpty)
            {
                if (logs.TryTake(out LogEvent log))
                {
                    existingLogs.Add(log);
                }
            }

            return existingLogs;
        }
    }
}