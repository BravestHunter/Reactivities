using System.Net;
using System.Text.Json;
using Reactivities.Api.Models;

namespace Reactivities.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            IHostEnvironment hostEnvironment,
            ILogger<ExceptionMiddleware> logger
            )
        {
            _next = next;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ServerErrorResponse response;
                if (_hostEnvironment.IsDevelopment())
                {
                    response = new ServerErrorResponse()
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = ex.Message,
                        Details = ex.StackTrace?.ToString()
                    };
                }
                else
                {
                    response = new ServerErrorResponse()
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = "Internal server error"
                    };
                }

                var jsonResponse = JsonSerializer.Serialize(response, JsonSerializerOptions);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
