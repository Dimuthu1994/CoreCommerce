using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
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
}