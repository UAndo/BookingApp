using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Users.Common;
using BookingApp.Server.Features.Users.Models;

namespace BookingApp.Server.Features.Users.GetUser
{
    public record GetUserQuery(Guid UserId) : IQuery<GetUserResult>;

    public record GetUserResult(UserDto User);

    public class GetUserHandler(ApplicationDbContext dbContext) : IQueryHandler<GetUserQuery, GetUserResult>
    {
        public async Task<ErrorOr<GetUserResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == UserId.Of(request.UserId))
                .Select(u => new UserDto(
                    u.Id.Value,
                    u.Email,
                    u.EmailVerified))
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return Errors.User.UserNotFound;
            }

            return new GetUserResult(user);
        }
    }
}
