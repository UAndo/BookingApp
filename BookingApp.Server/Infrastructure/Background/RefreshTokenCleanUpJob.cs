namespace BookingApp.Server.Infrastructure.Background
{
    public class RefreshTokenCleanUpJob(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested) 
            {
                await CleanUpExpiredTokens(ct);
                await Task.Delay(TimeSpan.FromDays(1), ct);
            }
        }

        private async Task CleanUpExpiredTokens(CancellationToken ct)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.RefreshTokens
                .Where(rt => rt.ExpiresOnUtc <= DateTimeOffset.UtcNow)
                .ExecuteDeleteAsync(ct);
        }
    }
}
