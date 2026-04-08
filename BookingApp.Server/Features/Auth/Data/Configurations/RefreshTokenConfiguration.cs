using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Auth.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Server.Features.Auth.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id).ValueGeneratedNever();

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.Property(rt => rt.ExpiresOnUtc)
            .IsRequired();

        builder.Property(rt => rt.UserId)
            .HasConversion(
               id => id.Value,
                value => UserId.Of(value));

        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}