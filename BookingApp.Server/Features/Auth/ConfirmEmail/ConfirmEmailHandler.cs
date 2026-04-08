using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Auth.Domain.Errors;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;

namespace BookingApp.Server.Features.Auth.ConfirmEmail;

public record ConfirmEmailCommand(
    string Email,
    string ConfirmationCode
    ) : ICommand;

public class ConfirmEmailHandler(
    ApplicationDbContext context,
    IVerificationService verificationService) : ICommandHandler<ConfirmEmailCommand>
{
    public async Task<ErrorOr<Unit>> Handle(
        ConfirmEmailCommand command,
        CancellationToken cancellationToken)
    {
        var user = context.Users.FirstOrDefault(u => u.Email.Equals(command.Email.ToLower()));
        if (user is null || !verificationService.VerifyToken(user.Id, command.ConfirmationCode,
            VerificationCodeType.EmailConfirmation))
        {
            return Errors.Authentication.InvalidVerificationToken;
        }

        user.MarkEmailAsVerified();

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
