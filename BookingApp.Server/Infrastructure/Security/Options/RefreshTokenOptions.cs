namespace BookingApp.Server.Services.Security.Options;

public class RefreshTokenOptions
{
    public const string SectionName = "RefreshTokenOptions";
    public int ExpiryDays { get; set; }
    public TimeSpan Expiry => TimeSpan.FromDays(ExpiryDays);
}
