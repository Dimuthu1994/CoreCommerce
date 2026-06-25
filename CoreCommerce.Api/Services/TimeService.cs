namespace CoreCommerce.Api.Services
{
    public class TimeService : ITimeService
    {
        // Returns UTC time, which is a strict best practice for backend systems
        public DateTime GetCurrentTime() => DateTime.UtcNow;
    }
}
