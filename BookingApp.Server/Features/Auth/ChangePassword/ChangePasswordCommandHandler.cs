using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Auth.Domain.Errors;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;

namespace BookingApp.Server.Features.Auth.ChangePassword;

public record ChangePasswordCommand(
    string Email,
    string NewPassword,
    string ResetToken
    ) : ICommand;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one digit.");
    }
}

public class ChangePasswordCommandHandler(
    ApplicationDbContext context,
    IVerificationService verificationService,
    IPasswordHasher passwordHasher) : ICommandHandler<ChangePasswordCommand>
{
    public async Task<ErrorOr<Unit>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email.Value == command.Email, cancellationToken);
        if (user == null)
            return Errors.Authentication.UserNotFound;

        var isValidToken = verificationService.VerifyToken(user.Id, command.ResetToken, VerificationCodeType.PasswordReset);
        if (!isValidToken)
            return Errors.Authentication.InvalidPasswordResetToken;

        var passwordHash = passwordHasher.HashPassword(command.NewPassword);

        user.ChangePassword(passwordHash);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
