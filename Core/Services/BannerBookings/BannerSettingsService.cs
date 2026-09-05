using Shared.Constants;
using Shared.DTOs.BannerBookings;
using Shared.Enums;
using Shared.Exceptions;
using ServicesAbstraction;

namespace Services.BannerBookings;

public class BannerSettingsService : IBannerSettingsService
{
    private readonly IBannerBookingRepository _repository;
    private readonly IAdminAuditService _audit;

    public BannerSettingsService(IBannerBookingRepository repository, IAdminAuditService audit)
    {
        _repository = repository;
        _audit = audit;
    }

    public async Task<IReadOnlyList<BannerPlacementDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var placements = await _repository.GetPlacementsAsync(activeOnly: false, cancellationToken);

        return placements.Select(BannerPlacementPresenter.ToDto).ToList();
    }

    public async Task<BannerPlacementDto> GetAsync(
        BannerLocation location, CancellationToken cancellationToken = default)
    {
        var placement = await _repository.GetPlacementAsync(location, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("لا توجد إعدادات لهذا المكان الإعلاني.");

        return BannerPlacementPresenter.ToDto(placement);
    }

    public async Task<BannerPlacementDto> UpdateAsync(
        BannerLocation location, UpdateBannerPlacementRequest request, CancellationToken cancellationToken = default)
    {
        var placement = await _repository.GetPlacementAsync(location, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("لا توجد إعدادات لهذا المكان الإعلاني.");

        var previousPrice = placement.Price;
        var previousSlots = placement.MaxSlots;
        var previouslyActive = placement.IsActive;
        var previousDuration = placement.DurationDays;

        var formats = NormalizeFormats(request.AllowedFormats);

        EnsureValidPrice(request.Price);
        EnsureValidSlots(location, request.MaxSlots);
        EnsureValidDimensions(request);
        var maxImageSizeBytes = ResolveMaxImageSize(request.MaxImageSizeMegabytes);
        var durationDays = ResolveDuration(request.DurationDays, placement.DurationDays);

        placement.Price = request.Price;
        placement.DurationDays = durationDays;

        placement.MaxSlots = location == BannerLocation.SubCategoryBanner ? 1 : request.MaxSlots;

        placement.DesktopWidth = request.DesktopWidth;
        placement.DesktopHeight = request.DesktopHeight;
        placement.MobileWidth = request.MobileWidth;
        placement.MobileHeight = request.MobileHeight;
        placement.MaxImageSizeBytes = maxImageSizeBytes;
        placement.AllowedFormats = BannerBookingCatalog.JoinFormats(formats);
        placement.IsActive = request.IsActive;
        placement.DisplayOrder = request.DisplayOrder;
        placement.UpdatedAt = DateTime.UtcNow;

        _repository.UpdatePlacement(placement);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdateBannerPrice, AdminAuditCatalog.Targets.BannerPlacement,
            ((int)location).ToString(),
            $"تعديل إعدادات المكان الإعلاني: {BannerBookingCatalog.GetLocationName(location)}",
            oldValue: Describe(previousPrice, previousSlots, previouslyActive, previousDuration),
            newValue: Describe(placement.Price, placement.MaxSlots, placement.IsActive, placement.DurationDays),
            cancellationToken: cancellationToken);

        return BannerPlacementPresenter.ToDto(placement);
    }

    private static string Describe(decimal price, int slots, bool isActive, int durationDays) =>
        $"السعر: {price:N0} {BannerBookingCatalog.DefaultCurrency} — " +
        $"المدة: {BannerBookingCatalog.FormatDuration(durationDays)} — " +
        $"عدد الأماكن: {slots} — {(isActive ? "متاح" : "غير متاح")}";

    private static void EnsureValidPrice(decimal price)
    {
        if (price < BannerBookingCatalog.MinPrice || price > BannerBookingCatalog.MaxPrice)
            throw new BadRequestException(
                $"السعر يجب أن يكون بين {BannerBookingCatalog.MinPrice} و {BannerBookingCatalog.MaxPrice} " +
                $"{BannerBookingCatalog.DefaultCurrency}.");

        if (decimal.Round(price, 2) != price)
            throw new BadRequestException("السعر يجب ألا يزيد عن رقمين عشريين.");
    }

    private static void EnsureValidSlots(BannerLocation location, int slots)
    {
        if (location == BannerLocation.SubCategoryBanner)
            return;

        if (slots < BannerBookingCatalog.MinSlots || slots > BannerBookingCatalog.MaxSlots)
            throw new BadRequestException(
                $"عدد الأماكن يجب أن يكون بين {BannerBookingCatalog.MinSlots} و {BannerBookingCatalog.MaxSlots}.");
    }

    private static void EnsureValidDimensions(UpdateBannerPlacementRequest request)
    {
        EnsureEdge(request.DesktopWidth, "عرض صورة Desktop");
        EnsureEdge(request.DesktopHeight, "ارتفاع صورة Desktop");
        EnsureEdge(request.MobileWidth, "عرض صورة Mobile");
        EnsureEdge(request.MobileHeight, "ارتفاع صورة Mobile");
    }

    private static void EnsureEdge(int value, string label)
    {
        if (value < BannerBookingCatalog.MinImageEdge || value > BannerBookingCatalog.MaxImageEdge)
            throw new BadRequestException(
                $"{label} يجب أن يكون بين {BannerBookingCatalog.MinImageEdge} و " +
                $"{BannerBookingCatalog.MaxImageEdge} بكسل.");
    }

    private static int ResolveDuration(int requested, int current)
    {
        if (requested == 0)
            return current;

        if (requested < BannerBookingCatalog.MinDurationDays ||
            requested > BannerBookingCatalog.MaxDurationDays)
        {
            throw new BadRequestException(
                $"مدة الإعلان يجب أن تكون بين {BannerBookingCatalog.MinDurationDays} و " +
                $"{BannerBookingCatalog.MaxDurationDays} يوم.");
        }

        return requested;
    }

    private static long ResolveMaxImageSize(int megabytes)
    {
        var bytes = (long)megabytes * 1024 * 1024;

        if (bytes < BannerBookingCatalog.MinConfigurableImageSizeBytes ||
            bytes > BannerBookingCatalog.MaxConfigurableImageSizeBytes)
        {
            var min = BannerImageRules.ToMegabytes(BannerBookingCatalog.MinConfigurableImageSizeBytes);
            var max = BannerImageRules.ToMegabytes(BannerBookingCatalog.MaxConfigurableImageSizeBytes);

            throw new BadRequestException($"الحد الأقصى لحجم الصورة يجب أن يكون بين {min} و {max} ميجابايت.");
        }

        return bytes;
    }

    private static IReadOnlyList<string> NormalizeFormats(IReadOnlyList<string>? requested)
    {
        if (requested is null || requested.Count == 0)
            return BannerBookingCatalog.DefaultAllowedFormats;

        var normalised = requested
            .Where(format => !string.IsNullOrWhiteSpace(format))
            .Select(BannerBookingCatalog.NormalizeFormat)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var measurable = BannerBookingCatalog.SupportedFormats
            .SelectMany(format => format.Extensions)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var rejected = normalised.Where(format => !measurable.Contains(format)).ToArray();

        if (rejected.Length > 0)
            throw new BadRequestException(
                $"الصيغ التالية غير مدعومة للبانرات: {string.Join(", ", rejected)}. " +
                $"الصيغ المدعومة: {BannerBookingCatalog.AllowedFormatNames}.");

        if (normalised.Length == 0)
            throw new BadRequestException("يجب اختيار صيغة واحدة على الأقل.");

        return normalised;
    }
}
