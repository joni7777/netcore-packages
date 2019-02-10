using System.Net.Http;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Bp.Logging.Sinks.Mattermost
{
    public static class BpMattermostSinkExtensions
    {
        public static LoggerConfiguration BpMattermostSink(
            this LoggerSinkConfiguration loggerConfiguration,
            BpMattermostInfo bpMattermostInfo, HttpClient httpClient = null,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Error)
        {
            return loggerConfiguration.Sink(new BpMattermostSink(bpMattermostInfo, httpClient),
                restrictedToMinimumLevel);
        }
    }
}