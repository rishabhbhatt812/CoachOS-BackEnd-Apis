using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoachOS.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var response = ApiResponse<object>.Fail(exception.Message);

            switch (exception)
            {
                case CoachOS.Shared.Exceptions.ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    response = ApiResponse<object>.Fail("Validation Failed");
                    foreach (var error in validationEx.Errors)
                    {
                        foreach (var message in error.Value)
                        {
                            response.Errors.Add($"{error.Key}: {message}");
                        }
                    }
                    break;
                case CoachOS.Shared.Exceptions.NotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case CoachOS.Shared.Exceptions.UnauthorizedException authEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response = ApiResponse<object>.Fail("Internal Server Error");
                    response.Errors.Add(exception.Message); // For production, you might want to hide the raw message
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
