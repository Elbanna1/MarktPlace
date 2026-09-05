using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ServicesAbstraction;
using Shared.Settings;

namespace Persistence.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.DisplayName, _settings.Email));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = htmlBody };
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

    public Task SendPasswordResetOtpAsync(string toEmail, string userName, string otp, int expiryMinutes, CancellationToken cancellationToken = default)
    {
        const string subject = "كود إعادة تعيين كلمة السر - ماركت بليس";

        var htmlBody = $@"
<div dir=""rtl"" style=""font-family: Tahoma, Arial, Helvetica, sans-serif; font-size: 15px; color: #222; text-align: right;"">
    <p>أهلاً {System.Net.WebUtility.HtmlEncode(userName)} 👋</p>
    <p>وصلنا طلب لإعادة تعيين كلمة السر بتاعت حسابك على ماركت بليس. ده الكود:</p>
    <p style=""font-size: 28px; font-weight: bold; letter-spacing: 4px; color: #1a73e8; direction: ltr; text-align: center;"">{otp}</p>
    <p>الكود ده صالح لمدة {expiryMinutes} دقيقة بس.</p>
    <hr />
    <p style=""font-size: 12px; color: #888;"">لو مش إنت اللي طلبت ده، تجاهل الرسالة دي وكلمة السر بتاعتك هتفضل زي ما هي.</p>
</div>";

        return SendAsync(toEmail, subject, htmlBody, cancellationToken);
    }
}
