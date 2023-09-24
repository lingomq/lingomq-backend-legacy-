namespace Authentication.Api.Middlewares
{
    public class ExceptionsHandlerMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        public ExceptionsHandlerMiddleware(ILogger logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContent context, Exception exception)
        {

        }
    }
}
