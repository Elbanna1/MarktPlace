namespace Shared.Settings;

public class EmailSettings
{
    public const string SectionName = "EmailSettings";

    public string SmtpServer { get; set; } = default!;

    public int Port { get; set; }

    public string Email { get; set; } = default!;

    public string DisplayName { get; set; } = "Shopiklopik";

    public string? LogoUrl { get; set; }

    public string Password { get; set; } = default!;

    public bool EnableSsl { get; set; }
}
