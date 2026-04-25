using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Bookings.ConifrmBooking
{
    public record ConifrmBookingRequest(
        Guid BookingId);

    public class ConifrmBookingEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/bookings/confirm", async (ConifrmBookingRequest request, ISender sender) =>
            {
                var command = request.Adapt<ConifrmBookingCommand>();

                var result = await sender.Send(command);

                return result.ToNoContentResult();
            });
        }
    }
}
