using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Bookings.Domain.Bookings;
using BookingApp.Server.Features.Bookings.Domain.Errors;

namespace BookingApp.Server.Features.Bookings.CompleteGuestDetails
{
    public record CompleteGuestDetailsCommand(
        BookingId BookingId,
        string FirstName,
        string LastName,
        string Email,
        string AddressLine,
        string Country,
        string City,
        string PhoneNumber,
        string CountryCode,
        string Nationality,
        DateTimeOffset? ArrivingTime) : ICommand;

    public class CompleteGuestDetailsCommandValidator : AbstractValidator<CompleteGuestDetailsCommand>
    {
        public CompleteGuestDetailsCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.AddressLine)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x)
                .Must(x => BeValidPhoneNumber(x.PhoneNumber, x.CountryCode))
                .WithMessage("Invalid phone number format.");
        }

        private static bool BeValidPhoneNumber(string? phoneNumber, string? countryCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber)) return true;
                if (string.IsNullOrWhiteSpace(countryCode)) return false;

                PhoneNumber.Of(phoneNumber, countryCode);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class CompleteGuestDetailsCommandHandler(
        ApplicationDbContext context,
        TimeProvider timeProvider) : ICommandHandler<CompleteGuestDetailsCommand>
    {
        public async Task<ErrorOr<Unit>> Handle(CompleteGuestDetailsCommand command, CancellationToken cancellationToken)
        {
            var booking = await context.Bookings
                .FirstOrDefaultAsync(b => b.Id == command.BookingId, cancellationToken);

            if (booking == null)
                return Errors.Booking.NotFound;

            if (booking.ExpiresAt < timeProvider.GetUtcNow())
                return Errors.Booking.Expired;

            var email = Email.Of(command.Email);

            var address = Address.Of(
                command.AddressLine,
                command.City,
                command.Country);

            var phoneNumber = PhoneNumber.Of(
                command.PhoneNumber,
                command.CountryCode);

            var bookingGuest = BookingGuest.Of(
                command.FirstName,
                command.LastName,
                email,
                address,
                phoneNumber,
                command.Nationality,
                command.ArrivingTime);

            booking.SetMainGuestDetails(bookingGuest, timeProvider);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
