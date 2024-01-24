public class RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();

    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation(
            $"Handling request: {context.Request.Method} {context.Request.Path}"
        );
        try
        {
            await next(context); // Call the next delegate/middleware in the pipeline
        }
        finally
        {
            _logger.LogInformation(
                $"Finished handling request. Status Code: {context.Response.StatusCode}"
            );
        }
    }
}