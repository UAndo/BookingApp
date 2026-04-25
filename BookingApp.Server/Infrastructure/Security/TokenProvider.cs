using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Cryptography;
using BookingApp.Server.Features.Auth.Domain.Users;


namespace BookingApp.Server.Infrastructure.Security;

public class TokenProvider(
    IOptions<JwtOptions> jwtOptions,
    IOptions<RefreshTokenOptions> refreshTokenOptions,
    TimeProvider timeProvider) : ITokenProvider
{

    public string Create(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
                new Claim("email_verified", user.EmailVerified.ToString())
            ]),
            Expires = timeProvider.GetUtcNow().AddMinutes(jwtOptions.Value.ExpiryMinutes).DateTime,
            SigningCredentials = signingCredentials,
            Issuer = jwtOptions.Value.Issuer,
            Audience = jwtOptions.Value.Audience
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
