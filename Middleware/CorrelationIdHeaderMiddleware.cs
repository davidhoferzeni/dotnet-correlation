public class CorrelationIdHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext, ICorrelationProvider correlationProvider)
    {
        correlationProvider.SetCorrelationId(httpContext);
        return _next(httpContext);
    }
}