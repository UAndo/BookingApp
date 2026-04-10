using BookingApp.Server.Features.Auth.Domain.Users;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;
using BookingApp.Server.Features.Bookings.Domain.Bookings;
using BookingApp.Server.Features.Users.Models;
using System.Reflection;

namespace BookingApp.Server.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
