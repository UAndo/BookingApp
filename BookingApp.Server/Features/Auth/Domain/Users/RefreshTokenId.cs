using BookingApp.Server.BuildingBlocks.Domain.Common;

namespace BookingApp.Server.Features.Auth.Domain.Users;

public record RefreshTokenId
{
    public Guid Value { get; }
    private RefreshTokenId(Guid value) => Value = value;
    public static RefreshTokenId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("RefreshTokenId cannot be empty.");
        }
        return new RefreshTokenId(value);
    }
}