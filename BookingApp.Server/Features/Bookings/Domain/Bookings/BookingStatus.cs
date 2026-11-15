namespace BookingApp.Server.Features.Bookings.Domain.Bookings
{
    public enum BookingStatus
    {
        Draft = 0,
        PendingPayment = 1,
        Confirmed = 2,
        Cancelled = 3,
        Expired = 4
    }
}
