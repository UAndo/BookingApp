using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookingApp.Server.BuildingBlocks.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static UserId GetUserId(this ClaimsPrincipal principal)
        {
            var value = principal.FindFirstValue(JwtRegisteredClaimNames.Sub) 
                ?? throw new InvalidOperationException("User ID claim not found.");

            return UserId.Of(Guid.Parse(value));
        }
    }
}
