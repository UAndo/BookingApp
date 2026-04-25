using BookingApp.Server.BuildingBlocks.Extensions;
using BookingApp.Server.Features.Bookings.Domain.Bookings;
using BookingApp.Server.Infrastructure.Web;
using System.Security.Claims;

namespace BookingApp.Server.Features.Bookings.CreateBooking
{
    public record CreateBookingRequest(
        List<Guest> Guests,
        DateTimeOffset CheckIn,
        DateTimeOffset CheckOut,
        Guid ListingId);

    public record CreateBookingResponse(Guid BookingId);

    public class CreateBookingEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/bookings", async (CreateBookingRequest request, ClaimsPrincipal user, ISender sender) =>
            {
                var command = request.Adapt<CreateBookingCommand>() with { UserId = user.GetUserId() };
                var result = await sender.Send(command);

                return result.ToCreatedResult<CreateBookingResult, CreateBookingResponse>(
                    r => $"/api/bookings/{r.BookingId}");
            });
        }
    }
}
