namespace BookingApp.Server.Services.Security.Options;

public class JwtOptions
{
    public const string SectionName = "JwtOptions";
    public string Secret { get; set; } = null!;
    public int ExpiryMinutes { get; set; }
    public TimeSpan Expiry => TimeSpan.FromMinutes(ExpiryMinutes);
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}
