using BookingApp.Server.Features.Listings.Domain.Listings;
using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Listings.GetAllListings
{
    public record GetAllListingsRequest(int? PageNumber = 1, int? PageSize = 10);
    public record GetAllListingsResponse(
        List<Listing> Listings,
        int PageNumber,
        int TotalPages,
        int TotalCount,
        bool HasNextPage
    );

    public class GetAllListingsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/listings/",
                    async ([AsParameters] GetAllListingsRequest request, ISender sender) =>
                    {
                        var query = request.Adapt<GetAllListingsQuery>();

                        var result = await sender.Send(query);

                        return result.ToOkResult<GetAllListingsResult, GetAllListingsResponse>();
                    })
                .WithName("GetAllListings")
                .Produces<GetAllListingsResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Gets all listings with pagination")
                .WithDescription("Retrieves the details of all listings with pagination support.")
                .WithTags("Listings");
            //.RequireRateLimiting("listings");
        }
    }
}
