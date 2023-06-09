using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CorreclationProvider;

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

    public string? SetCorrelationId(HttpContext httpContext)
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
        return _correlationId;
    }
}

public static class HttpCorrelationProviderExtensions
{
   public static IHostBuilder UseCorrelationProvider(this IHostBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException("builder");
        }
        builder.ConfigureServices(delegate (HostBuilderContext _, IServiceCollection collection)
        {
            collection.AddScoped<ICorrelationProvider, HttpCorrelationProvider>();;
        });
        return builder;
    }
}