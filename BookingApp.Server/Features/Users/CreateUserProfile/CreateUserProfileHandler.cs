using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Users.Common;
using BookingApp.Server.Features.Users.Models;

namespace BookingApp.Server.Features.Users.CreateUserProfile
{
    public record CreateUserProfileCommand(
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
        string? AvatarUrl) : ICommand<CreateUserProfileResult>;

    public record CreateUserProfileResult(UserId UserId);

    public class CreateUserProfileCommandValidator : AbstractValidator<CreateUserProfileCommand>
    {
        public CreateUserProfileCommandValidator()
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

    public class CreateUserProfileHandler(ApplicationDbContext context) : ICommandHandler<CreateUserProfileCommand, CreateUserProfileResult>
    {
        public async Task<ErrorOr<CreateUserProfileResult>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                return Errors.User.UserNotFound;

            var existingProfile = await context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

            if (existingProfile != null)
                return Errors.User.ProfileAlreadyExists;

            Address? address = null;
            if (!string.IsNullOrWhiteSpace(request.AddressLine)
                && !string.IsNullOrWhiteSpace(request.City)
                && !string.IsNullOrWhiteSpace(request.Country))
            {
                address = Address.Of(
                    request.AddressLine,
                    request.City,
                    request.Country
                );
            }

            PhoneNumber? phoneNumber = null; 
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber) 
                && !string.IsNullOrWhiteSpace(request.CountryCode))
            { 
                phoneNumber = PhoneNumber.Of(request.PhoneNumber, request.CountryCode);
            }

            var profile = UserProfile.Create(
                user.Id,
                request.FirstName,
                request.LastName,
                address,
                phoneNumber,
                request.DateOfBirth,
                request.Nationality,
                request.Gender,
                request.AvatarUrl
            );

            context.UserProfiles.Add(profile);

            await context.SaveChangesAsync(cancellationToken);

            return new CreateUserProfileResult(profile.UserId);
        }
    }
}