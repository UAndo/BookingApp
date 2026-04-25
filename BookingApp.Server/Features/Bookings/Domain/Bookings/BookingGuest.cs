using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Bookings.Domain.Bookings
{
    public record BookingGuest
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public Address Address { get; private set; } = default!;
        public PhoneNumber PhoneNumber { get; private set; } = default!;
        public string Nationality { get; private set; } = default!;
        public DateTimeOffset? ArrivingTime { get; private set; } = default!;

        private BookingGuest() { }

        private BookingGuest(string firstName, string lastName, Email email, 
            Address address, PhoneNumber phoneNumber, string nationality, DateTimeOffset? arrivingTime)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
            PhoneNumber = phoneNumber;
            Nationality = nationality;
            ArrivingTime = arrivingTime;
        }

        public static BookingGuest Of(string firstName, string lastName, Email email,
            Address address, PhoneNumber phoneNumber, string nationality, DateTimeOffset? arrivingTime)
        {
            ArgumentNullException.ThrowIfNull(firstName);
            ArgumentNullException.ThrowIfNull(lastName);
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(address);
            ArgumentNullException.ThrowIfNull(phoneNumber);
            ArgumentNullException.ThrowIfNull(nationality);

            return new BookingGuest(
                firstName,
                lastName,
                email,
                address,
                phoneNumber,
                nationality,
                arrivingTime);
        }
    }
}
