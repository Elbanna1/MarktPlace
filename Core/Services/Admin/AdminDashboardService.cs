using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.BannerBookings;
using Shared.Enums;

namespace Services.Admin;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IAdminDashboardRepository _dashboard;
    private readonly IAdminAdRepository _ads;
    private readonly IAdminAdService _adService;
    private readonly IBannerSettingsService _settings;
    private readonly ILookupService _lookups;

    private readonly IAdminReferralService _referrals;

    private const int DefaultWindowDays = 30;

    private const int MaxWindowDays = 365;

    private const int ExpiringSoonDays = 7;

    private const int TopCount = 5;

    public AdminDashboardService(
        IAdminDashboardRepository dashboard,
        IAdminAdRepository ads,
        IAdminAdService adService,
        IBannerSettingsService settings,
        ILookupService lookups,
        IAdminReferralService referrals)
    {
        _dashboard = dashboard;
        _ads = ads;
        _adService = adService;
        _settings = settings;
        _lookups = lookups;
        _referrals = referrals;
    }

    public async Task<AdminDashboardDto> GetAsync(
        int latestCount = 5, CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;
        var monthStart = new DateTime(utcNow.Year, utcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        latestCount = Math.Clamp(latestCount, 1, 20);

        var (usersTotal, usersSuspended) = await _dashboard.GetUserCountsAsync(cancellationToken);
        var (activeBanners, pendingBanners) = await _dashboard.GetBannerCountsAsync(utcNow, cancellationToken);
        var pendingPayments = await _dashboard.GetPendingPaymentsCountAsync(cancellationToken);
        var pendingReports = await _dashboard.GetPendingReportsCountAsync(cancellationToken);
        var (totalRevenue, monthlyRevenue) = await _dashboard.GetRevenueAsync(monthStart, cancellationToken);

        var adsByStatus = await _ads.GetStatusBreakdownAsync(utcNow, cancellationToken);

        var adsTotal = adsByStatus.Sum(row => row.Count);
        var adsActive = adsByStatus.Where(row => row.Status == ListingStatus.Active).Sum(row => row.Count);
        var adsPending = adsByStatus.Where(row => row.ModerationStatus == ModerationStatus.Pending).Sum(row => row.Count);
        var adsRejected = adsByStatus.Where(row => row.ModerationStatus == ModerationStatus.Rejected).Sum(row => row.Count);
        var suspendedAds = adsByStatus.Where(row => row.ModerationStatus == ModerationStatus.Suspended).Sum(row => row.Count);

        var latestAds = await _adService.GetLatestAdsAsync(latestCount, cancellationToken);

        var latestBanners = await _dashboard.GetLatestBannerRequestsAsync(latestCount, cancellationToken);
        var latestPayments = await _dashboard.GetLatestPaymentsAsync(latestCount, cancellationToken);

        var (bannerRevenue, _) = await _dashboard.GetBannerRevenueAsync(monthStart, cancellationToken);

        var referrals = await _referrals.GetStatisticsAsync(TopCount, cancellationToken);

        var stats = new AdminDashboardStatsDto
        {
            UsersCount = usersTotal,
            ActiveUsersCount = usersTotal - usersSuspended,
            SuspendedUsersCount = usersSuspended,
            AdsCount = adsTotal,
            ActiveAdsCount = adsActive,
            PendingAdsCount = adsPending,
            RejectedAdsCount = adsRejected,
            ActiveBannersCount = activeBanners,
            PendingBannerRequestsCount = pendingBanners,
            PendingPaymentsCount = pendingPayments,
            PendingReportsCount = pendingReports,
            SuspendedAdsCount = suspendedAds,
            BannerRevenue = bannerRevenue,
            TotalRevenue = totalRevenue,
            MonthlyRevenue = monthlyRevenue,
            Currency = PaymentCatalog.DefaultCurrency,
            ReferralsCount = referrals.TotalReferrals,
            CompletedReferralsCount = referrals.CompletedReferrals,
            PendingReferralsCount = referrals.PendingReferrals
        };

        return new AdminDashboardDto
        {
            Stats = stats,
            QuickActions = BuildQuickActions(stats),
            LatestAds = latestAds,
            LatestBannerRequests = latestBanners,
            LatestPayments = latestPayments,
            TopReferrers = referrals.TopReferrers,
            GeneratedAt = utcNow
        };
    }

    public async Task<AdminAnalyticsDto> GetAnalyticsAsync(
        DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
    {
        var (start, end) = ResolveWindow(from, to);
        var utcNow = DateTime.UtcNow;

        var adsOverTime = await _ads.CountByDayAsync(start, end, cancellationToken);
        var usersOverTime = await _dashboard.GetUsersOverTimeAsync(start, end, cancellationToken);
        var revenueOverTime = await _dashboard.GetRevenueOverTimeAsync(start, end, cancellationToken);
        var views = await _dashboard.GetViewsAsync(TopCount, cancellationToken);

        var byCategory = await BuildCategoryBreakdownAsync(utcNow, cancellationToken);

        return new AdminAnalyticsDto
        {
            From = start,
            To = end,
            AdsOverTime = adsOverTime,
            UsersOverTime = usersOverTime,
            RevenueOverTime = revenueOverTime,
            AdsByCategory = byCategory,
            TopCategories = byCategory
                .Where(entry => entry.Count > 0)
                .OrderByDescending(entry => entry.Count)
                .Take(TopCount)
                .ToList(),
            Views = views
        };
    }

    private async Task<IReadOnlyList<AdminCategoryCountDto>> BuildCategoryBreakdownAsync(
        DateTime utcNow, CancellationToken cancellationToken)
    {
        var counts = await _ads.CountByModuleAsync(utcNow, cancellationToken);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        var byCategory = new Dictionary<int, int>();

        foreach (var entry in counts)
        {
            var categoryId = entry.SubCategoryId is { } subCategoryId
                ? (int?)ListingModuleCatalog.CategoryOf((SubCategoryType)subCategoryId)
                : (int?)ListingModuleCatalog.CategoryOf(entry.Type);

            if (categoryId is not { } key)
                continue;

            byCategory[key] = byCategory.GetValueOrDefault(key) + entry.Count;
        }

        var total = byCategory.Values.Sum();

        return tree
            .Select(category => new AdminCategoryCountDto
            {
                CategoryId = category.Id,
                CategoryName = category.NameAr,
                Count = byCategory.GetValueOrDefault(category.Id),
                Percentage = total == 0
                    ? 0
                    : Math.Round(byCategory.GetValueOrDefault(category.Id) * 100d / total, 1)
            })
            .ToList();
    }

    private static (DateTime From, DateTime To) ResolveWindow(DateTime? from, DateTime? to)
    {
        var end = to ?? DateTime.UtcNow;
        var start = from ?? end.AddDays(-DefaultWindowDays);

        if (start > end)
            (start, end) = (end, start);

        if (end - start > TimeSpan.FromDays(MaxWindowDays))
            start = end.AddDays(-MaxWindowDays);

        return (start.Date, end);
    }

    public async Task<AdminModerationOverviewDto> GetModerationOverviewAsync(
        int recentCount = 5, CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;
        recentCount = Math.Clamp(recentCount, 1, 20);

        var adsByStatus = await _ads.GetStatusBreakdownAsync(utcNow, cancellationToken);
        var pendingAds = adsByStatus.Where(row => row.ModerationStatus == ModerationStatus.Pending).Sum(row => row.Count);
        var suspendedAds = adsByStatus.Where(row => row.ModerationStatus == ModerationStatus.Suspended).Sum(row => row.Count);
        var pendingReports = await _dashboard.GetPendingReportsCountAsync(cancellationToken);
        var (_, pendingBanners) = await _dashboard.GetBannerCountsAsync(utcNow, cancellationToken);
        var pendingPayments = await _dashboard.GetPendingPaymentsCountAsync(cancellationToken);

        var recentAds = await _adService.GetAdsAsync(
            new AdminAdFilterParams
            {
                ModerationStatus = ModerationStatus.Pending,
                PageIndex = 1,
                PageSize = recentCount
            },
            cancellationToken);

        return new AdminModerationOverviewDto
        {
            PendingAds = pendingAds,
            PendingReports = pendingReports,
            PendingPayments = pendingPayments,
            PendingBanners = pendingBanners,
            SuspendedAds = suspendedAds,
            TotalPending = pendingAds + pendingReports + pendingPayments + pendingBanners,

            RecentPendingAds = recentAds.Items,
            RecentReports = await _dashboard.GetRecentReportsAsync(recentCount, cancellationToken),
            RecentBannerRequests = await _dashboard.GetPendingBannerRequestsAsync(recentCount, cancellationToken),
            RecentPayments = await _dashboard.GetPendingPaymentsAsync(recentCount, cancellationToken)
        };
    }

    public async Task<AdminBannerCenterDto> GetBannerCenterAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;
        var monthStart = new DateTime(utcNow.Year, utcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var expiringUntil = utcNow.AddDays(ExpiringSoonDays);

        var statusCounts = await _dashboard.GetBannerStatusCountsAsync(cancellationToken);
        var paymentPending = await _dashboard.GetBannerPaymentPendingCountAsync(cancellationToken);
        var (bannerTotal, bannerMonthly) = await _dashboard.GetBannerRevenueAsync(monthStart, cancellationToken);
        var (active, _) = await _dashboard.GetBannerCountsAsync(utcNow, cancellationToken);
        var expiring = await _dashboard.GetExpiringBannersAsync(utcNow, expiringUntil, TopCount, cancellationToken);

        var placements = await _settings.GetAllAsync(cancellationToken);
        var occupancy = await _dashboard.GetSlotOccupancyAsync(utcNow, cancellationToken);

        return new AdminBannerCenterDto
        {
            Summary = new AdminBannerCenterSummaryDto
            {
                PendingRequests = statusCounts.GetValueOrDefault(BannerBookingStatus.PendingReview),
                PaymentPending = paymentPending,
                PaymentApproved = statusCounts.GetValueOrDefault(BannerBookingStatus.PaymentApproved),
                Active = active,
                ExpiringSoon = expiring.Count,
                Expired = statusCounts.GetValueOrDefault(BannerBookingStatus.Expired),
                Rejected = statusCounts.GetValueOrDefault(BannerBookingStatus.Rejected),
                Cancelled = statusCounts.GetValueOrDefault(BannerBookingStatus.Cancelled),
                TotalRevenue = bannerTotal,
                MonthlyRevenue = bannerMonthly,
            Currency = PaymentCatalog.DefaultCurrency
            },
            Placements = BuildSlotBoard(placements, occupancy),
            ExpiringSoon = expiring
        };
    }

    private static IReadOnlyList<AdminBannerPlacementOverviewDto> BuildSlotBoard(
        IReadOnlyList<BannerPlacementDto> placements,
        IReadOnlyList<AdminBannerSlotOccupancy> occupancy)
    {
        var board = new List<AdminBannerPlacementOverviewDto>(placements.Count);

        foreach (var placement in placements)
        {
            var taken = occupancy.Where(entry => entry.Location == placement.Location).ToList();
            var slots = new List<AdminBannerSlotOverviewDto>();

            if (placement.Location == BannerLocation.SubCategoryBanner)
            {
                slots.AddRange(taken.Select(entry => Occupied(entry, 1)));
            }
            else
            {
                for (var slotNumber = 1; slotNumber <= placement.MaxSlots; slotNumber++)
                {
                    var entry = taken.FirstOrDefault(item => item.SlotNumber == slotNumber);

                    slots.Add(entry is null ? Free(slotNumber) : Occupied(entry, slotNumber));
                }
            }

            board.Add(new AdminBannerPlacementOverviewDto
            {
                Location = placement.Location,
                LocationName = placement.LocationName,
                Price = placement.Price,
                Currency = placement.Currency,
                DurationDays = placement.DurationDays,
                IsActive = placement.IsActive,
                Slots = slots
            });
        }

        return board;
    }

    private static AdminBannerSlotOverviewDto Free(int slotNumber) =>
        new()
        {
            SlotNumber = slotNumber,
            SlotName = BannerBookingCatalog.GetSlotName(slotNumber),
            Status = "Available",
            StatusName = "متاح",
            IsAvailable = true
        };

    private static AdminBannerSlotOverviewDto Occupied(AdminBannerSlotOccupancy entry, int slotNumber) =>
        new()
        {
            SlotNumber = slotNumber,
            SlotName = BannerBookingCatalog.GetSlotName(slotNumber),
            CategoryId = entry.CategoryId,
            CategoryName = entry.CategoryName,
            SubCategoryId = entry.SubCategoryId,
            SubCategoryName = entry.SubCategoryName,
            Status = entry.IsLive ? "Active" : "Occupied",
            StatusName = entry.IsLive ? "نشط" : "محجوز",
            IsAvailable = false,
            BookingId = entry.BookingId,
            BookingTitle = entry.Title,
            AdvertiserName = entry.AdvertiserName,
            StartDate = entry.StartDate,
            EndDate = entry.EndDate,
            AvailableFrom = entry.EndDate
        };

    private static IReadOnlyList<AdminQuickActionDto> BuildQuickActions(AdminDashboardStatsDto stats) =>
    [
        new()
        {
            Key = "review-pending-ads",
            Label = "مراجعة الإعلانات",
            Count = stats.PendingAdsCount,
            Route = "/admin/ads?moderationStatus=Pending",
            Icon = "📢",
            IsUrgent = stats.PendingAdsCount > 0
        },
        new()
        {
            Key = "review-reports",
            Label = "مراجعة البلاغات",
            Count = stats.PendingReportsCount,
            Route = "/admin/reports?status=Pending",
            Icon = "🚨",
            IsUrgent = stats.PendingReportsCount > 0
        },
        new()
        {
            Key = "review-payments",
            Label = "مراجعة المدفوعات",
            Count = stats.PendingPaymentsCount,
            Route = "/admin/payments?status=Pending",
            Icon = "💳",
            IsUrgent = stats.PendingPaymentsCount > 0
        },
        new()
        {
            Key = "review-banner-requests",
            Label = "مراجعة طلبات البانر",
            Count = stats.PendingBannerRequestsCount,

            Route = $"/admin/banner-requests?status={(int)BannerBookingStatus.PendingReview}",
            Icon = "🖼️",
            IsUrgent = stats.PendingBannerRequestsCount > 0
        },
        new()
        {
            Key = "manage-users",
            Label = "إدارة المستخدمين",
            Count = stats.UsersCount,
            Route = "/admin/users",
            Icon = "👥",

            IsUrgent = false
        }
    ];
}
