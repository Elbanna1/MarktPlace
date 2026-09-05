using Shared.DTOs.Admin;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IAdminDashboardService
{
    Task<AdminDashboardDto> GetAsync(int latestCount = 5, CancellationToken cancellationToken = default);

    Task<AdminAnalyticsDto> GetAnalyticsAsync(
        DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);

    Task<AdminModerationOverviewDto> GetModerationOverviewAsync(
        int recentCount = 5, CancellationToken cancellationToken = default);

    Task<AdminBannerCenterDto> GetBannerCenterAsync(CancellationToken cancellationToken = default);
}

public interface IAdminDashboardRepository
{
    Task<(int Total, int Suspended)> GetUserCountsAsync(CancellationToken cancellationToken = default);

    Task<(int Active, int Pending)> GetBannerCountsAsync(
        DateTime utcNow, CancellationToken cancellationToken = default);

    Task<int> GetPendingPaymentsCountAsync(CancellationToken cancellationToken = default);

    Task<int> GetPendingReportsCountAsync(CancellationToken cancellationToken = default);

    Task<(decimal Total, decimal Monthly)> GetRevenueAsync(
        DateTime monthStartUtc, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminDashboardBannerRequestDto>> GetLatestBannerRequestsAsync(
        int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminDashboardPaymentDto>> GetLatestPaymentsAsync(
        int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminTimeSeriesPointDto>> GetUsersOverTimeAsync(
        DateTime from, DateTime to, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminRevenuePointDto>> GetRevenueOverTimeAsync(
        DateTime from, DateTime to, CancellationToken cancellationToken = default);

    Task<AdminViewsAnalyticsDto> GetViewsAsync(
        int topCount, CancellationToken cancellationToken = default);

    Task<(decimal Total, decimal Monthly)> GetBannerRevenueAsync(
        DateTime monthStartUtc, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<BannerBookingStatus, int>> GetBannerStatusCountsAsync(
        CancellationToken cancellationToken = default);

    Task<int> GetBannerPaymentPendingCountAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminDashboardBannerRequestDto>> GetExpiringBannersAsync(
        DateTime utcNow, DateTime until, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminBannerSlotOccupancy>> GetSlotOccupancyAsync(
        DateTime utcNow, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Shared.DTOs.Listings.ListingReportDto>> GetRecentReportsAsync(
        int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminDashboardBannerRequestDto>> GetPendingBannerRequestsAsync(
        int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminDashboardPaymentDto>> GetPendingPaymentsAsync(
        int count, CancellationToken cancellationToken = default);
}

public record AdminBannerSlotOccupancy(
    BannerLocation Location,
    int SlotNumber,
    int? CategoryId,
    string? CategoryName,
    int? SubCategoryId,
    string? SubCategoryName,
    Guid BookingId,
    string Title,
    string AdvertiserName,
    DateTime StartDate,
    DateTime EndDate,
    BannerBookingStatus Status,
    bool IsLive);
