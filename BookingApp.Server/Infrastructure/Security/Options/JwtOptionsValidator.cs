namespace BookingApp.Server.Services.Security.Options;

public class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        if (options == null)
            return ValidateOptionsResult.Fail("Options cannot be null.");

        if (string.IsNullOrWhiteSpace(options.Secret))
            return ValidateOptionsResult.Fail("JWT Secret cannot be empty.");

        if (options.ExpiryMinutes < 0)
            return ValidateOptionsResult.Fail("JWT ExpiryMinutes must be greater than zero.");

        if (string.IsNullOrWhiteSpace(options.Issuer))
            return ValidateOptionsResult.Fail("JWT Issuer cannot be empty.");

        if (string.IsNullOrWhiteSpace(options.Audience))
            return ValidateOptionsResult.Fail("JWT Audience cannot be empty.");

        return ValidateOptionsResult.Success;
    }
}
