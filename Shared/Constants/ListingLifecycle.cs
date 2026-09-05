namespace Shared.Constants;

public static class ListingLifecycle
{
    public const int ActiveDurationDays = AdvertisementConstants.ActiveDurationDays;

    public static DateTime EndOf(DateTime startUtc) => startUtc.AddDays(ActiveDurationDays);

    public static int? RemainingDays(DateTime? endUtc, DateTime utcNow)
    {
        if (endUtc is not { } end)
            return null;

        var remaining = (end - utcNow).TotalDays;
        return remaining <= 0 ? 0 : (int)Math.Ceiling(remaining);
    }

    public static bool HasExpired(DateTime? endUtc, DateTime utcNow) =>
        endUtc is { } end && end <= utcNow;
}
