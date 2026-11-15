using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Pagination;
using BookingApp.Server.Features.Listings.Domain.Listings;

namespace BookingApp.Server.Features.Listings.GetAllListings
{
    public record GetAllListingsQuery(int? PageNumber = 1, int? PageSize = 10)
        : IQuery<GetAllListingsResult>;

    public record GetAllListingsResult(
        List<Listing> Listings,
        int PageNumber,
        int TotalPages,
        int TotalCount,
        bool HasNextPage
    );

    public class GetAllListingsHandler(ApplicationDbContext dbContext) : IQueryHandler<GetAllListingsQuery, GetAllListingsResult>
    {
        public async Task<ErrorOr<GetAllListingsResult>> Handle(GetAllListingsQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber ?? 1;
            int pageSize = request.PageSize ?? 10;

            var query = dbContext.Listings
                .AsNoTracking()
                .Where(l => l.Status == ListingStatus.Active)
                .OrderBy(l => l.Title)
                .Select(l => l);

            var pagedListings = await PagedList<Listing>.CreateAsync(query, pageNumber, pageSize, cancellationToken);

            return new GetAllListingsResult(
                pagedListings.Items,
                pagedListings.PageNumber,
                pagedListings.TotalPages,
                pagedListings.TotalCount,
                pagedListings.HasNextPage);
        }
    }
}
