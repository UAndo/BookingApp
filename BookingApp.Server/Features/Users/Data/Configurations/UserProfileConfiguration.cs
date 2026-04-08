using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Auth.Domain.Users;
using BookingApp.Server.Features.Users.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Server.Features.Users.Data.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(up => up.UserId);

            builder.Property(up => up.UserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Of(value));

            builder.Property(up => up.FirstName)
                .HasMaxLength(100);

            builder.Property(up => up.LastName)
                .HasMaxLength(100);

            builder.Property(up => up.PhoneNumber)
                .HasConversion(
                    pn => pn.Value,
                    value => PhoneNumber.FromDb(value))
                .HasMaxLength(20);

            builder.Property(up => up.DateOfBirth);

            builder.Property(up => up.Nationality)
                .HasMaxLength(100);

            builder.Property(up => up.Gender);

            builder.Property(up => up.AvatarUrl)
                .HasMaxLength(200);

            builder.OwnsOne(up => up.Address, a =>
            {
                a.Property(ad => ad.AddressLine)
                    .HasMaxLength(200);
                a.Property(ad => ad.City)
                    .HasMaxLength(100);
                a.Property(ad => ad.Country)
                    .HasMaxLength(100);
            });

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
