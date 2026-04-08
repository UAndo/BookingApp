namespace BookingApp.Server.Features.Auth.Domain.Errors;

public static class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Unauthorized(
            code: "User.InvalidCredentials",
            description: "Invalid email or password."
        );

        public static Error BotsNotAllowed => Error.Forbidden(
            code: "Authentication.BotsNotAllowed",
            description: "Bots are not allowed to log in."
        );

        public static Error EmailNotConfirmed => Error.Forbidden(
            code: "Authentication.EmailNotConfirmed",
            description: "Email address is not confirmed."
        );

        public static Error AccountLocked => Error.Forbidden(
            code: "Authentication.AccountLocked",
            description: "The account is locked due to multiple failed login attempts."
        );

        public static Error InvalidVerificationToken => Error.Validation(
            code: "Authentication.InvalidVerificationCode",
            description: "The verification code is invalid."
        );

        public static Error InvalidPasswordResetToken => Error.Validation(
            code: "Authentication.InvalidPasswordResetCode",
            description: "The password reset code is invalid."
        );

        public static Error VerificationTokenExpired => Error.Validation(
            code: "Authentication.VerificationCodeExpired",
            description: "The verification code has expired."
        );

        public static Error UserNotFound => Error.NotFound(
            code: "Authentication.UserNotFound",
            description: "The specified user does not exist."
        );

        public static Error EmailAlreadyVerified => Error.Conflict(
            code: "Authentication.EmailIsAlreadyVerified",
            description: "The email address has already been verified."
        );

        public static Error UserAlreadyExists => Error.Conflict(
            code: "Authentication.UserAlreadyExists",
            description: "A user with the given email already exists."
        );

        public static Error InvalidRefreshToken => Error.Unauthorized(
            code: "Authentication.InvalidRefreshToken",
            description: "The provided refresh token is invalid."
        );
    }
}