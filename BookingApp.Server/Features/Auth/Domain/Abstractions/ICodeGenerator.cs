namespace BookingApp.Server.Features.Auth.Domain.Abstractions;

public interface ICodeGenerator
{
    string GenerateCode(int length);
}
