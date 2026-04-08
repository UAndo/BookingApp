using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Server.Features.Auth.Data.Configurations;

public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
{
    public void Configure(EntityTypeBuilder<VerificationCode> builder)
    {
        builder.HasKey(vc => vc.Id);
        
        builder.Property(vc => vc.UserId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Of(value));

        builder.Property(vc => vc.TokenHash)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(vc => vc.IsUsed)
            .IsRequired();

        builder.Property(vc => vc.Type)
            .IsRequired();

        builder.Property(vc => vc.ExpiresAt)
            .IsRequired();

        builder.Property(vc => vc.CreatedAt)
            .IsRequired();
    }
}