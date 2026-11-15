using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Listings.Domain.Errors;
using BookingApp.Server.Features.Listings.Domain.Listings;

namespace BookingApp.Server.Features.Listings.DeleteListing
{
    public record DeleteListingCommand(
        Guid ListingId,
        UserId OwnerId) : ICommand;

    public class DeleteListingCommandHandler(ApplicationDbContext context) : ICommandHandler<DeleteListingCommand>
    {
        public async Task<ErrorOr<Unit>> Handle(DeleteListingCommand command, CancellationToken cancellationToken)
        {
            var existingListing = await context.Listings.FirstOrDefaultAsync(x => x.Id.Value == command.ListingId, cancellationToken);

            if (existingListing is null || existingListing.OwnerId != command.OwnerId)
                return Errors.Listings.NotFound;

            existingListing.ChangeStatus(ListingStatus.Archived);

            context.Listings.Update(existingListing);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
