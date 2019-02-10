using System.Net.Http;
using System.Text;
using Bp.Logging.Sinks.Http;
using Serilog.Events;

namespace Bp.Logging.Sinks.Mattermost
{
    public class BpMattermostSink : BpHttpSink
    {
        private readonly BpMattermostInfo _bpMattermostInfo;

        public BpMattermostSink(BpMattermostInfo bpMattermostInfo, HttpClient httpClient = null)
            : base(httpClient ?? new HttpClient())
        {
            _bpMattermostInfo = bpMattermostInfo;
        }

        protected override string GenerateRequestUri(LogEvent logEvent)
        {
            return
                $"{_bpMattermostInfo.Protocol}://{_bpMattermostInfo.Host}:{_bpMattermostInfo.Port}/{_bpMattermostInfo.Path}";
        }

        protected override HttpContent GenerateRequestBody(LogEvent logEvent)
        {
            logEvent.Properties.TryGetValue("ServiceName", out var serviceName);

            var mattermostMessage = BpMattermostLogFormatter.Format(logEvent);

            var stringLog = new StringBuilder();
            stringLog.Append('{');
            stringLog.Append("\"username\":\"").Append(serviceName).Append("\",");
            stringLog.Append("\"text\":\"").Append(mattermostMessage).Append("\"");
            stringLog.Append("\"");
            stringLog.Append('}');
            return new StringContent(stringLog.ToString(), Encoding.UTF8, "text/plain");
        }
    }
}