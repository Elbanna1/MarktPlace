using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminAnalyticsDto
{
    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public IReadOnlyList<AdminTimeSeriesPointDto> AdsOverTime { get; set; } =
        Array.Empty<AdminTimeSeriesPointDto>();

    public IReadOnlyList<AdminTimeSeriesPointDto> UsersOverTime { get; set; } =
        Array.Empty<AdminTimeSeriesPointDto>();

    public IReadOnlyList<AdminRevenuePointDto> RevenueOverTime { get; set; } =
        Array.Empty<AdminRevenuePointDto>();

    public IReadOnlyList<AdminCategoryCountDto> AdsByCategory { get; set; } =
        Array.Empty<AdminCategoryCountDto>();

    public IReadOnlyList<AdminCategoryCountDto> TopCategories { get; set; } =
        Array.Empty<AdminCategoryCountDto>();

    public AdminViewsAnalyticsDto Views { get; set; } = new();
}

public class AdminTimeSeriesPointDto
{
    public DateTime Date { get; set; }

    public int Count { get; set; }
}

public class AdminRevenuePointDto
{
    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public decimal BannerAmount { get; set; }
}

public class AdminCategoryCountDto
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int Count { get; set; }

    public double Percentage { get; set; }
}

public class AdminViewsAnalyticsDto
{
    public long TotalViews { get; set; }

    public IReadOnlyList<AdminTopListingDto> TopListings { get; set; } =
        Array.Empty<AdminTopListingDto>();
}

public class AdminTopListingDto
{
    public Guid ListingId { get; set; }

    public ListingModuleType ListingType { get; set; }

    public string ListingTypeName { get; set; } = default!;

    public int Views { get; set; }
}

public class AdminQuickActionDto
{
    public string Key { get; set; } = default!;

    public string Label { get; set; } = default!;

    public int Count { get; set; }

    public string Route { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public bool IsUrgent { get; set; }
}

public class AdminModerationOverviewDto
{
    public int PendingAds { get; set; }

    public int PendingReports { get; set; }

    public int PendingPayments { get; set; }

    public int PendingBanners { get; set; }

    public int SuspendedAds { get; set; }

    public int TotalPending { get; set; }

    public IReadOnlyList<AdminAdListItemDto> RecentPendingAds { get; set; } =
        Array.Empty<AdminAdListItemDto>();

    public IReadOnlyList<Listings.ListingReportDto> RecentReports { get; set; } =
        Array.Empty<Listings.ListingReportDto>();

    public IReadOnlyList<AdminDashboardBannerRequestDto> RecentBannerRequests { get; set; } =
        Array.Empty<AdminDashboardBannerRequestDto>();

    public IReadOnlyList<AdminDashboardPaymentDto> RecentPayments { get; set; } =
        Array.Empty<AdminDashboardPaymentDto>();
}

public class AdminBannerCenterDto
{
    public AdminBannerCenterSummaryDto Summary { get; set; } = new();

    public IReadOnlyList<AdminBannerPlacementOverviewDto> Placements { get; set; } =
        Array.Empty<AdminBannerPlacementOverviewDto>();

    public IReadOnlyList<AdminDashboardBannerRequestDto> ExpiringSoon { get; set; } =
        Array.Empty<AdminDashboardBannerRequestDto>();
}

public class AdminBannerCenterSummaryDto
{
    public int PendingRequests { get; set; }

    public int PaymentPending { get; set; }

    public int PaymentApproved { get; set; }

    public int Active { get; set; }

    public int ExpiringSoon { get; set; }

    public int Expired { get; set; }

    public int Rejected { get; set; }

    public int Cancelled { get; set; }

    public decimal TotalRevenue { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public string Currency { get; set; } = Shared.Constants.PaymentCatalog.DefaultCurrency;
}

public class AdminBannerPlacementOverviewDto
{
    public BannerLocation Location { get; set; }

    public string LocationName { get; set; } = default!;

    public decimal Price { get; set; }

    public string Currency { get; set; } = default!;

    public int DurationDays { get; set; }

    public bool IsActive { get; set; }

    public IReadOnlyList<AdminBannerSlotOverviewDto> Slots { get; set; } =
        Array.Empty<AdminBannerSlotOverviewDto>();
}

public class AdminBannerSlotOverviewDto
{
    public int SlotNumber { get; set; }

    public string SlotName { get; set; } = default!;

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? SubCategoryId { get; set; }

    public string? SubCategoryName { get; set; }

    public string Status { get; set; } = default!;

    public string StatusName { get; set; } = default!;

    public bool IsAvailable { get; set; }

    public Guid? BookingId { get; set; }

    public string? BookingTitle { get; set; }

    public string? AdvertiserName { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? AvailableFrom { get; set; }
}
