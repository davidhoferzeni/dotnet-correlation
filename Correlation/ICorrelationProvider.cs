public interface ICorrelationProvider {
    string? GetCorrelationId();
    void SetCorrelationId(HttpContext httpContext);
}