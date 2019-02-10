using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bp.RouterAliases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bp.HealthChecks
{
    public class SystemRouter : RouterBase
    {
        private readonly HealthCheckService _healthCheckService;

        public SystemRouter(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet("sanity")]
        public async Task<IDictionary<string, BpHealthReportEntry>> GetSanity()
        {
            var reports = (await _healthCheckService.CheckHealthAsync(checks =>
                checks.Tags.Contains(HealthCheckTag.SANITY))).Entries;

            return reports.ToDictionary(pair => pair.Key, pair => new BpHealthReportEntry(pair.Value));
        }

        [HttpGet("data")]
        public async Task<IReadOnlyDictionary<string, BpHealthReportEntry>> GetData()
        {
            var reports = (await _healthCheckService.CheckHealthAsync(checks =>
                checks.Tags.Contains(HealthCheckTag.DATA))).Entries;

            return reports.ToDictionary(pair => pair.Key, pair => new BpHealthReportEntry(pair.Value));
        }
    }
}