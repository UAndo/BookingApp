using BookingApp.Server.BuildingBlocks.Extensions;
using BookingApp.Server.Infrastructure.Web;
using System.Security.Claims;

namespace BookingApp.Server.Features.Listings.UpdateListing
{
    public record UpdateListingRequest(
        string Title,
        string Description,
        decimal PricePerNight,
        int MaxGuests,
        string AddressLine,
        string Country,
        string City);

    public record UpdateListingResponse(Guid ListingId);

    public class UpdateListingEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/listings/{id:guid}", async (Guid id, UpdateListingRequest request, ClaimsPrincipal user, ISender sender) =>
            {
                var command = request.Adapt<UpdateListingCommand>() with { ListingId = id, OwnerId = user.GetUserId() };

                var result = await sender.Send(command);

                return result.ToCreatedResult<UpdateListingResult, UpdateListingResponse>(
                    r => $"/api/listings/{r.ListingId}");
            })
            .WithName("UpdateListing")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Updates an existing listing")
            .WithDescription("Updates the details of an existing listing with the provided information.")
            .WithTags("Listings")
            //.RequireRateLimiting("listings")
            .RequireAuthorization();
        }
    }
}
