public class HttpCorrelationProvider : ICorrelationProvider
{
    private readonly ILogger<HttpCorrelationProvider> _logger;
    private const string _headerKey = "x-correlation-id";
    private const string _propertyKey = "CorrelationId";

    private string? _correlationId;
    public HttpCorrelationProvider(ILogger<HttpCorrelationProvider> logger)
    {
        _logger = logger;
    }

    public string? GetCorrelationId()
    {
        return _correlationId;
    }

    public void SetCorrelationId(HttpContext httpContext)
    {
        string? correlationId = httpContext.Request.Headers.TryGetValue(_headerKey, out var values)
                      ? values.First()
                      : Guid.NewGuid().ToString();

        if (!httpContext.Items.ContainsKey(_propertyKey))
        {
            httpContext.Items.Add(_propertyKey, correlationId);
        }
        if (!httpContext.Response.Headers.ContainsKey(_headerKey))
        {
            httpContext.Response.Headers.Add(_headerKey, correlationId);
        }
        _correlationId = correlationId;
    }
}