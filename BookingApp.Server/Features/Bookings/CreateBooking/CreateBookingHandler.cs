using BookingApp.Server.BuildingBlocks.CQRS;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using BookingApp.Server.Features.Bookings.Domain.Bookings;
using BookingApp.Server.Features.Bookings.Domain.Errors;

namespace BookingApp.Server.Features.Bookings.CreateBooking
{
    public record CreateBookingCommand(
        List<Guest> Guests,
        DateTimeOffset CheckIn,
        DateTimeOffset CheckOut,
        ListingId ListingId,
        UserId? UserId) : ICommand<CreateBookingResult>;

    public record CreateBookingResult(BookingId BookingId);

    public class CreateBookingCommandHandler(
        ApplicationDbContext context,
        TimeProvider timeProvider) : ICommandHandler<CreateBookingCommand, CreateBookingResult>
    {
        public async Task<ErrorOr<CreateBookingResult>> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var existingBooking = await context.Bookings
                .AnyAsync(b => b.ListingId == command.ListingId
                               && (b.Status == BookingStatus.Draft
                               || b.Status == BookingStatus.PendingPayment
                               || b.Status == BookingStatus.Confirmed), cancellationToken);

            if (existingBooking)
                return Errors.Booking.AlreadyExists;

            var booking = Booking.Create(
                command.Guests,
                DateRange.Of(command.CheckIn, command.CheckOut),
                command.ListingId,
                timeProvider,
                command.UserId
            );

            context.Bookings.Add(booking);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return Errors.Booking.Conflict;
            }

            return new CreateBookingResult(booking.Id);
        }
    }
}
