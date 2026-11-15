using BookingApp.Server.BuildingBlocks.Extensions;
using BookingApp.Server.Infrastructure.Web;
using System.Security.Claims;

namespace BookingApp.Server.Features.Listings.CreateListing
{
    public record CreateListingRequest(
        string Title,
        string Description,
        decimal PricePerNight,
        int MaxGuests,
        string AddressLine,
        string Country,
        string City);

    public record CreateListingResponse(Guid ListingId);

    public class CreateListingEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/listings", async (CreateListingRequest request, ClaimsPrincipal user, ISender sender) =>
            {
                var command = request.Adapt<CreateListingCommand>() with { OwnerId = user.GetUserId() };

                var result = await sender.Send(command);

                return result.ToHttpResult(value =>
                {
                    var response = new CreateListingResponse(value.ListingId.Value);

                    return TypedResults.CreatedAtRoute(
                        response,
                        routeName: "GetListing",
                        routeValues: new { id = response.ListingId });
                });
            })
            .WithName("CreateListing")  
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new listing")
            .WithDescription("Creates a new listing with the provided details.")
            .WithTags("Listings")
            //.RequireRateLimiting("listings")
            .RequireAuthorization();
        } 
    }
}
