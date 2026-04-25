using BookingApp.Server.BuildingBlocks.Domain.Abstractions;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Bookings.Domain.Bookings
{
    public class Booking : Aggregate<BookingId>
    {
        private readonly List<Guest> _guests = [];
        public IReadOnlyList<Guest> Guests => _guests.AsReadOnly();
        public UserId? UserId { get; private set; }
        public ListingId ListingId { get; private set; }
        public BookingGuest? BookingGuest { get; private set; }
        public DateRange Period {  get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }

        private Booking() { }

        public static Booking Create(List<Guest> guests, DateRange period,
            ListingId listingId, UserId? userId = null)
        {
            if (period.From < DateTimeOffset.UtcNow)
                throw new ArgumentException("Check-in cannot be in the past.");

            var booking = new Booking
            {
                Id = BookingId.Of(Guid.NewGuid()),
                UserId = userId,
                ListingId = listingId,
                Period = period,
                Status = BookingStatus.Draft
            };

            booking.SelectGuests(guests);

            return booking;
        }

        public void ProvideGuestDetails(BookingGuest bookingGuest)
        {
            EnsureDraft();
            BookingGuest = bookingGuest;
        }

        public void SelectGuests(List<Guest> guests)
        {
            EnsureDraft();

            if (guests == null || guests.Count == 0)
                throw new ArgumentException("At least one guest must be provided.");

            _guests.Clear();
            _guests.AddRange(guests);
        }

        public void SelectDates(DateTimeOffset checkIn, DateTimeOffset checkOut)
        {
            EnsureDraft();

            if (checkIn < DateTimeOffset.UtcNow)
                throw new ArgumentException("Check-in cannot be in the past.");

            

            //TODO: add domain event for date change, e.g. to recalculate total price based on new dates and listing's pricing rules
        }

        public void Submit()
        {
            EnsureDraft();

            if (_guests.Count == 0)
                throw new InvalidOperationException("At least one guest must be provided.");

            if (Period.From == default || Period.To == default)
                throw new InvalidOperationException("Check-in and check-out dates must be set.");

            Status = BookingStatus.Pending;
            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(15);
        }

        public void Confirm()
        {
            if (Status != BookingStatus.Pending)
                throw new InvalidOperationException("Invalid state transition.");

            Status = BookingStatus.Confirmed;

            //TODO: add domain event for booking confirmation, e.g. to send notification to the user
        }

        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Booking is already cancelled.");

            if (Status == BookingStatus.Confirmed)
            {
                //TODO: add domain event for booking cancellation, e.g. to handle refundч' logic and penalty fees if applicable
            }

            Status = BookingStatus.Cancelled;
            //TODO: add domain event for booking cancellation, e.g. to send notification to the user
        }

        private void EnsureDraft()
        {
            if (Status != BookingStatus.Draft)
                throw new InvalidOperationException("Cannot update booking in current state.");
        }
    }
}
