using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Users.Common;
using BookingApp.Server.Features.Users.Models;

namespace BookingApp.Server.Features.Users.GetUserProfile
{
    public record GetUserProfileQuery(Guid UserId) : IQuery<GetUserProfileResult>;

    public record GetUserProfileResult(UserProfile User);

    public class GetUserProfileHandler(ApplicationDbContext dbContext) : IQueryHandler<GetUserProfileQuery, GetUserProfileResult>
    {
        public async Task<ErrorOr<GetUserProfileResult>> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
        {
            var user = await dbContext.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId.Value == query.UserId, cancellationToken);

            if (user == null)
            {
                return Errors.User.UserNotFound;
            }

            return new GetUserProfileResult(user);
        }
    }
}
