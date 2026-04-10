using BookingApp.Server.BuildingBlocks.Domain.Abstractions;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Auth.Domain.Users;

public sealed class User : Entity<UserId>
{
    public Email Email { get; private set; } = default!;
    public string NormalizedEmail { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public bool EmailVerified { get; private set; }
    public bool IsLocked { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public int AccessFailedCount { get; private set; }
    public DateTimeOffset? LockoutEnd { get; private set; }
    public Guid SecurityStamp { get; private set; }
    public DateTimeOffset? LastLoginAt { get; private set; }
    private User() { }

    public static User Create(Email email, string passwordHash, DateTime createdAt)
    {
        return new User
        {
            Id = UserId.Of(Guid.NewGuid()),
            Email = email,
            PasswordHash = passwordHash,
            EmailVerified = false,
            IsLocked = false,
            SecurityStamp = Guid.NewGuid(),
            AccessFailedCount = 0,
            CreatedAt = createdAt,
            LastModified = createdAt
        };
    }

    public void RecordFailedLogin(DateTimeOffset failedAt)
    {
        AccessFailedCount++;

        if (AccessFailedCount >= 5)
        {
            LockAccount(failedAt.AddMinutes(15));
        }
    }

    public void ResetAccessFailedCount()
    {
        AccessFailedCount = 0;
    }

    public void LockAccount(DateTimeOffset lockoutEnd)
    {
        IsLocked = true;
        LockoutEnd = lockoutEnd;
    }

    public void UnlockAccount()
    {
        IsLocked = false;
        LockoutEnd = null;
        ResetAccessFailedCount();
    }

    public void MarkEmailAsVerified()
    {
        EmailVerified = true;
    }

    public void UpdateLastLogin(DateTimeOffset lastLoginAt)
    {
        LastLoginAt = lastLoginAt;
    }

    public void SoftDelete(DateTimeOffset deletedAt)
    {
        if (DeletedAt.HasValue) return;
        DeletedAt = deletedAt;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        SecurityStamp = Guid.NewGuid();
    }
}
