using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Listings.Domain.Listings;
using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Listings.GetListing
{
    public record GetListingResponse(ListingDto Listing);

    public class GetListingEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/listings/{id:guid}", async (Guid id, ISender sender) =>
                {
                    var query = new GetListingQuery(ListingId.Of(id));

                    var result = await sender.Send(query);

                    return result.ToOkResult<GetListingResult, GetListingResponse>();
                })
                .WithName("GetListing")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Gets a listing by ID")
                .WithDescription("Retrieves a listing by its unique identifier.")
                .WithTags("Listings");
            //.RequireRateLimiting("listings");
        }
    }
}
