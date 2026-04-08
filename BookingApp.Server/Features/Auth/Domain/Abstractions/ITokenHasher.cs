namespace BookingApp.Server.Features.Auth.Domain.Abstractions;

public interface ITokenHasher
{
    string HashToken(string rawToken);
    bool VerifyToken(string rawToken, string tokenHash);
}
