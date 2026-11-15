using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.Features.Bookings.Domain.Bookings;
using BookingApp.Server.Features.Users.Common;

namespace BookingApp.Server.Features.Bookings.GetBooking
{
    public record GetBookingQuery(Guid Id) : IQuery<GetBookingResult>;

    public record GetBookingResult(BookingDto User);

    public record BookingDto(
        Guid Id,
        Guid ListingId,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string AddressLine,
        string Country,
        string City,
        string Nationality,
        DateTimeOffset? ArrivingTime,
        DateTimeOffset From,
        DateTimeOffset To);

    public class GetBookingHandler(ApplicationDbContext dbContext) : IQueryHandler<GetBookingQuery, GetBookingResult>
    {
        public async Task<ErrorOr<GetBookingResult>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Bookings
                .AsNoTracking()
                .Where(b => b.Id == BookingId.Of(request.Id))
                .Select(b => new BookingDto(
                    b.Id.Value,
                    b.ListingId.Value,
                    b.BookingGuest.FirstName,
                    b.BookingGuest.LastName,
                    b.BookingGuest.Email.Value,
                    b.BookingGuest.PhoneNumber.Value,
                    b.BookingGuest.Address.AddressLine,
                    b.BookingGuest.Address.Country,
                    b.BookingGuest.Address.City,
                    b.BookingGuest.Nationality,
                    b.BookingGuest.ArrivingTime,
                    b.Period.From,
                    b.Period.To))
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return Errors.User.UserNotFound;
            }

            return new GetBookingResult(user);
        }
    }
}
