namespace Fzerey.DDDStarter.WebApi.Models.Exception
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
            var correlationId = Guid.NewGuid();

            context.Request?.Headers.Add(CorrelationHeaderKey, correlationId.ToString());
            context.Response?.Headers.Add(CorrelationHeaderKey, correlationId.ToString());
            await _next.Invoke(context);
        }
    }
}