namespace BookingApp.Server.Features.Bookings.Domain.Bookings
{
    public record DateRange
    {
        public DateTimeOffset From { get; }
        public DateTimeOffset To { get; }

        private DateRange(DateTimeOffset from, DateTimeOffset to)
        {
            From = from;
            To = to;
        }

        public static DateRange Of(DateTimeOffset from, DateTimeOffset to)
        {
            if (from >= to)
                throw new ArgumentException("From must be before To.");

            return new DateRange(from, to);
        }
    }
}