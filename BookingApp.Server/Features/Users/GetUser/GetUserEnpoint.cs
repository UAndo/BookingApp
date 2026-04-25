using BookingApp.Server.Features.Users.Models;
using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Users.GetUser
{
    public record GetUserResponse(UserDto User);

    public class GetUserEnpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/{userId:guid}", async (
                Guid userId,
                ISender sender) =>
            {
                var query = new GetUserQuery(userId);
                var result = await sender.Send(query);
                return result.ToOkResult<GetUserResult, GetUserResponse>();
            })
            .WithName("GetUser")
            .Produces<GetUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Gets a user by their ID")
            .WithDescription("Retrieves the details of a user with the specified ID.")
            .WithTags("Users")
            .RequireRateLimiting("users")
            .RequireAuthorization();
        }
    }
}
