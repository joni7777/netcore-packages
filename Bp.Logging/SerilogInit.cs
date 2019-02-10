using System;
using Bp.Logging.Sinks.Mattermost;
using Bp.Logging.Sinks.Splunk;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace Bp.Logging
{
    public static class SerilogInit
    {
        private const string SERILOG_CUSTOM_LOGGERS_PATH = "Serilog:CustomLoggers:";

        public static void ConfigureLogger(WebHostBuilderContext builder, LoggerConfiguration configuration)
        {
            configuration
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.WithProperty("ServiceName", builder.Configuration.GetSection("Service")["name"]);

            var splunkInfo = builder.Configuration.GetSection($"{SERILOG_CUSTOM_LOGGERS_PATH}SplunkLogger")
                ?.Get<BpSplunkInfo>();
            if (splunkInfo != null)
            {
                if (!splunkInfo.IsValid())
                    throw new ArgumentException(BpSplunkInfo.SPLUNK_MISSING_CONFIGURATION_MESSAGE);
                configuration.WriteTo.BpSplunkSink(splunkInfo);
            }

            var mattermostInfo = builder.Configuration.GetSection($"{SERILOG_CUSTOM_LOGGERS_PATH}MattermostLogger")
                ?.Get<BpMattermostInfo>();
            if (mattermostInfo != null)
            {
                if (!mattermostInfo.IsValid())
                    throw new ArgumentException(BpMattermostInfo.MATTERMOST_MISSING_CONFIGURATION_MESSAGE);
                configuration.WriteTo.BpMattermostSink(mattermostInfo);
            }

            configuration.WriteTo.Console();
        }
    }
}