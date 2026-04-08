using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Auth.Login;

public record LoginRequest(
    string Email,
    string Password);

public record LoginResponse(
    string AccessToken,
    string RefreshToken);

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
            LoginRequest request,
            ISender sender) =>
        {
            var command = request.Adapt<LoginCommand>();

            var result = await sender.Send(command);

            return result.ToOkResult<LoginResult, LoginResponse>();
        })
        .WithName("Login")
        .Produces<LoginResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Logs in a user")
        .WithDescription("Logs in a user with the provided credentials.")
        .WithTags("Auth")
        .RequireRateLimiting("auth");
    }
}
