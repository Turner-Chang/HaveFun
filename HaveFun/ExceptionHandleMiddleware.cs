using Newtonsoft.Json;

namespace HaveFun
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> logger;

        public ExceptionHandleMiddleware(RequestDelegate next,ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        // IMessageWriter is injected into InvokeAsync
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var errorString = JsonConvert.SerializeObject(
                new
                {
                    ErrorMessage = exception.Message,
                    InnerError = exception.InnerException,
                });
            logger.LogError(errorString);
            return context.Response.WriteAsync(errorString);

        }
    }
}
