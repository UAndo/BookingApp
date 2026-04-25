using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Auth.Domain.Users;
using BookingApp.Server.Features.Bookings.Domain.Bookings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Server.Features.Bookings.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => BookingId.Of(value));

            builder.OwnsOne(b => b.BookingGuest, bc =>
            {
                bc.Property(c => c.FirstName)
                .HasColumnName("GuestFirstName")
                .HasMaxLength(100);

                bc.Property(c => c.LastName)
                .HasColumnName("GuestLastName")
                .HasMaxLength(100);

                bc.Property(c => c.Email)
                .HasColumnName("GuestEmail")
                .HasMaxLength(255)
                .HasConversion(
                    email => email.Value,
                    value => Email.Of(value));

                bc.OwnsOne(c => c.Address, a =>
                {
                    a.Property(ad => ad.AddressLine)
                    .HasColumnName("GuestAddressStreet")
                    .HasMaxLength(200);

                    a.Property(ad => ad.City)
                    .HasColumnName("GuestAddressCity")
                    .HasMaxLength(100);

                    a.Property(ad => ad.Country)
                    .HasColumnName("GuestAddressCountry")
                    .HasMaxLength(100);
                });

                bc.Property(c => c.PhoneNumber)
                .HasColumnName("GuestPhoneNumber")
                .HasConversion(
                    phone => phone.Value,
                    value => PhoneNumber.FromDb(value));

                bc.Property(c => c.ArrivingTime)
                .HasColumnName("GuestArrivingTime");
            });

            builder.OwnsMany(b => b.Guests, g =>
            {
                g.WithOwner().HasForeignKey("BookingId");
                g.Property<int>("Id");
                g.HasKey("Id");

                g.Property(g => g.GuestKind)
                .HasColumnName("GuestKind")
                .HasConversion<string>();
            });

            builder.Property(b => b.UserId)
                .HasConversion(
                    id => id != null ? id.Value : (Guid?)null,
                    value => value.HasValue ? UserId.Of(value.Value) : null);

            builder.Property(b => b.ListingId)
               .HasConversion(
                   id => id.Value,
                   value => ListingId.Of(value));

            builder.Property(b => b.Period)
                .HasConversion(
                    v => v.ToNpgsqlRange(),
                    v => DateRange.FromNpgsqlRange(v));

            builder.Property(b => b.Status)
               .HasConversion<string>();

            builder.Property(b => b.ExpiresAt);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(b => b.UserId);

            //builder.HasOne<Listing>()
            //    .WithMany()
            //    .HasForeignKey(b => b.ListingId); //TODO: uncomment when Listing entity is implemented

            builder.HasIndex(b => b.ListingId);

            builder.HasIndex(b => new { b.ListingId, b.Status});
        }
    }
}
