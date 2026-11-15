using BookingApp.Server.Features.Bookings.Domain.Bookings;
using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Bookings.CompleteGuestDetails
{
    public record CompleteGuestDetailsRequest(
        Guid BookingId,
        string FirstName,
        string LastName,
        string Email,
        string AddressLine,
        string Country,
        string City,
        string PhoneNumber,
        string CountryCode,
        string Nationality,
        DateTimeOffset? ArrivingTime);

    public class CompleteGuestDetailsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/bookings/complete-guest-details",
                async (CompleteGuestDetailsRequest request, ISender sender) =>
                {
                    var command = new CompleteGuestDetailsCommand(
                        BookingId.Of(request.BookingId),
                        request.FirstName,
                        request.LastName,
                        request.Email,
                        request.AddressLine,
                        request.Country,
                        request.City,
                        request.PhoneNumber,
                        request.CountryCode,
                        request.Nationality,
                        request.ArrivingTime);
                    
                    var result = await sender.Send(command);

                    return result.ToNoContentResult();
                });
        }
    }
}