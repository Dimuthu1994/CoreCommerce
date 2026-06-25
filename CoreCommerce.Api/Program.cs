using CoreCommerce.Api.Background;
using CoreCommerce.Api.Middleware;
using CoreCommerce.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. SERVICES CONFIGURATION (Dependency Injection Container)
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// DEPENDENCY INJECTION REGISTRATIONS
// ==========================================
// We map the Interface to the Implementation and declare its lifetime
builder.Services.AddScoped<IOrderProcessingService, OrderProcessingService>();
builder.Services.AddTransient<ITimeService, TimeService>();
// KEYED SERVICES REGISTRATIONS
builder.Services.AddKeyedScoped<IPaymentService, StripePaymentService>("Stripe");
builder.Services.AddKeyedScoped<IPaymentService, PayPalPaymentService>("PayPal");

builder.Services.AddSingleton<BackgroundMetricsPublisher>();

var app = builder.Build();

// ==========================================
// 2. HTTP REQUEST PIPELINE CONFIGURATION (Strict Order!)
// ==========================================

// Catch exceptions from ALL middleware below this line
app.UseGlobalExceptionHandler();

// Assign the correlation tracking identity to the request context
app.UseCorrelationId();

// Track processing duration of ALL steps below this line
app.UseRequestTiming();

// Safely buffer and log payloads if needed
app.UseRequestBodyLogging();

// http://localhost:5197/swagger/index.html
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Resolve the singleton manually just to test it on startup
var metricsPublisher = app.Services.GetRequiredService<BackgroundMetricsPublisher>();
metricsPublisher.PublishMetrics();

app.Run();
