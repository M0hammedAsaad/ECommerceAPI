using ECommerce.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ECommerce.Api.Middlewares
{
    public class GlobalExeptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        public GlobalExeptionHandlingMiddleware(RequestDelegate request, ILoggerManager logger)
        {
            _next = request;
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
                _logger.LogError($"Something went wrong: {ex}");

                var response = context.Response;

                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "Internal Server Error"
                };

                var json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);

            }
        }
    }
}
