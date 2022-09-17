using System.Net;
using SuitsAltering.API.Common;
using SuitsAltering.Infrastructure.Extensions;

namespace SuitsAltering.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (HttpRequestException ex)
            {
                await HandleHttpRequestException(ctx, ex);
            }
            catch (Exception ex)
            {
                await HandleSystemException(ctx, ex);
            }
        }

        private async Task HandleHttpRequestException(HttpContext ctx, HttpRequestException ex)
        {
            var level = LogLevel.Error;
            var statusCode = (int)HttpStatusCode.BadGateway;
            var messageToLog = GetExceptionMessage(ex);

            var error = new ErrorResponse.ErrorModel
            {
                Code = statusCode.ToString(),
                Message = ex.Message,
            };
            Log(ex, level, statusCode, messageToLog);
            await WriteToResponse(ctx, statusCode, error);
        }

        private async Task HandleSystemException(HttpContext ctx, Exception ex)
        {
            var level = LogLevel.Error;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var messageToLog = GetExceptionMessage(ex);

            var error = new ErrorResponse.ErrorModel
            {
                Code = HttpStatusCode.InternalServerError.ToString(),
                Message = "Internal server error.",
            };
            Log(ex, level, statusCode, messageToLog);
            await WriteToResponse(ctx, statusCode, error);
        }

        private string GetExceptionMessage(Exception ex)
        {
            return $"{ex.Message}{(ex.InnerException == null ? string.Empty : "\n" + GetExceptionMessage(ex.InnerException))}";
        }

        private void Log(Exception exception, LogLevel level, int statusCode, string message)
        {
            var eventId = new EventId(statusCode, exception.GetType().Name);

            if (level.In(LogLevel.Critical, LogLevel.Error))
            {
                // call with "exception" parameter will be logged in AppInsights as Exception
                _logger.Log(level, eventId, exception, message);
            }
            else
            {
                // call without "exception" parameter will be logged in AppInsights as Custom event
                _logger.Log(level, eventId, message);
            }
        }

        private async Task WriteToResponse(HttpContext ctx, int statusCode, ErrorResponse.ErrorModel error)
        {
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = statusCode;
            var errorResponse = new ErrorResponse { Errors = error.ToEnumerable() };
            await ctx.Response.WriteAsync(errorResponse.ToJson());
        }
    }
}
