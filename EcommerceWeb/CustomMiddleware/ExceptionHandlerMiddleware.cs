using Ecommerce.Service.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.CustomMiddleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandleNotFoundEndPoint(context);
                }
            }
            catch (Exception ex)
            {
                // logging
                _logger.LogError(ex.Message);
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Response already started; cannot write error response.");
                    throw;
                }
                context.Response.ContentType = "application/problem+json";
                var problem = new ProblemDetails()
                {
                    Title = "Error occured while executing the endpoint",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    Status = ex switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    }
                };
                context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problem);
            }
        }

        private static async Task HandleNotFoundEndPoint(HttpContext context)
        {
            var problem = new ProblemDetails()
            {
                Title = "Error while processing http request - endpoint not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The requested endpoint {context.Request.Path} was not found.",
                Instance = context.Request.Path
            };
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
