using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Auth.ResendVerification;

public record ResendVerificationRequest(
    string Email);

public class ResendVerificationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/resend-verification", async (
            ResendVerificationRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<ResendVerificationCommand>();
            var result = await sender.Send(command, cancellationToken);
            return result.ToNoContentResult();
        })
        .WithName("ResendVerification")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Resends the verification code to a user's email")
        .WithDescription("Resends the verification code to a user's email if the user exists and is not yet verified.")
        .WithTags("Auth")
        .RequireRateLimiting("auth");
    }
}
