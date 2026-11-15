using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Auth.Domain.Users;
using BookingApp.Server.Features.Listings.Domain.Listings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Server.Features.Listings.Data.Configurations
{
    public class ListingConfiguration : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ListingId.Of(value));

            builder.Property(l => l.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(l => l.PricePerNight)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.MaxGuests)
                .IsRequired();

            builder.Property(l => l.Status)
                .IsRequired();

            builder.Property(l => l.OwnerId)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(l => l.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(l => l.Address, a =>
            {
                a.Property(ad => ad.AddressLine)
                    .HasColumnName("AddressStreet")
                    .IsRequired()
                    .HasMaxLength(200);
                a.Property(ad => ad.City)
                    .HasColumnName("AddressCity")
                    .IsRequired()
                    .HasMaxLength(100);
                a.Property(ad => ad.Country)
                    .HasColumnName("AddressCountry")
                    .IsRequired()   
                    .HasMaxLength(100);
            });

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

            builder.HasIndex(l => new { l.OwnerId, l.Status });
        }
    }
}
