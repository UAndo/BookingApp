using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Auth.Domain.Abstractions;

namespace BookingApp.Server.Features.Auth.Logout;

public record LogoutCommand(
    string RefreshToken
    ) : ICommand;

public class LogoutHandler(ApplicationDbContext context, ITokenHasher tokenHasher) : ICommandHandler<LogoutCommand>
{
    public async Task<ErrorOr<Unit>> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var tokenHash = tokenHasher.HashToken(command.RefreshToken);
        
        await context.RefreshTokens.Where(rt => rt.Token == tokenHash).ExecuteDeleteAsync(cancellationToken);

        return Unit.Value;
    }
}
