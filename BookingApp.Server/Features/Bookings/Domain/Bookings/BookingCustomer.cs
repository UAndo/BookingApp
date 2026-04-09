using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Bookings.Domain.Bookings
{
    public record BookingCustomer
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public Address Address { get; private set; } = default!;
        public PhoneNumber PhoneNumber { get; private set; } = default!;
        public string Nationality { get; private set; } = default!;
        public DateTimeOffset? ArrivingTime { get; private set; } = default!;
    }
}
