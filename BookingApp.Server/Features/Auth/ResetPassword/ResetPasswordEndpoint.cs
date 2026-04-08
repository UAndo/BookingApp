using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Auth.ResetPassword;

public class ResetPasswordEndpoint : ICarterModule
{
    public record ResetPasswordRequest(
        string Email
    );

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/reset-password", async (
            [AsParameters] ResetPasswordRequest request, ISender sender) =>
        {
            var command = request.Adapt<ResetPasswordCommand>();
            var result = await sender.Send(command);
            return result.ToNoContentResult();
        })
        .WithName("ResetPassword")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Requests a password reset")
        .WithDescription("Sends a password reset email to the specified user.")
        .WithTags("Auth")
        .RequireRateLimiting("auth");
    }
}
