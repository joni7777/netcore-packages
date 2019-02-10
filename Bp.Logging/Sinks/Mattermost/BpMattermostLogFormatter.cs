using System;
using System.Text;
using Serilog.Events;

namespace Bp.Logging.Sinks.Mattermost
{
    public class BpMattermostLogFormatter
    {
        private const string CODE_PREFIX = "```";

        public static string Format(LogEvent log)
        {
            var logMessage = new StringBuilder();

            logMessage.AppendLine($"{Monospace(log.RenderMessage())} - ");

            return logMessage.ToString();
        }

        private static string Bold(string message)
        {
            return $"**{message}**";
        }

        private static string Hyperlink(string message, string link)
        {
            return $"[{message}]({link})";
        }

        private static string Emoji(string name)
        {
            return $":{name}:";
        }

        private static string Monospace(string message)
        {
            return $"`{message}`";
        }

        private static string Code(string message, string lang)
        {
            return $"{CODE_PREFIX}{lang}{Environment.NewLine}{message}{Environment.NewLine}{CODE_PREFIX}";
        }
    }
}