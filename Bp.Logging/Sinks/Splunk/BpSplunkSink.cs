using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Bp.Logging.Sinks.Http;
using Serilog.Events;

namespace Bp.Logging.Sinks.Splunk
{
    public class BpSplunkSink : BpBulkHttpSink
    {
        private readonly BpSplunkInfo _splunkInfo;
        private readonly AuthenticationHeaderValue _basicAuth;

        public BpSplunkSink(BpSplunkInfo splunkInfo, ConcurrentBag<LogEvent> logs = null, Timer timer = null,
            HttpClient httpClient = null)
            : base(httpClient ?? new HttpClient(), timer, logs)
        {
            _basicAuth = CreateBasicAuth(splunkInfo);
            _splunkInfo = splunkInfo;
        }

        protected override void ConfigureHttpRequest(HttpRequestMessage request)
        {
            request.Headers.Authorization = _basicAuth;
        }

        private AuthenticationHeaderValue CreateBasicAuth(BpSplunkInfo splunkInfo)
        {
            string usernamePassword = $"{splunkInfo.Username}:{splunkInfo.Password}";
            byte[] encodedBytes = Encoding.UTF8.GetBytes(usernamePassword);

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(encodedBytes));
        }

        protected override string GenerateRequestUri(LogEvent logEvent)
        {
            logEvent.Properties.TryGetValue("ServiceName", out var serviceName);
            return
                $"{_splunkInfo.Protocol}://{_splunkInfo.Host}:{_splunkInfo.Port}/services/receivers/simple?index={_splunkInfo.Index}&sourcetype={_splunkInfo.SourceType}&source={serviceName.ToString().Replace("\"", "")}&output_mode=json";
        }

        protected override HttpContent GenerateRequestBody(ConcurrentBag<LogEvent> logs)
        {
            var joinedLogs = new StringBuilder();

            foreach (LogEvent log in logs)
            {
                joinedLogs.AppendLine(BpSplunkLogFormatter.Format(log));
            }

            return new StringContent(joinedLogs.ToString(), Encoding.UTF8, "text/plain");
        }
    }
}
