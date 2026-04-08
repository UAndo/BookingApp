using BookingApp.Server.Features.Auth.Domain.Users;

namespace BookingApp.Server.Features.Auth.Domain.Abstractions;

public interface ITokenProvider
{
    public string Create(User user);
    public string GenerateRefreshToken();   
}
