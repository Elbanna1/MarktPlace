using Shared.Enums;

namespace Shared.Constants;

public static class AdminRepublishPolicy
{
    public const int WarningDays = 5;

    private static readonly IReadOnlySet<ListingModuleType> Modules = new HashSet<ListingModuleType>
    {
        ListingModuleType.RestaurantAndCafe
    };

    public static IReadOnlyCollection<ListingModuleType> SupportedModules => (IReadOnlyCollection<ListingModuleType>)Modules;

    public static bool Supports(ListingModuleType type) => Modules.Contains(type);

    public static bool IsWindowOpen(DateTime? expireAt, DateTime utcNow) =>
        expireAt is { } end && end <= utcNow.AddDays(WarningDays);

    public static bool IsExpiringSoon(DateTime? expireAt, DateTime utcNow) =>
        expireAt is { } end && end > utcNow && end <= utcNow.AddDays(WarningDays);

    public static bool CanRepublish(
        ListingModuleType type, ModerationStatus moderationStatus, DateTime? expireAt, DateTime utcNow) =>
        Supports(type) && moderationStatus == ModerationStatus.Approved && IsWindowOpen(expireAt, utcNow);
}
