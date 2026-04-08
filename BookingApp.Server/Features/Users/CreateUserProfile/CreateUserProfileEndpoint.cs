using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.BuildingBlocks.Extensions;
using BookingApp.Server.Features.Users.Models;
using BookingApp.Server.Infrastructure.Web;
using System.Security.Claims;

namespace BookingApp.Server.Features.Users.CreateUserProfile
{
    public record CreateUserProfileRequest(
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

    public record CreateUserProfileResponse(
        UserId UserId);

    public class CreateUserProfileEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/profiles", async (
                CreateUserProfileRequest request,
                ISender sender,
                ClaimsPrincipal user) =>
            {
                var command = request.Adapt<CreateUserProfileCommand>() with
                {
                    UserId = user.GetUserId()
                };
                var result = await sender.Send(command);
                return result.ToCreatedResult<CreateUserProfileResult, CreateUserProfileResponse>(
                    r => $"/api/users/profiles/{r.UserId.Value}");
            })
            .WithName("CreateUserProfile")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a user profile")
            .WithDescription("Creates a user profile with the provided information.")
            .WithTags("Users")
            .RequireRateLimiting("users")
            .RequireAuthorization();
        }
    }
}
