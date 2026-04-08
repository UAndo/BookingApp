using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Auth.Domain.Errors;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;

namespace BookingApp.Server.Features.Auth.ResendVerification;

public record ResendVerificationCommand(
    string Email
    ) : ICommand;

public class ResendVerificationHandler(
    ApplicationDbContext context,
    IEmailSender emailSender,
    IVerificationService verificationService) : ICommandHandler<ResendVerificationCommand>
{
    public async Task<ErrorOr<Unit>> Handle(
        ResendVerificationCommand command,
        CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(command.Email.ToLower()), cancellationToken);
        if (user is null)
        {
            return Errors.Authentication.UserNotFound;
        }

        var verificationCode = await verificationService.CreateVerificationToken(
            user.Id, VerificationCodeType.EmailConfirmation);

        await emailSender.SendVerificationEmailAsync(command.Email, user.Id, verificationCode);

        return Unit.Value;
    }
}
