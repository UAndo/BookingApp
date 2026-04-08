using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;

namespace BookingApp.Server.Infrastructure.Abstractions;

public interface IEmailSender
{
    Task SendEmailAsync(string recipient, string subject, string body);
    Task SendVerificationEmailAsync(string recipient, UserId userId, string verificationToken);
    Task SendPasswordResetEmailAsync(string recipient, UserId userId, string resetToken);
}
