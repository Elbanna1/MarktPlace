using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Settings;

namespace Persistence.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default) =>
        await SendAsync(toEmail, subject, htmlBody, null, cancellationToken);

    public async Task SendAsync(string toEmail, string subject, string htmlBody, string? textBody, CancellationToken cancellationToken = default)
    {
        var displayName = string.IsNullOrWhiteSpace(_settings.DisplayName)
            ? EmailCatalog.Brand.Name
            : _settings.DisplayName;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(displayName, _settings.Email));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = htmlBody };

        if (!string.IsNullOrWhiteSpace(textBody))
            builder.TextBody = textBody;

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();

        var secureOption = _settings.EnableSsl
            ? (_settings.Port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls)
            : SecureSocketOptions.None;

        await client.ConnectAsync(_settings.SmtpServer, _settings.Port, secureOption, cancellationToken);
        await client.AuthenticateAsync(_settings.Email, _settings.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    public Task SendPasswordResetOtpAsync(string toEmail, string userName, string otp, int expiryMinutes, CancellationToken cancellationToken = default) =>
        SendAsync(
            toEmail,
            EmailCatalog.PasswordReset.Subject,
            EmailCatalog.BuildPasswordResetOtp(userName, otp, expiryMinutes, _settings.LogoUrl),
            EmailCatalog.BuildPasswordResetOtpText(userName, otp, expiryMinutes),
            cancellationToken);
}
