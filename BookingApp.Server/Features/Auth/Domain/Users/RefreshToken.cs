using BookingApp.Server.BuildingBlocks.Domain.Abstractions;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Auth.Domain.Users;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = default!;
    public UserId UserId { get; set; } = default!;
    public DateTimeOffset ExpiresOnUtc { get; set; }

    public User User { get; set; } = default!;
}