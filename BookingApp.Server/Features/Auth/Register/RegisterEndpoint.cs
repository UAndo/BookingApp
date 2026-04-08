using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Auth.Register;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);

public record RegisterResponse(Guid UserId);

public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (RegisterRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterCommand>();

            var result =  await sender.Send(command);

            return result.ToCreatedResult<RegisterResult, RegisterResponse>(
                r => $"/api/users/{r.UserId}");
        })
        .WithName("Register")
        .Produces<RegisterResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Registers a new user")
        .WithDescription("Registers a new user with the provided information.")
        .WithTags("Auth")
        .RequireRateLimiting("auth");
    }
}
