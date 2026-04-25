using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Users.Models
{
    public record UserDto(
        Guid Id,
        Email Email,
        bool EmailVerified
    );
}
