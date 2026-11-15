using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Auth.ChangePassword;

public class ChangePasswordEndpoint : ICarterModule
{
    public record ChangePasswordRequest(
        string Email,
        string NewPassword,
        string ResetToken
    );

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/change-password", async (
            [AsParameters] ChangePasswordRequest request, ISender sender) =>
        {
            var command = new ChangePasswordCommand(
                Email.Of(request.Email),
                request.NewPassword,
                request.ResetToken);
            
            var result = await sender.Send(command);
            return result.ToNoContentResult();
        })
        .WithName("ChangePassword")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Changes the user's password")
        .WithDescription("Changes the user's password using the provided reset token.")
        .WithTags("Auth")
        .RequireRateLimiting("auth");
    }
}
