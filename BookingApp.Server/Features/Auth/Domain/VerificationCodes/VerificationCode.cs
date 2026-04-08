using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Auth.Domain.VerificationCodes
{
    public class VerificationCode
    {
        public Guid Id { get; set; }
        public UserId UserId { get; set; } 
        public string TokenHash { get; set; }
        public bool IsUsed { get; set; }
        public VerificationCodeType Type { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
