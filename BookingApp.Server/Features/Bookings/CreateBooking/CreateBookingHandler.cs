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
        Guid ListingId,
        UserId? UserId) : ICommand<CreateBookingResult>;

    public record CreateBookingResult(BookingId BookingId);

    public class CreateBookingCommandHandler(ApplicationDbContext context) : ICommandHandler<CreateBookingCommand, CreateBookingResult>
    {
        public async Task<ErrorOr<CreateBookingResult>> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var existingBooking = await context.Bookings
                .AnyAsync(b => b.ListingId.Value == command.ListingId &&
                            b.Status == BookingStatus.Confirmed ||
                            (b.Status == BookingStatus.Pending && b.ExpiresAt > DateTimeOffset.UtcNow) &&   
                            b.Period.From < command.CheckOut &&
                            b.Period.To > command.CheckIn, cancellationToken);

            if (existingBooking)
                return Errors.Booking.AlreadyExists;

            var booking = Booking.Create(
                command.Guests,
                DateRange.Of(command.CheckIn, command.CheckOut),
                ListingId.Of(command.ListingId),
                command.UserId != null ? UserId.Of(command.UserId.Value) : null
            );

            booking.Submit();

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
