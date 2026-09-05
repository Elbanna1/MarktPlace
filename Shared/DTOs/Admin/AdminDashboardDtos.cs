using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminDashboardDto
{
    public AdminDashboardStatsDto Stats { get; set; } = new();

    public IReadOnlyList<AdminAdListItemDto> LatestAds { get; set; } = Array.Empty<AdminAdListItemDto>();

    public IReadOnlyList<AdminDashboardBannerRequestDto> LatestBannerRequests { get; set; } =
        Array.Empty<AdminDashboardBannerRequestDto>();

    public IReadOnlyList<AdminDashboardPaymentDto> LatestPayments { get; set; } =
        Array.Empty<AdminDashboardPaymentDto>();

    public IReadOnlyList<AdminQuickActionDto> QuickActions { get; set; } =
        Array.Empty<AdminQuickActionDto>();

    public IReadOnlyList<AdminTopReferrerDto> TopReferrers { get; set; } =
        Array.Empty<AdminTopReferrerDto>();

    public DateTime GeneratedAt { get; set; }
}

public class AdminDashboardStatsDto
{
    public int UsersCount { get; set; }

    public int ActiveUsersCount { get; set; }

    public int SuspendedUsersCount { get; set; }

    public int AdsCount { get; set; }

    public int ActiveAdsCount { get; set; }

    public int PendingAdsCount { get; set; }

    public int RejectedAdsCount { get; set; }

    public int ActiveBannersCount { get; set; }

    public int PendingBannerRequestsCount { get; set; }

    public int PendingPaymentsCount { get; set; }

    public int PendingReportsCount { get; set; }

    public int SuspendedAdsCount { get; set; }

    public decimal TotalRevenue { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public decimal BannerRevenue { get; set; }

    public string Currency { get; set; } = Shared.Constants.PaymentCatalog.DefaultCurrency;

    public int ReferralsCount { get; set; }

    public int CompletedReferralsCount { get; set; }

    public int PendingReferralsCount { get; set; }
}

public class AdminDashboardBannerRequestDto
{
    public Guid Id { get; set; }

    public string AdvertiserName { get; set; } = default!;

    public string Title { get; set; } = default!;

    public BannerLocation Location { get; set; }

    public string LocationName { get; set; } = default!;

    public int SlotNumber { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = default!;

    public BannerPaymentStatus PaymentStatus { get; set; }

    public string PaymentStatusName { get; set; } = default!;

    public BannerBookingStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime SubmittedAt { get; set; }
}

public class AdminDashboardPaymentDto
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;

    public string? UserName { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = default!;

    public string PaymentMethodName { get; set; } = default!;

    public PaymentStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime SubmittedAt { get; set; }
}
