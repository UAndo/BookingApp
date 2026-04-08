using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;
using System.Security.Cryptography;

namespace BookingApp.Server.Infrastructure.Security
{
    public class VerificationService(ApplicationDbContext context, TimeProvider timeProvider) : IVerificationService
    {
        public async Task<string> CreateVerificationToken(UserId userId, VerificationCodeType type)
        {
            var token = Guid.NewGuid().ToString();
            var tokenHash = Hash(token);
            var now = timeProvider.GetUtcNow();

            context.VerificationCodes.Add(new VerificationCode
            {
                UserId = userId,
                TokenHash = tokenHash,
                IsUsed = false,
                Type = type,
                ExpiresAt = now.AddMinutes(10),
                CreatedAt = now
            });
            await context.SaveChangesAsync();

            return token;
        }

        public bool VerifyToken(UserId userId, string token, VerificationCodeType type)
        {
            var tokenHash = Hash(token);

            var existingCode = context.VerificationCodes
                .Where(vc => vc.UserId == userId && vc.Type == type && !vc.IsUsed && vc.ExpiresAt > DateTimeOffset.UtcNow)
                .FirstOrDefault(vc => vc.TokenHash == tokenHash);

            if (existingCode == null)
                return false;

            existingCode.IsUsed = true;
            context.SaveChanges();

            return true;
        }

        private static string Hash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA256.HashData(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
