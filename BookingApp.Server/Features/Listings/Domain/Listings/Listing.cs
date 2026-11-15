using BookingApp.Server.BuildingBlocks.Domain.Abstractions;
using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Listings.Domain.Listings
{
    public class Listing : Aggregate<ListingId>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal PricePerNight { get; private set; }
        public int MaxGuests { get; private set; }
        public ListingStatus Status { get; private set; }
        public UserId OwnerId { get; private set; }
        public Address Address { get; private set; }

        private Listing() { }

        public static Listing Create(string title, string description, decimal pricePerNight,
            int maxGuests, UserId ownerId, Address address, UserId? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");
            if (pricePerNight <= 0)
                throw new ArgumentException("Price per night must be greater than zero.");
            if (maxGuests <= 0)
                throw new ArgumentException("Max guests must be greater than zero.");

            return new Listing
            {
                Id = ListingId.Of(Guid.NewGuid()),
                Title = title,
                Description = description,
                PricePerNight = pricePerNight,
                MaxGuests = maxGuests,
                Status = ListingStatus.Active,
                OwnerId = ownerId,
                Address = address,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = createdBy,
                LastModified = DateTimeOffset.UtcNow,
                LastModifiedBy = createdBy
            };
        }

        public void Update(string title, string description, decimal pricePerNight,
            int maxGuests, Address address, UserId? lastModifiedBy = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");
            if (pricePerNight <= 0)
                throw new ArgumentException("Price per night must be greater than zero.");
            if (maxGuests <= 0)
                throw new ArgumentException("Max guests must be greater than zero.");

            Title = title;
            Description = description;
            PricePerNight = pricePerNight;
            MaxGuests = maxGuests;
            Address = address;
            Touch(lastModifiedBy);
        }

        public void ChangeStatus(ListingStatus newStatus, UserId? lastModifiedBy = null)
        {
            if (Status == newStatus) return;

            if (Status == ListingStatus.Archived)
                throw new InvalidOperationException("Cannot modify an archived listing.");

            Status = newStatus;
            Touch(lastModifiedBy);
        }

        private void Touch(UserId? lastModifiedBy)
        {
            LastModified = DateTimeOffset.UtcNow;
            LastModifiedBy = lastModifiedBy;
        }
    }
}
