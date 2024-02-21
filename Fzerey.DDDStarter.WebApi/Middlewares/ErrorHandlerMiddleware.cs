using Fzerey.DDDStarter.Application.Common.Exceptions.Base;
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
                await HandleExceptionAsync(context, error);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorModel = new ErrorModel();
            switch (error.GetType().BaseType.Name)
            {
                case "ValidationException":
                    var validationException = (ValidationException)error;
                    errorModel.Message = validationException.Message;
                    errorModel.ErrorCode = validationException.Code;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case "NotFoundException":
                    var notFoundException = (NotFoundException)error;
                    errorModel.Message = notFoundException.Message;
                    errorModel.ErrorCode = notFoundException.Code;
                    response.StatusCode = (int)HttpStatusCode.UnprocessableEntity; // 404 does not work due to AWS CloudFront
                    break;

                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorModel.Message = "Unexpected exception";
                    break;
            }

            using (LogContext.PushProperty("CorrelationId", response.Headers[CorrelationIdMiddleware.CorrelationHeaderKey]))
            {
                _logger.LogError(error, $"Message: {errorModel.Message} CorrelationId: {response.Headers[CorrelationIdMiddleware.CorrelationHeaderKey]}");
            }
            await response.WriteAsync(errorModel.ToString());
        }
    }
}
