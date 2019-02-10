using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Bp.Logging.Sinks.Splunk
{
    public static class BpSplunkSinkExtensions
    {
        public static LoggerConfiguration BpSplunkSink(
            this LoggerSinkConfiguration loggerConfiguration,
            BpSplunkInfo splunkInfo, ConcurrentBag<LogEvent> logs = null, Timer timer = null,
            HttpClient httpClient = null, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Information)
        {
            return loggerConfiguration.Sink(new BpSplunkSink(splunkInfo, logs, timer, httpClient),
                restrictedToMinimumLevel);
        }
    }
}