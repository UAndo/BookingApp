using NpgsqlTypes;

namespace BookingApp.Server.Features.Bookings.Domain.Bookings
{
    public record DateRange
    {
        public DateTimeOffset From { get; }
        public DateTimeOffset To { get; }

        public NpgsqlRange<DateTimeOffset> ToNpgsqlRange()
        {
            return new NpgsqlRange<DateTimeOffset>(From.UtcDateTime, true, To.UtcDateTime, false);
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

        public static DateRange FromNpgsqlRange(NpgsqlRange<DateTimeOffset> range)
        {
            if (range.IsEmpty)
                throw new ArgumentException("Range cannot be empty.");

            return new DateRange(new DateTimeOffset(range.LowerBound.UtcDateTime), new DateTimeOffset(range.UpperBound.UtcDateTime));
        }

        public bool Overlaps(DateRange other)
        {
            return other.From <= To && From <= other.To;
        }
    }
}