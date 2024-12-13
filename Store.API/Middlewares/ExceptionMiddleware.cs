using Microsoft.AspNetCore.Http;
using Store.API.Errors;
using System.Text.Json;

namespace Store.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment environment;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                next.Invoke(context);
            } 
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = environment.IsDevelopment() ?
                    new ApiExceptionResponse(StatusCodes.Status500InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiExceptionResponse(StatusCodes.Status500InternalServerError);

               var json= JsonSerializer.Serialize(response);

               await context.Response.WriteAsync(json);
            }
        }
    }
}
