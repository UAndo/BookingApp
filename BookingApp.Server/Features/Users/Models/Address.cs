namespace BookingApp.Server.Features.Users.Models
{
    public record Address
    {
        public string AddressLine { get; init; } = default!;
        public string Country { get; init; } = default!;
        public string City { get; init; } = default!;

        private Address(string addressLine, string country, string city)
        {
            AddressLine = addressLine;
            Country = country;
            City = city;
        }

        public static Address Of(string addressLine, string country, string city)
        {
            return new Address(addressLine, country, city);
        }
    }
}
