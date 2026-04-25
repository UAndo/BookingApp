using NpgsqlTypes;

namespace BookingApp.Server.Features.Bookings.Domain.Bookings
{
    public record DateRange
    {
        public DateTimeOffset From { get; }
        public DateTimeOffset To { get; }

        public NpgsqlRange<DateTime> ToNpgsqlRange()
        {
            return new NpgsqlRange<DateTime>(From.UtcDateTime, true, To.UtcDateTime, false);
        }

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

        public static DateRange FromNpgsqlRange(NpgsqlRange<DateTime> range)
        {
            if (range.IsEmpty)
                throw new ArgumentException("Range cannot be empty.");

            return new DateRange(new DateTimeOffset(range.LowerBound), new DateTimeOffset(range.UpperBound));
        }
    }
}