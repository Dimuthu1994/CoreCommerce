namespace CoreCommerce.Api.Services;

// C# 12 Primary Constructor: (ILogger<OrderProcessingService> logger) replaces the standard constructor
public class OrderProcessingService(ILogger<OrderProcessingService> logger) : IOrderProcessingService
{
    // A unique ID to prove when this object was created
    private readonly Guid _instanceId = Guid.NewGuid();

    public Guid GetServiceInstanceId() => _instanceId;

    public string ProcessOrder(string orderId)
    {
        logger.LogInformation("Processing order {OrderId} using Service Instance {InstanceId}", orderId, _instanceId);
        return $"Order {orderId} processed successfully!";
    }
}