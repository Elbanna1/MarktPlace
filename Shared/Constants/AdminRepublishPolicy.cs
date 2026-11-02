using Shared.Enums;

namespace Shared.Constants;

public static class AdminRepublishPolicy
{
    public const int WarningDays = 5;

    public static bool Supports(ListingModuleType type) => Enum.IsDefined(type);

    public static bool IsWindowOpen(DateTime? expireAt, DateTime utcNow) =>
        expireAt is { } end && end <= utcNow.AddDays(WarningDays);

    public static bool IsExpiringSoon(DateTime? expireAt, DateTime utcNow) =>
        expireAt is { } end && end > utcNow && end <= utcNow.AddDays(WarningDays);

    public static bool CanRepublish(
        ListingModuleType type, ModerationStatus moderationStatus, DateTime? expireAt, DateTime utcNow) =>
        Supports(type) && moderationStatus == ModerationStatus.Approved && IsWindowOpen(expireAt, utcNow);
}
