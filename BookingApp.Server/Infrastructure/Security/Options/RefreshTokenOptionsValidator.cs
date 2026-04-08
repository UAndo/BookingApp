namespace BookingApp.Server.Services.Security.Options;

public class RefreshTokenOptionsValidator : IValidateOptions<RefreshTokenOptions>
{
    public ValidateOptionsResult Validate(string? name, RefreshTokenOptions options)
    {
        if (options == null)
            return ValidateOptionsResult.Fail("Options cannot be null.");

        if (options.ExpiryDays <= 0)
            return ValidateOptionsResult.Fail("ExpiryMinutes must be greater than zero.");

        return ValidateOptionsResult.Success;
    }
}
