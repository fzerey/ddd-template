using Microsoft.Extensions.Primitives;

namespace Fzerey.DDDStarter.WebApi.Middlewares
{
    public class CorrelationIdMiddleware
    {
        internal const string CorrelationHeaderKey = "CorrelationId";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = context.Request.Headers[CorrelationHeaderKey];
            if (StringValues.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[CorrelationHeaderKey] = correlationId;
            }

            context.Response.Headers[CorrelationHeaderKey] = correlationId;
            await _next.Invoke(context);
        }
    }
}
