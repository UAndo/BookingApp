using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Auth.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Server.Features.Auth.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Of(value));

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasConversion(
                email => email.Value,
                value => Email.Of(value));

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(b => b.CreatedAt);

        builder.Property(b => b.CreatedBy)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? UserId.Of(value.Value) : null);

        builder.Property(b => b.LastModified);

        builder.Property(b => b.LastModifiedBy)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? UserId.Of(value.Value) : null);

        builder.Property(u => u.AccessFailedCount)
            .IsRequired();

        builder.Property(u => u.IsLocked)
            .IsRequired();

        builder.Property(u => u.DeletedAt);

        builder.Property(u => u.EmailVerified)
            .IsRequired();

        builder.Property(u => u.LastLoginAt);

        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
