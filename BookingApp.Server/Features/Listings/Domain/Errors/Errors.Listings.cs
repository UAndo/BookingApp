namespace BookingApp.Server.Features.Listings.Domain.Errors;

public static class Errors
{
    public static class Listings
    {
        public static Error NotFound => Error.NotFound(
            code: "Listing.NotFound",
            description: "The listing does not exist."
        );
    }
}