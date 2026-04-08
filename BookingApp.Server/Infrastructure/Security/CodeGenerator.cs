using System.Security.Cryptography;

namespace BookingApp.Server.Infrastructure.Security;

public class CodeGenerator : ICodeGenerator
{
    public string GenerateCode(int length)
    {
        return RandomNumberGenerator.GetInt32((int)Math.Pow(10, length)).ToString();
    }
}