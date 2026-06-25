namespace CoreCommerce.Api.Services
{
    public interface IOrderProcessingService
    {
        Guid GetServiceInstanceId();
        string ProcessOrder(string orderId);
    }
}
