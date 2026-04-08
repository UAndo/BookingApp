using System.Security.Cryptography;

namespace BookingApp.Server.Infrastructure.Security;

public class TokenHasher : ITokenHasher
{
    public string HashToken(string rawToken)
    {
        var bytes = Encoding.UTF8.GetBytes(rawToken);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    public bool VerifyToken(string rawToken, string tokenHash)
    {
        var hashedToken = HashToken(rawToken);
        return hashedToken == tokenHash;
    }
}
