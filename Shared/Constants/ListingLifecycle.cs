namespace Shared.Constants;

public static class ListingLifecycle
{
    public const int ActiveDurationDays = AdvertisementConstants.ActiveDurationDays;

    private static readonly TimeZoneInfo? DisplayTimeZone = FindDisplayTimeZone();

    public static DateTime EndOf(DateTime startUtc) => startUtc.AddDays(ActiveDurationDays);

    public static DateTime ToDisplayTime(DateTime utc) =>
        DisplayTimeZone is null
            ? utc
            : TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), DisplayTimeZone);

    private static TimeZoneInfo? FindDisplayTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo");
        }
        catch (Exception exception) when (exception is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            return null;
        }
    }

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
