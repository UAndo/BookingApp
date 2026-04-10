namespace BookingApp.Server.BuildingBlocks.Domain.ValueObjects
{
    public record Address
    {
        public string AddressLine { get; }
        public string Country { get; }
        public string City { get; }

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
