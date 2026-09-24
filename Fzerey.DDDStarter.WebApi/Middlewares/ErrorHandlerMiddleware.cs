using Fzerey.DDDStarter.Application.Common.Exceptions.Base;
using Fzerey.DDDStarter.Domain.Exceptions;
using Fzerey.DDDStarter.WebApi.Models.Exception;
using Serilog.Context;
using System.Net;

namespace Fzerey.DDDStarter.WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogError(error, "Unhandled exception after the response started");
                    throw;
                }
                await HandleExceptionAsync(context, error);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorModel = new ErrorModel();
            switch (error)
            {
                case ValidationException validationException:
                    errorModel.Message = validationException.Message;
                    errorModel.ErrorCode = validationException.Code;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case DomainException domainException:
                    errorModel.Message = domainException.Message;
                    errorModel.ErrorCode = domainException.Code;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case NotFoundException notFoundException:
                    errorModel.Message = notFoundException.Message;
                    errorModel.ErrorCode = notFoundException.Code;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorModel.Message = "Unexpected exception";
                    break;
            }

            var correlationId = context.Request.Headers[CorrelationIdMiddleware.CorrelationHeaderKey].ToString();
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                _logger.LogError(error, "Message: {Message} CorrelationId: {CorrelationId}", errorModel.Message, correlationId);
            }
            await response.WriteAsync(errorModel.ToString());
        }
    }
}
