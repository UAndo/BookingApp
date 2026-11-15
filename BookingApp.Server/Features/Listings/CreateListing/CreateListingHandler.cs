using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Listings.Domain.Listings;

namespace BookingApp.Server.Features.Listings.CreateListing
{
    public record CreateListingCommand(
        string Title,
        string Description,
        decimal PricePerNight,
        int MaxGuests,
        UserId OwnerId,
        string AddressLine,
        string Country,
        string City) : ICommand<CreateListingResult>;

    public record CreateListingResult(ListingId ListingId);

    public class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
    {
        public CreateListingCommandValidator()
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

    public class CreateListingCommandHandler(ApplicationDbContext context) : ICommandHandler<CreateListingCommand, CreateListingResult>
    {
        public async Task<ErrorOr<CreateListingResult>> Handle(CreateListingCommand command, CancellationToken cancellationToken)
        {
            var address = Address.Of(command.AddressLine, command.Country, command.City);

            var listing = Listing.Create(
                command.Title,
                command.Description,
                command.PricePerNight,
                command.MaxGuests,
                command.OwnerId,
                address);

            context.Listings.Add(listing);

            await context.SaveChangesAsync(cancellationToken);

            return new CreateListingResult(listing.Id);
        }
    }
}
