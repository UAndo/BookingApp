using BookingApp.Server.Features.Users.Models;
using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Users.GetUserProfile
{
    public record GetUserProfileResponse(UserProfile UserProfile);

    public class GetUserProfileEnpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/profiles/{userId:guid}", async (
                Guid userId,
                ISender sender) =>
            {
                var query = new GetUserProfileQuery(userId);
                var result = await sender.Send(query);
                return result.ToOkResult<GetUserProfileResult, GetUserProfileResponse>();
            })
            .WithName("GetUserProfile")
            .Produces<GetUserProfileResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Gets a user's profile by their ID")
            .WithDescription("Retrieves the details of a user's profile with the specified ID.")
            .WithTags("Users")
            .RequireRateLimiting("users")
            .RequireAuthorization();
        }
    }
}
