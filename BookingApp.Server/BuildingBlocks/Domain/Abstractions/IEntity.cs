using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.BuildingBlocks.Domain.Abstractions;

public interface IEntity<TId> : IEntity
{
    public TId Id { get; set; }
}

public interface IEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public UserId? CreatedBy { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public UserId? LastModifiedBy { get; set; }
}
