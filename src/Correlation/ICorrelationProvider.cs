public interface ICorrelationProvider {
    string? GetCorrelationId();
    string? SetCorrelationId(HttpContext httpContext);
}