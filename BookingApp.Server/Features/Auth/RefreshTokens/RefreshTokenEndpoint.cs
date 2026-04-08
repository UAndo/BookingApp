using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Auth.RefreshTokens
{
    public record RefreshTokenRequest(string RefreshToken);

    public record RefreshTokenResponse(
        string AccessToken,
        string RefreshToken
    );

    public class RefreshTokenEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/refresh-token", async (RefreshTokenRequest request, ISender sender) =>
            {
                var command = request.Adapt<RefreshTokenCommand>();
                var result = await sender.Send(command);
                return result.ToOkResult<RefreshTokenResult, RefreshTokenResponse>();
            })
            .WithName("RefreshToken")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Refreshes a user's access token")
            .WithDescription("Refreshes a user's access token using a valid refresh token.")
            .WithTags("Auth")
            .RequireRateLimiting("auth");
        }
    }
}
