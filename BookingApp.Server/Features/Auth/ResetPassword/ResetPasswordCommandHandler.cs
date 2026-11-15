using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Auth.Domain.Errors;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;

namespace BookingApp.Server.Features.Auth.ResetPassword;

public record ResetPasswordCommand(
    Email Email
    ) : ICommand;

public class ResetPasswordCommandHandler(
    ApplicationDbContext context,
    IEmailSender emailSender,
    IVerificationService verificationService) : ICommandHandler<ResetPasswordCommand>
{
    public async Task<ErrorOr<Unit>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .Where(x => x.Email == command.Email)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (user == null)
            return Errors.Authentication.UserNotFound;

        var verificationCode = await verificationService.CreateVerificationToken(user.Id, VerificationCodeType.PasswordReset);
        await emailSender.SendPasswordResetEmailAsync(command.Email.Value, user.Id, verificationCode);

        return Unit.Value;
    }
}
