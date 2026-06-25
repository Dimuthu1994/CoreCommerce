using CoreCommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(
    [FromKeyedServices("Stripe")] IPaymentService stripeService,
    [FromKeyedServices("PayPal")] IPaymentService paypalService) : ControllerBase
{
    [HttpPost("stripe")]
    public IActionResult PayWithStripe([FromQuery] decimal amount)
    {
        return Ok(new { Result = stripeService.ProcessPayment(amount) });
    }

    [HttpPost("paypal")]
    public IActionResult PayWithPayPal([FromQuery] decimal amount)
    {
        return Ok(new { Result = paypalService.ProcessPayment(amount) });
    }
}