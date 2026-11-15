using BookingApp.Server.BuildingBlocks.Extensions;
using BookingApp.Server.Features.Bookings.Domain.Bookings;
using BookingApp.Server.Infrastructure.Web;
using System.Security.Claims;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Bookings.CreateBooking;

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
        app.MapPost("/bookings/draft", async (CreateBookingRequest request, ClaimsPrincipal user, ISender sender) =>
            {
                var command = new CreateBookingCommand(
                    request.Guests,
                    request.CheckIn,
                    request.CheckOut,
                    ListingId.Of(request.ListingId),
                    user.GetUserId());
                
                var result = await sender.Send(command);

                return result.ToHttpResult(value =>
                {
                    var response = new CreateBookingResponse(value.BookingId.Value);

                    return TypedResults.CreatedAtRoute(
                        response,
                        routeName: "GetBooking",
                        routeValues: new { id = response.BookingId });
                });
            })
            .WithName("CreateBooking")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new booking")
            .WithDescription("Creates a new booking for the specified listing.")
            .WithTags("Bookings")
            .RequireAuthorization();
    }
}