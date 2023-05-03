using Microsoft.AspNetCore.Http;

namespace CorreclationProvider;

public interface ICorrelationProvider {
    string? GetCorrelationId();
    string? SetCorrelationId(HttpContext httpContext);
}