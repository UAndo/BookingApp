using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Auth.ConfirmEmail;

public record ConfirmEmailRequest(
    string Email,
    string ConfirmationCode);

public class ConfirmEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/confirm-email", async (ConfirmEmailRequest request, ISender sender) =>
        {
            var command = request.Adapt<ConfirmEmailCommand>();

            var result = await sender.Send(command);

            return result.ToNoContentResult();
        })
        .WithName("ConfirmEmail")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Confirms a user's email")
        .WithDescription("Confirms a user's email with the provided verification code.")
        .WithTags("Auth")
        .RequireRateLimiting("auth");
    }
}
