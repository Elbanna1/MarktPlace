using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using ServicesAbstraction;
using Shared.DTOs.Settings;

namespace Services.Settings;

public class PublicSettingsService : IPublicSettingsService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

    private const string CacheKeyPrefix = "public-settings";

    private readonly IAdminSettingsRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILookupCache _cacheVersion;

    public PublicSettingsService(
        IAdminSettingsRepository repository, IMemoryCache cache, ILookupCache cacheVersion)
    {
        _repository = repository;
        _cache = cache;
        _cacheVersion = cacheVersion;
    }

    public async Task<PublicSettingsDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{CacheKeyPrefix}:v{_cacheVersion.Version}";

        if (_cache.TryGetValue(cacheKey, out PublicSettingsDto? cached) && cached is not null)
            return cached;

        var settings = await _repository.GetAsync(cancellationToken);

        var mapped = Map(settings);

        _cache.Set(cacheKey, mapped, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration,

            Size = 1
        });

        return mapped;
    }

    private static PublicSettingsDto Map(PlatformSetting settings) =>
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

            AboutUs = settings.AboutUs,
            TermsAndConditions = settings.TermsAndConditions,
            PrivacyPolicy = settings.PrivacyPolicy,

            MaintenanceMode = settings.MaintenanceMode,
            MaintenanceMessage = settings.MaintenanceMessage
        };
}
