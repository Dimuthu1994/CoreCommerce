using System.Diagnostics;

namespace CoreCommerce.Api.Middleware;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Pass the request to the next component in the pipeline
        await _next(context);

        stopwatch.Stop();

        // High-performance structured logging
        _logger.LogInformation(
            "HTTP {RequestMethod} {RequestPath} completed in {ElapsedMilliseconds}ms",
            context.Request.Method,
            context.Request.Path,
            stopwatch.ElapsedMilliseconds);
    }
}

public static class RequestTimingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTimingMiddleware>();
    }
}