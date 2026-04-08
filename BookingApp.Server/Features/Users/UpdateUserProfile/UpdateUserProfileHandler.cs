using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Users.Common;
using BookingApp.Server.Features.Users.Models;

namespace BookingApp.Server.Features.Users.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        UserId UserId,
        string? FirstName,
        string? LastName,
        string? AddressLine,
        string? City,
        string? Country,
        string? PhoneNumber,
        string? CountryCode,
        DateOnly? DateOfBirth,
        string? Nationality,
        Gender? Gender,
        string? AvatarUrl) : ICommand;

    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .MaximumLength(100);

            RuleFor(x => x.AddressLine)
                .MaximumLength(200);

            RuleFor(x => x.City)
                .MaximumLength(100);

            RuleFor(x => x.Country)
                .MaximumLength(100);

            RuleFor(x => x)
                .Must(x => BeValidPhoneNumber(x.PhoneNumber, x.CountryCode))
                .WithMessage("Invalid phone number format.");

            RuleFor(x => x)
                .Must(x =>
                    string.IsNullOrWhiteSpace(x.AddressLine) && 
                    string.IsNullOrWhiteSpace(x.City) && 
                    string.IsNullOrWhiteSpace(x.Country)
                    ||
                    !string.IsNullOrWhiteSpace(x.AddressLine) &&
                    !string.IsNullOrWhiteSpace(x.City) &&
                    !string.IsNullOrWhiteSpace(x.Country)
                )
                .WithMessage("All address fields must be provided together.");

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

    public class UpdateUserProfileHandler(ApplicationDbContext context) : ICommandHandler<UpdateUserProfileCommand>
    {
        public async Task<ErrorOr<Unit>> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

            if (user == null)
                return Errors.User.UserNotFound;

            var existingProfile = await context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == command.UserId, cancellationToken);

            if (existingProfile == null)
                return Errors.User.ProfileNotFound;

            Address? address = null;
            if (!string.IsNullOrWhiteSpace(command.AddressLine)
                && !string.IsNullOrWhiteSpace(command.City)
                && !string.IsNullOrWhiteSpace(command.Country))
            {
                address = Address.Of(
                    command.AddressLine,
                    command.City,
                    command.Country
                );
            }

            PhoneNumber? phoneNumber = null; 
            if (!string.IsNullOrWhiteSpace(command.PhoneNumber) 
                && !string.IsNullOrWhiteSpace(command.CountryCode))
            { 
                phoneNumber = PhoneNumber.Of(command.PhoneNumber, command.CountryCode);
            }

            existingProfile.UpdateProfile(
                command.FirstName,
                command.LastName,
                address,
                phoneNumber,
                command.DateOfBirth,
                command.Nationality,
                command.Gender,
                command.AvatarUrl
            );

            context.UserProfiles.Update(existingProfile);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}