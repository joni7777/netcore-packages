using System;
using System.Text;
using Serilog.Context;
using Serilog.Events;

namespace Bp.Logging.Sinks.Splunk
{
    public class BpSplunkLogFormatter
    {
        public static string Format(LogEvent logEvent)
        {
            var formattedLog = new StringBuilder();

            formattedLog.Append($"timestamp=\"{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()}\" ");
            formattedLog.Append($"severity=\"{logEvent.Level}\" ");
            formattedLog.Append($"action=\"{logEvent.RenderMessage()}\" ");

            foreach (var property in logEvent.Properties)
            {
                formattedLog.Append($"{property.Key}={property.Value} ");
            }

            return formattedLog.ToString();
        }
    }
}