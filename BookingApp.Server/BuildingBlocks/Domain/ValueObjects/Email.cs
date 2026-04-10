namespace BookingApp.Server.BuildingBlocks.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Of(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email is invalid.", nameof(value));

            var normalized = value.Trim().ToLowerInvariant();

            if (!IsValid(normalized))
                throw new ArgumentException("Email is invalid.", nameof(value));

            return new Email(normalized);
        }

        public static bool IsValid(string value)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(value);
                return addr.Address == value;
            }
            catch
            {
                return false;
            }
        }
    }
}
