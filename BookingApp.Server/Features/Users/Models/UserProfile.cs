using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Features.Users.Models
{
    public class UserProfile
    {
        public UserId UserId { get; set; }
        public string? FirstName { get; private set; } = default!;
        public string? LastName { get; private set; } = default!;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public Address? Address { get; private set; } = default!;
        public PhoneNumber? PhoneNumber { get; private set; } = default!;
        public DateOnly? DateOfBirth { get; private set; }
        public string? Nationality { get; private set; } = default!;
        public Gender? Gender { get; private set; }
        public string? AvatarUrl { get; private set; } 

        private UserProfile() { }

        public static UserProfile Create(UserId userId, string? firstName, string? lastName, Address? address, PhoneNumber? phoneNumber,
            DateOnly? dateOfBirth, string? nationality, Gender? gender, string? avatarUrl)
        {
            return new UserProfile
            {
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                Nationality = nationality,
                Gender = gender,
                AvatarUrl = avatarUrl
            };
        }

        public void UpdateProfile(string? firstName, string? lastName, Address? address, PhoneNumber? phoneNumber,
            DateOnly? dateOfBirth, string? nationality, Gender? gender, string? avatarUrl)
        {
            if (dateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException("Date of birth cannot be in the future.");
            }

            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Nationality = nationality;
            Gender = gender;
            AvatarUrl = avatarUrl;
        }
    }
}