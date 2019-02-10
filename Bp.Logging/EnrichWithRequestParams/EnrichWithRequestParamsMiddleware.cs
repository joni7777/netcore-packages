using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Bp.Logging.EnrichWithRequestParams
{
    public class EnrichWithRequestParamsMiddleware
    {
        private readonly RequestDelegate _next;

        public EnrichWithRequestParamsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (LogContext.PushProperty("transactionId", context.TraceIdentifier))
            {
                await _next.Invoke(context);
            }
        }
    }

    public static class EnrichWithRequestParamsMiddlewareExtensions
    {
        public static void UseEnrichWithRequestParams(this IApplicationBuilder app)
        {
            app.UseMiddleware<EnrichWithRequestParamsMiddleware>();
        }
    }
}