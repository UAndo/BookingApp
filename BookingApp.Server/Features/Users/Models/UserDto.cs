namespace BookingApp.Server.Features.Users.Common
{
    public record UserDto(
        Guid Id,
        string Email,
        bool EmailVerified
    );
}
