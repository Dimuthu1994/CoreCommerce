using CoreCommerce.Api.Options;
using CoreCommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreCommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderProcessingService orderProcessingService, 
    ITimeService timeService, IOptionsSnapshot<OrderSettings> orderOptions) : ControllerBase
{
    [HttpGet]
    public IActionResult GetOrders()
    {
        return Ok(new[] { "Order #1001", "Order #1002", "Order #1003" });
    }

    [HttpGet("error-test")]
    public IActionResult SimulateError()
    {
        // This unhandled error will trigger our GlobalExceptionMiddleware!
        throw new InvalidOperationException("Simulated database failure.");
    }

    [HttpPost("{orderId}/process")]
    public IActionResult Process(string orderId, [FromQuery] int itemCount)
    {
        OrderSettings settings = orderOptions.Value;

        // Applying configuration properties to drive business constraints
        if (itemCount > settings.MaxItemsPerOrder)
        {
            return BadRequest($"Order exceeds the maximum allowed quantity of {settings.MaxItemsPerOrder} items.");
        }

        var result = orderProcessingService.ProcessOrder(orderId);

        return Ok(new
        {
            Message = result,
            Currency = settings.DefaultCurrency,
            ProcessedAt = timeService.GetCurrentTime()
        });
    }

    [HttpGet("settings")]
    public IActionResult GetCurrentSettings()
    {
        // Extract the actual data class using the .Value property
        OrderSettings settings = orderOptions.Value;

        return Ok(new
        {
            Merchant = settings.MerchantName,
            AllowedMaxItems = settings.MaxItemsPerOrder,
            ShippingInternational = settings.EnableInternationalShipping,
            CurrencySystem = settings.DefaultCurrency
        });
    }


}