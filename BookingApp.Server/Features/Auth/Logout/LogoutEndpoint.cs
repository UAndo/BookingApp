using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Auth.Logout;

public record LogoutRequest(
    string RefreshToken
);

public class LogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/logout", async (
            LogoutRequest request,
            ISender sender) =>
        {
            var command = request.Adapt<LogoutCommand>();

            var result = await sender.Send(command);

            return result.ToNoContentResult();
        })
        .WithName("Logout")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Logs out a user")
        .WithDescription("Logs out a user by revoking their refresh token.")
        .WithTags("Auth")
        .RequireRateLimiting("auth");
    }
}
