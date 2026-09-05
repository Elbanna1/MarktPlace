using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Admin;

public class AdminSettingsDto
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

    public string? TermsAndConditions { get; set; }

    public string? PrivacyPolicy { get; set; }

    public string? AboutUs { get; set; }

    public bool MaintenanceMode { get; set; }

    public string? MaintenanceMessage { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public AdminUploadLimitsDto UploadLimits { get; set; } = new();
}

public class UpdateSettingsRequest
{
    [Required(ErrorMessage = "اسم الموقع مطلوب.")]
    [MaxLength(150, ErrorMessage = "لا يمكن أن يتجاوز اسم الموقع 150 حرفًا.")]
    public string SiteName { get; set; } = default!;

    [MaxLength(150)]
    public string? SiteNameEn { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(30)]
    public string? PhoneNumber { get; set; }

    [MaxLength(30)]
    public string? WhatsAppNumber { get; set; }

    [MaxLength(256)]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح.")]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(500)]
    [Url(ErrorMessage = "رابط فيسبوك غير صحيح.")]
    public string? FacebookUrl { get; set; }

    [MaxLength(500)]
    [Url(ErrorMessage = "رابط إنستجرام غير صحيح.")]
    public string? InstagramUrl { get; set; }

    [MaxLength(500)]
    [Url(ErrorMessage = "رابط تيليجرام غير صحيح.")]
    public string? TelegramUrl { get; set; }

    [MaxLength(500)]
    [Url(ErrorMessage = "الرابط غير صحيح.")]
    public string? TwitterUrl { get; set; }

    [MaxLength(500)]
    [Url(ErrorMessage = "الرابط غير صحيح.")]
    public string? YouTubeUrl { get; set; }

    [MaxLength(500)]
    [Url(ErrorMessage = "الرابط غير صحيح.")]
    public string? TikTokUrl { get; set; }

    [MaxLength(500)]
    [Url(ErrorMessage = "الرابط غير صحيح.")]
    public string? LinkedInUrl { get; set; }

    public string? TermsAndConditions { get; set; }

    public string? PrivacyPolicy { get; set; }

    public string? AboutUs { get; set; }

    public bool MaintenanceMode { get; set; }

    [MaxLength(1000)]
    public string? MaintenanceMessage { get; set; }
}

public class AdminUploadLimitsDto
{
    public int ImageMaxSizeMb { get; set; }

    public int MaxImagesPerItem { get; set; }

    public IReadOnlyList<string> ImageExtensions { get; set; } = Array.Empty<string>();

    public int DocumentMaxSizeMb { get; set; }

    public IReadOnlyList<string> DocumentExtensions { get; set; } = Array.Empty<string>();

    public int VideoMaxSizeMb { get; set; }

    public IReadOnlyList<string> VideoExtensions { get; set; } = Array.Empty<string>();

    public int MaxRequestBodySizeMb { get; set; }
}

public class AdminUploadResultDto
{
    public string Url { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string Path { get; set; } = default!;

    public long Size { get; set; }

    public string Folder { get; set; } = default!;
}
