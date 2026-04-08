using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Auth.Domain.VerificationCodes;

namespace BookingApp.Server.Features.Auth.Domain.Abstractions
{
    public interface IVerificationService
    {
        public Task<string> CreateVerificationToken(UserId userId, VerificationCodeType type);
        public bool VerifyToken(UserId userId, string token, VerificationCodeType type);
    }
}
