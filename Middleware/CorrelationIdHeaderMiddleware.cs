public class CorrelationIdHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public CorrelationIdHeaderMiddleware(RequestDelegate next, ILogger<CorrelationIdHeaderMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public Task Invoke(HttpContext httpContext, ICorrelationProvider correlationProvider)
    {
        var correlationId = correlationProvider.SetCorrelationId(httpContext);
        using (_logger.BeginScope(new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("CorrelationId", correlationId ?? string.Empty),
        }))
        {
            return _next(httpContext);
        }
    }
}