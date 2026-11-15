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
            ListingId listingId, TimeProvider timeProvider, UserId? userId = null)
        {
            if (guests == null || guests.Count == 0)
                throw new InvalidOperationException("At least one guest must be provided.");

            var booking = new Booking
            {
                Id = BookingId.Of(Guid.NewGuid()),
                UserId = userId,
                ListingId = listingId,
                Period = period,
                Status = BookingStatus.Draft,
                ExpiresAt = timeProvider.GetUtcNow().AddMinutes(15),
                CreatedAt = timeProvider.GetUtcNow(),
                CreatedBy = userId,
                LastModified = timeProvider.GetUtcNow(),
                LastModifiedBy = userId
            };

            booking._guests.AddRange(guests);

            return booking;
        }

        public void SetMainGuestDetails(BookingGuest bookingGuest, TimeProvider timeProvider, UserId? userId = null)
        {
            if (Status != BookingStatus.Draft && Status != BookingStatus.PendingPayment)
                throw new InvalidOperationException("Cannot update booking in current state.");

            BookingGuest = bookingGuest ?? 
                throw new ArgumentNullException(nameof(bookingGuest), "Booking guest details must be provided.");

            Status = BookingStatus.PendingPayment;
            ExpiresAt = timeProvider.GetUtcNow().AddMinutes(15);
            Touch(timeProvider, userId);
        }

        public void Cancel(TimeProvider timeProvider, UserId? userId = null)
        {
            if (Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Booking is already cancelled.");

            if (Status == BookingStatus.Confirmed)
            {
                //TODO: add domain event for booking cancellation, e.g. to handle refund logic and penalty fees if applicable
            }

            Status = BookingStatus.Cancelled;
            Touch(timeProvider, userId);
            //TODO: add domain event for booking cancellation, e.g. to send notification to the user
        }

        public void Confirm(TimeProvider timeProvider, UserId? userId = null)
        {
            if (Status != BookingStatus.PendingPayment)
                throw new InvalidOperationException("Only bookings pending payment can be confirmed.");

            Status = BookingStatus.Confirmed;
            Touch(timeProvider, userId);
        }

        public void MarkAsExpired(TimeProvider timeProvider, UserId? userId = null)
        {
            if (Status != BookingStatus.Draft && Status != BookingStatus.PendingPayment)
                throw new InvalidOperationException("Only bookings pending payment or draft can be marked as expired.");
            Status = BookingStatus.Expired;
            Touch(timeProvider, userId);
        }

        public void UpdateGuests(List<Guest> guests, TimeProvider timeProvider, UserId? userId = null)
        {
            if (Status != BookingStatus.Draft)
                throw new InvalidOperationException("Cannot update guests for booking in current state.");
            if (guests == null || guests.Count == 0)
                throw new InvalidOperationException("At least one guest must be provided.");
            _guests.Clear();
            _guests.AddRange(guests);
            Touch(timeProvider, userId);
        }

        private void Touch(TimeProvider timeProvider, UserId? userId)
        {
            LastModified = timeProvider.GetUtcNow();
            LastModifiedBy = userId;
        }
    }
}