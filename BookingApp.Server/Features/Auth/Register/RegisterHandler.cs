using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Auth.Domain.Errors;
using BookingApp.Server.Features.Auth.Domain.Users;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Auth.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
    ) : ICommand<RegisterResult>;

public record RegisterResult(Guid UserId);

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
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

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one digit.");
    }
}

public class RegisterCommandHandler(
    ApplicationDbContext context, IPasswordHasher passwordHasher,
    IVerificationService verificationService, IEmailSender emailSender,
    TimeProvider timeProvider)
 : ICommandHandler<RegisterCommand, RegisterResult>
{
    public async Task<ErrorOr<RegisterResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(command.Email.ToLower()), cancellationToken);

        if (user != null)
        {
            return Errors.Authentication.UserAlreadyExists;
        }

        var email = Email.Of(command.Email);

        user = User.Create(
             email,
             passwordHasher.HashPassword(command.Password),
             timeProvider.GetUtcNow().UtcDateTime
         );

        var verificationCode = await verificationService.CreateVerificationToken(
            user.Id, VerificationCodeType.EmailConfirmation);

        await emailSender.SendVerificationEmailAsync(command.Email, user.Id, verificationCode);

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return new RegisterResult(user.Id.Value);
    }
}
