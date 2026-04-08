using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Pagination;
using BookingApp.Server.Features.Users.Common;

namespace BookingApp.Server.Features.Users.GetAllUsers
{
    public record GetAllUsersQuery(int? PageNumber = 1, int? PageSize = 10)
        : IQuery<GetAllUsersResult>;

    public record GetAllUsersResult(
        List<UserDto> Users,
        int PageNumber,
        int TotalPages,
        int TotalCount,
        bool HasNextPage
    );

    public class GetAllUsersHandler(ApplicationDbContext dbContext) : IQueryHandler<GetAllUsersQuery, GetAllUsersResult>
    {
        public async Task<ErrorOr<GetAllUsersResult>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber ?? 1;
            int pageSize = request.PageSize ?? 10;

            var query = dbContext.Users
                .AsNoTracking()
                .OrderBy(u => u.Email)
                .Select(u => new UserDto(
                    u.Id.Value,
                    u.Email,
                    u.EmailVerified));

            var pagedUsers = await PagedList<UserDto>.CreateAsync(query, pageNumber, pageSize, cancellationToken);

            return new GetAllUsersResult(
                pagedUsers.Items,
                pagedUsers.PageNumber,
                pagedUsers.TotalPages,
                pagedUsers.TotalCount,
                pagedUsers.HasNextPage);
        }
    }
}
