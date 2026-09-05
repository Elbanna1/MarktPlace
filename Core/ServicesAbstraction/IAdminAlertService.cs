using Shared.Enums;

namespace ServicesAbstraction;

public interface IAdminAlertService
{
    Task NotifyPendingListingAsync(
        ListingModuleType listingType, Guid listingId, string? title,
        DateTime? cycleStartedUtc,
        CancellationToken cancellationToken = default);

    Task NotifyNewReportAsync(
        Guid reportId, ListingModuleType listingType, Guid listingId, string? listingTitle,
        string reasonName, CancellationToken cancellationToken = default);

    Task NotifyNewBannerRequestAsync(
        Guid bookingId, string? title, string advertiserName, decimal price,
        CancellationToken cancellationToken = default);

    Task NotifyNewPaymentAsync(
        Guid paymentId, decimal amount, string currency, string? payerName,
        CancellationToken cancellationToken = default);

    Task NotifyUserModerationAsync(
        string targetUserId, string? targetUserName, UserAccountStatus status, string? reason,
        CancellationToken cancellationToken = default);

    Task NotifyBannerExpiringAsync(
        Guid bookingId, string? title, DateTime endDate, int daysLeft,
        CancellationToken cancellationToken = default);
}
