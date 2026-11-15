using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Listings.Domain.Errors;

namespace BookingApp.Server.Features.Listings.UpdateListing
{
    public record UpdateListingCommand(
        Guid ListingId,
        string Title,
        string Description,
        decimal PricePerNight,
        int MaxGuests,
        UserId OwnerId,
        string AddressLine,
        string Country,
        string City) : ICommand<UpdateListingResult>;

    public record UpdateListingResult(ListingId ListingId);

    public class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
    {
        public UpdateListingCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.PricePerNight)
                .GreaterThan(0);

            RuleFor(x => x.MaxGuests)
                .GreaterThan(0);

            RuleFor(x => x.OwnerId)
                .NotEmpty();

            RuleFor(x => x.AddressLine)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(100);
        }
    }

    public class UpdateListingCommandHandler(ApplicationDbContext context) : ICommandHandler<UpdateListingCommand, UpdateListingResult>
    {
        public async Task<ErrorOr<UpdateListingResult>> Handle(UpdateListingCommand command, CancellationToken cancellationToken)
        {
            var existingListing = await context.Listings.FirstOrDefaultAsync(x => x.Id.Value == command.ListingId, cancellationToken);

            if (existingListing is null || existingListing.OwnerId != command.OwnerId)
                return Errors.Listings.NotFound;

            var address = Address.Of(command.AddressLine, command.Country, command.City);

            existingListing.Update(command.Title, command.Description, command.PricePerNight, command.MaxGuests, address);

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateListingResult(existingListing.Id);
        }
    }
}
