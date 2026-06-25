using CoreCommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderProcessingService orderProcessingService, ITimeService timeService) : ControllerBase
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
    public IActionResult Process(string orderId)
    {
        // We don't care HOW orderProcessingService was created. We just use it.
        var result = orderProcessingService.ProcessOrder(orderId);

        return Ok(new
        {
            Message = result,
            ServiceId = orderProcessingService.GetServiceInstanceId(),
            ProcessedAt = timeService.GetCurrentTime()
        });
    }

}