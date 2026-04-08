using BookingApp.Server.BuildingBlocks.Domain.ValueObjects;
using MimeKit;

namespace BookingApp.Server.Infrastructure.Email;

public class SmtpEmailSender(IOptions<EmailOptions> options, ILogger<SmtpEmailSender> logger) : IEmailSender
{
    private readonly ILogger<SmtpEmailSender> _logger = logger;

    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(options.Value.SenderName, options.Value.SenderEmail));
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(options.Value.SmtpServer, options.Value.SmtpPort, MailKit.Security.SecureSocketOptions.None);
            await smtp.AuthenticateAsync(options.Value.SmtpUsername, options.Value.SmtpPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", recipient);
        }
    }

    public Task SendVerificationEmailAsync(string recipient, UserId userId, string verificationCode)
    {
        var subject = "Confirm your registration - BookingApp";
        var body = $"Please follow the link to confirm your email address: <a href=\"http://localhost:5000/confirm-email?userId={userId.Value}&code={verificationCode}\">Confirm Email</a>. This link is valid for 10 minutes.";

        return SendEmailAsync(recipient, subject, body);
    }

    public Task SendPasswordResetEmailAsync(string recipient, UserId userId, string resetToken)
    {
        var subject = "Reset your password - BookingApp";
        var body = $"Please follow the link to reset your password: <a href=\"http://localhost:5000/reset-password?userId={userId.Value}&token={resetToken}\">Reset Password</a>. This link is valid for 10 minutes.";
        return SendEmailAsync(recipient, subject, body);
    }
}
