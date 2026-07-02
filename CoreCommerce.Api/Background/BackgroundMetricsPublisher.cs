using CoreCommerce.Api.Options;
using CoreCommerce.Api.Services;
using Microsoft.Extensions.Options;

namespace CoreCommerce.Api.Background;

// This service will be registered as a Singleton
public class BackgroundMetricsPublisher(
    IServiceScopeFactory scopeFactory,
    IOptionsMonitor<OrderSettings> orderOptionsMonitor,
    ILogger<BackgroundMetricsPublisher> logger)
{
    public void PublishMetrics()
    {
        // IOptionsMonitor tracks changes dynamically via the .CurrentValue property
        var currentMerchant = orderOptionsMonitor.CurrentValue.MerchantName;
        var currentCurrency = orderOptionsMonitor.CurrentValue.DefaultCurrency;

        logger.LogInformation("Background task started. Creating local scope...");

        // 1. Manually create an isolated scope
        using IServiceScope scope = scopeFactory.CreateScope();

        // 2. Resolve the Scoped service safely FROM the new scope, NOT the root container
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderProcessingService>();

        // 3. Execute business logic
        var id = orderService.GetServiceInstanceId();
        logger.LogInformation("Successfully resolved Scoped Service {Id} inside Singleton.", id);

        // 4. When the method ends, the 'using' block disposes the scope, 
        // safely destroying the Scoped service and any attached database connections.
    }
}