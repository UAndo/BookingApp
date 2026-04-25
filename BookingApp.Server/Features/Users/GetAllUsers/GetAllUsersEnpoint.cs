using BookingApp.Server.Features.Users.Models;
using BookingApp.Server.Infrastructure.Web;

namespace BookingApp.Server.Features.Users.GetAllUsers
{
    public record GetAllUsersRequest(int? PageNumber = 1, int? PageSize = 10);
    public record GetAllUsersResponse(
        List<UserDto> Users,
        int PageNumber,
        int TotalPages,
        int TotalCount,
        bool HasNextPage
    );

    public class GetAllUsersEnpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/",
                async ([AsParameters] GetAllUsersRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetAllUsersQuery>();

                var result = await sender.Send(query);

                return result.ToOkResult<GetAllUsersResult, GetAllUsersResponse>();
            })
            .WithName("GetAllUsers")
            .Produces<GetAllUsersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Gets all users with pagination")
            .WithDescription("Retrieves the details of all users with pagination support.")
            .WithTags("Users")
            .RequireRateLimiting("users")
            .RequireAuthorization();
        }
    }
}
