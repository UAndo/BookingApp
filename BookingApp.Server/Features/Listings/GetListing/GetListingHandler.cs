using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Listings.Domain.Errors;
using BookingApp.Server.Features.Listings.Domain.Listings;

namespace BookingApp.Server.Features.Listings.GetListing
{
    public record GetListingQuery(ListingId ListingId) : IQuery<GetListingResult>;

    public record GetListingResult(ListingDto Listing);
    
    public record ListingDto(
        Guid Id,
        string Title,
        string Description,
        decimal PricePerNight,
        int MaxGuests,
        ListingStatus Status);

    public class GetListingQueryHandler(ApplicationDbContext context) : IQueryHandler<GetListingQuery, GetListingResult>
    {
        public async Task<ErrorOr<GetListingResult>> Handle(GetListingQuery query, CancellationToken cancellationToken)
        {
            var existingListing = await context.Listings
                .AsNoTracking()
                .Where(x => x.Id == query.ListingId)
                .Select(x => new ListingDto(
                    x.Id.Value,
                    x.Title,
                    x.Description,
                    x.PricePerNight,
                    x.MaxGuests,
                    x.Status))
                .SingleOrDefaultAsync(cancellationToken);

            if (existingListing is null)
                return Errors.Listings.NotFound;

            return new GetListingResult(existingListing);
        }
    }
}
