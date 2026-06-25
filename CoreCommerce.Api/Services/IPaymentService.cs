namespace CoreCommerce.Api.Services;

public interface IPaymentService
{
    string ProcessPayment(decimal amount);
}

public class StripePaymentService : IPaymentService
{
    public string ProcessPayment(decimal amount) => $"Successfully charged ${amount} via Stripe API.";
}

public class PayPalPaymentService : IPaymentService
{
    public string ProcessPayment(decimal amount) => $"Successfully charged ${amount} via PayPal API.";
}