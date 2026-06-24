using Microsoft.Extensions.Primitives;

namespace CoreCommerce.Api.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeaderKey = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Easy Exercise: Check if incoming request has the header. If not, generate it.
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out StringValues correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers.Append(CorrelationIdHeaderKey, correlationId);
        }

        // Medium Exercise: Add the Correlation ID to the Response headers 
        // so the client can read it back for support triage.
        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey(CorrelationIdHeaderKey))
            {
                context.Response.Headers.Append(CorrelationIdHeaderKey, correlationId);
            }
            return Task.CompletedTask;
        });

        await _next(context);
    }
}

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationIdMiddleware>();
    }
}