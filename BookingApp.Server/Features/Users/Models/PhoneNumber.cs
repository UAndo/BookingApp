using PhoneNumbers;

namespace BookingApp.Server.Features.Users.Models
{
    public record PhoneNumber
    {
        public string Value { get; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static PhoneNumber Of(string input, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or empty", nameof(input));

            var util = PhoneNumberUtil.GetInstance();

            try
            {
                var phoneNumber = util.Parse(input, countryCode);

                if (!util.IsValidNumber(phoneNumber))
                    throw new ArgumentException("Invalid phone number format", nameof(input));

                var formattedNumber = util.Format(phoneNumber, PhoneNumberFormat.E164);

                return new PhoneNumber(formattedNumber);
            }
            catch (NumberParseException)
            {
                throw new ArgumentException("Invalid phone number format", nameof(input));
            }
        }

        public static PhoneNumber FromDb(string value) => new(value);
    }
}   
