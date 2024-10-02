using System.Net;
using System.Text.Json;
using Reactivities.Application.Core;

namespace Reactivities.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment hostEnvironment
            )
        {
            this._next = next;
            this._logger = logger;
            this._hostEnvironment = hostEnvironment;
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

                AppException response;
                if (_hostEnvironment.IsDevelopment())
                {
                    response = new AppException(
                        context.Response.StatusCode, ex.Message, ex.StackTrace.ToString()
                    );
                }
                else
                {
                    response = new AppException(context.Response.StatusCode, "Internal server error");
                }

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonResponse = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
