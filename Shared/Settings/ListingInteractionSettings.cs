using Shared.Constants;

namespace Shared.Settings;

public class ListingInteractionSettings
{
    public const string SectionName = "ListingInteractions";

    private int _viewDedupeWindowMinutes = ListingInteractionCatalog.DefaultViewDedupeWindowMinutes;
    private int _recentlyViewedRetentionCount = ListingInteractionCatalog.DefaultRecentlyViewedRetentionCount;

    public int ViewDedupeWindowMinutes
    {
        get => _viewDedupeWindowMinutes;
        set => _viewDedupeWindowMinutes =
            value > 0 ? value : ListingInteractionCatalog.DefaultViewDedupeWindowMinutes;
    }

    public int RecentlyViewedRetentionCount
    {
        get => _recentlyViewedRetentionCount;
        set => _recentlyViewedRetentionCount =
            value > 0 ? value : ListingInteractionCatalog.DefaultRecentlyViewedRetentionCount;
    }

    public TimeSpan ViewDedupeWindow => TimeSpan.FromMinutes(ViewDedupeWindowMinutes);
}
