using BookingApp.Server.Features.Users.Models;
using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Bookings.GetBooking
{
    public record GetBookingResponse(BookingDto Booking);

    public class GetBookingEndpoint : ICarterModule 
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/bookings/{id:guid}", async (
                Guid id,
                ISender sender) =>
            {
                var query = new GetBookingQuery(id);
                var result = await sender.Send(query);
                return result.ToOkResult<GetBookingResult, GetBookingResponse>();
            })
            .WithName("GetBooking")
            .Produces<GetBookingResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Gets a booking by its ID.")
            .WithDescription("Retrieves the details of a booking with the specified ID.")
            .WithTags("Bookings")   
            //.RequireRateLimiting("Bookings")
            .RequireAuthorization();
        }
    }
}
