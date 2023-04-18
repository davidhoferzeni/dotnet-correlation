using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

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

public static class CorrelationIdHeaderMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestCorrealtion(
        this IApplicationBuilder builder)
    {
        builder.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var logger = builder.ApplicationServices.GetRequiredService<ILogger<CorrelationIdHeaderMiddleware>>();
                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionHandlerPathFeature?.Error is InvalidOperationException &&
                exceptionHandlerPathFeature.Error.Message.Contains(nameof(ICorrelationProvider)))
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = Text.Plain;
                    var criticalMessage = "You are using the Correlation Provider library to track your requests but have not correctly provided the service.";
                    logger.LogCritical(criticalMessage);
                    await context.Response.WriteAsync("Error during application configuration. Please contact the developers.");
                }
            });
        });

        return builder.UseMiddleware<CorrelationIdHeaderMiddleware>();
    }
}