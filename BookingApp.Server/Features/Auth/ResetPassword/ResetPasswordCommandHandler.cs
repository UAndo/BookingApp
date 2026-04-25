using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Auth.Domain.Errors;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;

namespace BookingApp.Server.Features.Auth.ResetPassword;

public record ResetPasswordCommand(
    string Email
    ) : ICommand;

public class ResetPasswordCommandHandler(
    ApplicationDbContext context,
    IEmailSender emailSender,
    IVerificationService verificationService) : ICommandHandler<ResetPasswordCommand>
{
    public async Task<ErrorOr<Unit>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email.Value == command.Email, cancellationToken);
        if (user == null)
            return Errors.Authentication.UserNotFound;

        var verificationCode = await verificationService.CreateVerificationToken(user.Id, VerificationCodeType.PasswordReset);
        await emailSender.SendPasswordResetEmailAsync(command.Email, user.Id, verificationCode);

        return Unit.Value;
    }
}
