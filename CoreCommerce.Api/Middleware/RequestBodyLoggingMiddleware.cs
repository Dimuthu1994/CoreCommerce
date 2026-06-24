using System.Text;

namespace CoreCommerce.Api.Middleware;

public class RequestBodyLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestBodyLoggingMiddleware> _logger;

    public RequestBodyLoggingMiddleware(RequestDelegate next, ILogger<RequestBodyLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Advanced Exercise: Enable multi-read buffering on the stream
        context.Request.EnableBuffering();

        // LeaveOpen: true is mandatory, otherwise the StreamReader closes the network stream!
        using (var reader = new StreamReader(
                   context.Request.Body,
                   encoding: Encoding.UTF8,
                   detectEncodingFromByteOrderMarks: false,
                   bufferSize: 1024,
                   leaveOpen: true))
        {
            var body = await reader.ReadToEndAsync();

            if (!string.IsNullOrWhiteSpace(body))
            {
                _logger.LogInformation("Incoming Request Body Payload: {Body}", body);
            }
        }

        // Rewind the stream to the absolute beginning so the Controllers can read it normally
        context.Request.Body.Position = 0;

        await _next(context);
    }
}

public static class RequestBodyLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestBodyLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestBodyLoggingMiddleware>();
    }
}