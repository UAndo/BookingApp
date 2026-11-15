using BookingApp.Server.Features.Bookings.Domain.Bookings;

namespace BookingApp.Server.Infrastructure.Background
{
    public class MarkBookingsAsExpiredJob(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested) 
            {
                await MarkBookingsAsExpired(ct);
                await Task.Delay(TimeSpan.FromMinutes(1), ct);
            }
        }

        private async Task MarkBookingsAsExpired(CancellationToken ct)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Bookings
                .Where(b => 
                    (b.Status == BookingStatus.Draft || b.Status == BookingStatus.PendingPayment) 
                    && 
                    b.ExpiresAt <= DateTimeOffset.UtcNow)
                .ExecuteUpdateAsync(setProperty => setProperty.SetProperty(b => b.Status, BookingStatus.Expired), ct);
        }
    }
}
