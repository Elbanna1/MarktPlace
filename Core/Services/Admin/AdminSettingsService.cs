using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Advertisements;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.Admin;

public class AdminSettingsService : IAdminSettingsService
{
    private readonly IAdminSettingsRepository _repository;
    private readonly IFileService _fileService;
    private readonly IAdminAuditService _audit;

    private readonly ILookupCache _cache;

    private static readonly HashSet<string> AllowedFolders =
        new(ImageConstants.AllFolders, StringComparer.OrdinalIgnoreCase);

    public AdminSettingsService(
        IAdminSettingsRepository repository, IFileService fileService, IAdminAuditService audit,
        ILookupCache cache)
    {
        _repository = repository;
        _fileService = fileService;
        _audit = audit;
        _cache = cache;
    }

    private static string Describe(PlatformSetting settings) =>
        $"{settings.SiteName} — هاتف: {settings.PhoneNumber ?? "—"} — " +
        $"واتساب: {settings.WhatsAppNumber ?? "—"} — بريد: {settings.Email ?? "—"} — " +
        $"وضع الصيانة: {(settings.MaintenanceMode ? "مفعل" : "معطل")}";

    public async Task<AdminSettingsDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _repository.GetAsync(cancellationToken);

        return Map(settings);
    }

    public async Task<AdminSettingsDto> UpdateAsync(
        string adminUserId, UpdateSettingsRequest request, CancellationToken cancellationToken = default)
    {
        var settings = await _repository.GetAsync(cancellationToken);

        var before = Describe(settings);

        settings.SiteName = request.SiteName.Trim();
        settings.SiteNameEn = Normalize(request.SiteNameEn);
        settings.Description = Normalize(request.Description);

        settings.PhoneNumber = Normalize(request.PhoneNumber);
        settings.WhatsAppNumber = Normalize(request.WhatsAppNumber);
        settings.Email = Normalize(request.Email);
        settings.Address = Normalize(request.Address);

        settings.FacebookUrl = Normalize(request.FacebookUrl);
        settings.InstagramUrl = Normalize(request.InstagramUrl);
        settings.TelegramUrl = Normalize(request.TelegramUrl);
        settings.TwitterUrl = Normalize(request.TwitterUrl);
        settings.YouTubeUrl = Normalize(request.YouTubeUrl);
        settings.TikTokUrl = Normalize(request.TikTokUrl);
        settings.LinkedInUrl = Normalize(request.LinkedInUrl);

        settings.TermsAndConditions = Normalize(request.TermsAndConditions);
        settings.PrivacyPolicy = Normalize(request.PrivacyPolicy);
        settings.AboutUs = Normalize(request.AboutUs);

        settings.MaintenanceMode = request.MaintenanceMode;
        settings.MaintenanceMessage = Normalize(request.MaintenanceMessage);

        settings.UpdatedAt = DateTime.UtcNow;
        settings.UpdatedBy = adminUserId;

        await _repository.SaveChangesAsync(cancellationToken);

        _cache.Invalidate();

        await _audit.LogAsync(
            AdminAuditAction.UpdateSettings, AdminAuditCatalog.Targets.PlatformSettings, null,
            "تعديل إعدادات المنصة",
            oldValue: before, newValue: Describe(settings),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        return Map(settings);
    }

    public Task<AdminSettingsDto> UpdateLogoAsync(
        string adminUserId, UploadImageModel? logo, CancellationToken cancellationToken = default) =>
        ReplaceImageAsync(
            adminUserId,
            logo,
            settings => settings.LogoPath,
            (settings, stored) =>
            {
                settings.LogoUrl = stored?.Url;
                settings.LogoPath = stored?.RelativePath;
            },
            cancellationToken);

    public Task<AdminSettingsDto> UpdateFaviconAsync(
        string adminUserId, UploadImageModel? favicon, CancellationToken cancellationToken = default) =>
        ReplaceImageAsync(
            adminUserId,
            favicon,
            settings => settings.FaviconPath,
            (settings, stored) =>
            {
                settings.FaviconUrl = stored?.Url;
                settings.FaviconPath = stored?.RelativePath;
            },
            cancellationToken);

    public async Task<AdminUploadResultDto> UploadAsync(
        UploadImageModel? file, string folder, AdminUploadKind kind,
        CancellationToken cancellationToken = default)
    {
        if (file is null)
            throw new BadRequestException("الملف مطلوب.");

        var target = (folder ?? string.Empty).Trim();

        if (!AllowedFolders.Contains(target))
            throw new BadRequestException(
                "مجلد الرفع غير معروف. اختر أحد المجلدات المعتمدة في المنصة.");

        var stored = kind switch
        {
            AdminUploadKind.Document => await _fileService.SaveDocumentAsync(file, target, cancellationToken),
            AdminUploadKind.Video => await _fileService.SaveVideoAsync(file, target, cancellationToken),
            _ => await _fileService.SaveAsync(file, target, cancellationToken)
        };

        return new AdminUploadResultDto
        {
            Url = stored.Url,
            FileName = stored.FileName,
            Path = stored.RelativePath,
            Size = file.Length,
            Folder = target
        };
    }

    public AdminUploadLimitsDto GetUploadLimits() => BuildLimits();

    private async Task<AdminSettingsDto> ReplaceImageAsync(
        string adminUserId,
        UploadImageModel? image,
        Func<PlatformSetting, string?> readPreviousPath,
        Action<PlatformSetting, StoredFile?> apply,
        CancellationToken cancellationToken)
    {
        if (image is null)
            throw new BadRequestException("الصورة مطلوبة.");

        var settings = await _repository.GetAsync(cancellationToken);

        var stored = await _fileService.SaveAsync(image, ImageConstants.PlatformFolder, cancellationToken);
        var previousPath = readPreviousPath(settings);

        apply(settings, stored);

        settings.UpdatedAt = DateTime.UtcNow;
        settings.UpdatedBy = adminUserId;

        try
        {
            await _repository.SaveChangesAsync(cancellationToken);

            _cache.Invalidate();

            await _audit.LogAsync(
                AdminAuditAction.UpdateLogo, AdminAuditCatalog.Targets.PlatformSettings, null,
                "تعديل صور هوية المنصة",
                oldValue: previousPath, newValue: stored.RelativePath,
                adminUserId: adminUserId, cancellationToken: cancellationToken);
        }
        catch
        {
            _fileService.Delete(stored.RelativePath);
            throw;
        }

        if (!string.IsNullOrWhiteSpace(previousPath))
            _fileService.Delete(previousPath);

        return Map(settings);
    }

    private static AdminUploadLimitsDto BuildLimits() =>
        new()
        {
            ImageMaxSizeMb = ToMegabytes(ImageConstants.MaxFileSizeBytes),
            MaxImagesPerItem = ImageConstants.MaxImagesPerItem,
            ImageExtensions = StripDots(ImageConstants.AllowedExtensions),

            DocumentMaxSizeMb = ToMegabytes(FileUploadConstants.MaxDocumentSizeBytes),
            DocumentExtensions = StripDots(DocumentFormatCatalog.AllExtensions),

            VideoMaxSizeMb = ToMegabytes(FileUploadConstants.MaxVideoSizeBytes),
            VideoExtensions = StripDots(VideoFormatCatalog.AllExtensions),

            MaxRequestBodySizeMb = ToMegabytes(FileUploadConstants.MaxRequestBodySizeBytes)
        };

    private static int ToMegabytes(long bytes) => (int)(bytes / (1024 * 1024));

    private static IReadOnlyList<string> StripDots(IReadOnlyList<string> extensions) =>
        extensions.Select(extension => extension.TrimStart('.')).ToList();

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static AdminSettingsDto Map(PlatformSetting settings) =>
        new()
        {
            SiteName = settings.SiteName,
            SiteNameEn = settings.SiteNameEn,
            Description = settings.Description,
            LogoUrl = settings.LogoUrl,
            FaviconUrl = settings.FaviconUrl,
            PhoneNumber = settings.PhoneNumber,
            WhatsAppNumber = settings.WhatsAppNumber,
            Email = settings.Email,
            Address = settings.Address,
            FacebookUrl = settings.FacebookUrl,
            InstagramUrl = settings.InstagramUrl,
            TelegramUrl = settings.TelegramUrl,
            TwitterUrl = settings.TwitterUrl,
            YouTubeUrl = settings.YouTubeUrl,
            TikTokUrl = settings.TikTokUrl,
            LinkedInUrl = settings.LinkedInUrl,
            TermsAndConditions = settings.TermsAndConditions,
            PrivacyPolicy = settings.PrivacyPolicy,
            AboutUs = settings.AboutUs,
            MaintenanceMode = settings.MaintenanceMode,
            MaintenanceMessage = settings.MaintenanceMessage,
            UpdatedAt = settings.UpdatedAt,
            UploadLimits = BuildLimits()
        };
}
