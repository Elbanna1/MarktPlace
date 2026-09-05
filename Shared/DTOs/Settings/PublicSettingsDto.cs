namespace Shared.DTOs.Settings;

public class PublicSettingsDto
{
    public string SiteName { get; set; } = default!;

    public string? SiteNameEn { get; set; }

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public string? FaviconUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public string? WhatsAppNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? FacebookUrl { get; set; }

    public string? InstagramUrl { get; set; }

    public string? TelegramUrl { get; set; }

    public string? TwitterUrl { get; set; }

    public string? YouTubeUrl { get; set; }

    public string? TikTokUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? AboutUs { get; set; }

    public string? TermsAndConditions { get; set; }

    public string? PrivacyPolicy { get; set; }

    public bool MaintenanceMode { get; set; }

    public string? MaintenanceMessage { get; set; }
}
