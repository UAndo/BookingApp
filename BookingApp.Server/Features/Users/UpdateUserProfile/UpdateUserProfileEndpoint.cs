using BookingApp.Server.BuildingBlocks.Extensions;
using BookingApp.Server.Features.Users.Models;
using BookingApp.Server.Infrastructure.Web;
using System.Security.Claims;

namespace BookingApp.Server.Features.Users.UpdateUserProfile
{
    public record UpdateUserProfileRequest(
        string? FirstName,
        string? LastName,
        string? AddressLine,
        string? City,
        string? Country,
        string? PhoneNumber,
        string? CountryCode,
        DateOnly? DateOfBirth,
        string? Nationality,
        Gender? Gender,
        string? AvatarUrl);

    public class UpdateUserProfileEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/users/profiles", async (
                UpdateUserProfileRequest request,
                ISender sender,
                ClaimsPrincipal user) =>
            {
                var command = request.Adapt<UpdateUserProfileCommand>() with
                {
                    UserId = user.GetUserId()
                };
                var result = await sender.Send(command);
                return result.ToNoContentResult();
            })
            .WithName("UpdateUserProfile")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Updates a user profile")
            .WithDescription("Updates a user profile with the provided information.")
            .WithTags("Users")
            .RequireRateLimiting("users")
            .RequireAuthorization();
        }
    }
}
