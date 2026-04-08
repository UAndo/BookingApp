namespace BookingApp.Server.Features.Users.Common;

public static class Errors
{
    public static class User
    {
        public static Error UserNotFound => Error.NotFound(
            code: "User.UserNotFound",
            description: "The specified user does not exist."
        );

        public static Error ProfileAlreadyExists => Error.Conflict(
            code: "User.ProfileAlreadyExists",
            description: "The user already has a profile."
        );

        public static Error ProfileNotFound => Error.NotFound(
            code: "User.ProfileNotFound",
            description: "The specified profile does not exist."
        );
    }
}