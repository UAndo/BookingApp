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
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.CreatedAt);

        builder.Property(u => u.CreatedBy)
            .HasMaxLength(255);

        builder.Property(u => u.LastModified);

        builder.Property(u => u.LastModifiedBy)
            .HasMaxLength(255);

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
