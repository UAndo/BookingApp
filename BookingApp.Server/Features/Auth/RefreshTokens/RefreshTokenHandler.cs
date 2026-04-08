using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Auth.Domain.Abstractions;
using BookingApp.Server.Features.Auth.Domain.Errors;

namespace BookingApp.Server.Features.Auth.RefreshTokens
{
    public record RefreshTokenCommand(string RefreshToken)
        : ICommand<RefreshTokenResult>;

    public record RefreshTokenResult(
        string AccessToken,
        string RefreshToken
    );

    public class RefreshTokenHandler(
        ApplicationDbContext context,
        ITokenProvider tokenProvider,
        TimeProvider timeProvider) : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
    {
        public async Task<ErrorOr<RefreshTokenResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefault(rt => rt.Token == request.RefreshToken);

            if (refreshToken == null || refreshToken.ExpiresOnUtc < timeProvider.GetUtcNow())
                return Errors.Authentication.InvalidRefreshToken;

            var accessToken = tokenProvider.Create(refreshToken.User);

            refreshToken.Token = tokenProvider.GenerateRefreshToken();
            refreshToken.ExpiresOnUtc = timeProvider.GetUtcNow().AddDays(7);

            await context.SaveChangesAsync(cancellationToken);

            return new RefreshTokenResult(accessToken, refreshToken.Token);
        }
    }
}
