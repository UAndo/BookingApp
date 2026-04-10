using BookingApp.Server.BuildingBlocks.Domain.Common;

namespace BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

public record ListingId
{
    public Guid Value { get; }
    private ListingId(Guid value) => Value = value;
    public static ListingId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("ListingId cannot be empty.");
        }
        return new ListingId(value);
    }
}