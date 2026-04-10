namespace BookingApp.Server.Features.Bookings.Domain.Errors;

public static class Errors
{
    public static class Booking
    {
        public static Error AlreadyExists => Error.Conflict(
            code: "Booking.AlreadyExists",
            description: "A booking for the same dates already exists."
        );

        public static Error Conflict => Error.Conflict(
            code: "Booking.Conflict",
            description: "The booking conflicts with an existing booking."
        );

        public static Error NotFound => Error.NotFound(
            code: "Booking.NotFound",
            description: "The booking does not exist."
        );

        public static Error Expired => Error.Validation(
            code: "Booking.Expired",
            description: "The booking has expired."
        );
    }
}