namespace StockApp.Api.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    public const string HeaderName = "X-Correlation-Id";

    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var correlationId = GetOrCreateCorrelationId(context);

        context.TraceIdentifier = correlationId;

        context.Response.Headers[HeaderName] = correlationId;

        await _next(context);
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(
                HeaderName,
                out var correlationId))
        {
            return correlationId!;
        }

        return Guid.NewGuid().ToString();
    }
}