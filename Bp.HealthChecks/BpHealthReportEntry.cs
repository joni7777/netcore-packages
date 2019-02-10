using System;
using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bp.HealthChecks
{
    public class BpHealthReportEntry
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public Exception Exception { get; set; }
        public IReadOnlyDictionary<string, object> Data { get; set; }

        public BpHealthReportEntry(HealthStatus status, string description, TimeSpan duration, Exception exception,
            IReadOnlyDictionary<string, object> data)
        {
            Status = status.ToString();
            Description = description;
            Duration = duration;
            Exception = exception;
            Data = data;
        }

        public BpHealthReportEntry(HealthReportEntry healthReportEntry)
            : this(healthReportEntry.Status, healthReportEntry.Description, healthReportEntry.Duration,
                healthReportEntry.Exception, healthReportEntry.Data)
        {
        }
    }
}