namespace ServicesAbstraction;

public interface IEmailService
{
    Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default);

    Task SendPasswordResetOtpAsync(string toEmail, string userName, string otp, int expiryMinutes, CancellationToken cancellationToken = default);
}
