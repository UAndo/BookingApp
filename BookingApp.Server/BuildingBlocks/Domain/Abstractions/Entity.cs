using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.BuildingBlocks.Domain.Abstractions;

public abstract class Entity<TId> : IEntity<TId>
{
    public TId Id { get; set; } = default!;
    public DateTimeOffset? CreatedAt { get; set; }
    public UserId? CreatedBy { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public UserId? LastModifiedBy { get; set; }
}