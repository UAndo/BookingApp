using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Auth.Domain.Errors;
using BookingApp.Server.Features.Auth.Domain.Users;

namespace BookingApp.Server.Features.Auth.Login;

public record LoginCommand(
    string Email,
    string Password
    ) : ICommand<LoginResult>;

public record LoginResult(string AccessToken, string RefreshToken);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);
    }
}

public class LoginHandler(
    ApplicationDbContext context,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    TimeProvider timeProvider,
    IOptions<RefreshTokenOptions> refreshTokenOptions) : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<ErrorOr<LoginResult>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail.Equals(command.Email.ToLowerInvariant()), cancellationToken);

        if (user == null || !user.EmailVerified)
            return Errors.Authentication.UserNotFound;

        if (user.IsLocked)
            return Errors.Authentication.AccountLocked;

        if (!passwordHasher.VerifyPassword(command.Password, user.PasswordHash))
        {
            user.RecordFailedLogin(timeProvider.GetUtcNow());

            if (user.AccessFailedCount >= 5)
            {
                user.LockAccount(timeProvider.GetUtcNow().AddMinutes(15));
            }

            await context.SaveChangesAsync(cancellationToken);

            return Errors.Authentication.InvalidCredentials;
        }

        var accessToken = tokenProvider.Create(user);
        var token = tokenProvider.GenerateRefreshToken();

        var refreshToken = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = token,
            ExpiresOnUtc = timeProvider.GetUtcNow().AddDays(refreshTokenOptions.Value.ExpiryDays)
        };

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        return new LoginResult(accessToken, refreshToken.Token);
    }
}
