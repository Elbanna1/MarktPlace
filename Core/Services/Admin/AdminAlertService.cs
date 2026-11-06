using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Services.Notifications;
using Shared.Constants;
using Shared.Enums;

namespace Services.Admin;

public class AdminAlertService : IAdminAlertService
{
    private readonly INotificationService _notifications;
    private readonly IUserRepository _users;
    private readonly IUserListingRepository _listings;
    private readonly ILookupService _lookups;
    private readonly INotificationRepository _notificationHistory;
    private readonly ILogger<AdminAlertService> _logger;

    public AdminAlertService(
        INotificationService notifications,
        IUserRepository users,
        IUserListingRepository listings,
        ILookupService lookups,
        INotificationRepository notificationHistory,
        ILogger<AdminAlertService> logger)
    {
        _notifications = notifications;
        _users = users;
        _listings = listings;
        _lookups = lookups;
        _notificationHistory = notificationHistory;
        _logger = logger;
    }

    public async Task<int> NotifyListingsExpiringAsync(
        IReadOnlyCollection<ExpiringListingRow> listings, DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        if (listings.Count == 0)
            return 0;

        var admins = await GetAdminIdsAsync(cancellationToken);

        if (admins.Count == 0)
        {
            _logger.LogWarning(
                "{Count} listing(s) are about to expire but no administrator exists to notify.", listings.Count);
            return 0;
        }

        var deliveries = await _notificationHistory.FindDeliveriesAsync(
            admins,
            NotificationType.AdminListingExpiring,
            listings.Select(listing => listing.Id).Distinct().ToList(),
            cancellationToken);

        var lastSent = deliveries.ToDictionary(
            delivery => (delivery.UserId, delivery.ReferenceId),
            delivery => delivery.LastCreatedAt);

        var sent = 0;

        foreach (var listing in listings)
        {
            var recipients = admins
                .Where(admin => !WasToldThisCycle(lastSent, admin, listing))
                .ToList();

            if (recipients.Count == 0)
                continue;

            try
            {
                var context = await ListingContextResolver.ResolveAsync(
                    _listings, _lookups, _users, listing.Type, listing.Id, cancellationToken);

                var content = NotificationCatalog.AdminAlerts.ListingExpiring(
                    listing.Type,
                    listing.Id,
                    listing.Title,
                    listing.ExpireAt,
                    ListingLifecycle.RemainingDays(listing.ExpireAt, utcNow) ?? 0,
                    ownerId: listing.OwnerId,
                    ownerName: context?.OwnerName,
                    categoryId: context?.CategoryId ?? listing.CategoryId,
                    categoryName: context?.CategoryName,
                    subCategoryId: context?.SubCategoryId ?? listing.SubCategoryId,
                    subCategoryName: context?.SubCategoryName,
                    imageUrl: context?.ImageUrl);

                sent += await _notifications.CreateManyAsync(
                    recipients, content, listing.Id, NotificationAction.Expired, cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                _logger.LogError(exception,
                    "Failed to alert administrators that {Module} {ListingId} is about to expire.",
                    listing.Type, listing.Id);
            }
        }

        return sent;
    }

    private static bool WasToldThisCycle(
        IReadOnlyDictionary<(string UserId, Guid ReferenceId), DateTime> lastSent,
        string adminId,
        ExpiringListingRow listing) =>
        lastSent.TryGetValue((adminId, listing.Id), out var sentAt) &&
        (listing.PublishedAt is not { } cycleStart || sentAt > cycleStart);

    public async Task NotifyPendingListingAsync(
        ListingModuleType listingType, Guid listingId, string? title,
        DateTime? cycleStartedUtc,
        CancellationToken cancellationToken = default)
    {
        var context = await ListingContextResolver.ResolveAsync(
            _listings, _lookups, _users, listingType, listingId, cancellationToken);

        await SendOnceAsync(
            NotificationCatalog.AdminAlerts.PendingListing(
                listingType,
                listingId,
                title ?? context?.Title,
                ownerId: context?.OwnerId,
                ownerName: context?.OwnerName,
                categoryId: context?.CategoryId,
                categoryName: context?.CategoryName,
                subCategoryId: context?.SubCategoryId,
                subCategoryName: context?.SubCategoryName,
                imageUrl: context?.ImageUrl),
            listingId, cycleStartedUtc, cancellationToken);
    }

    public Task NotifyNewReportAsync(
        Guid reportId, ListingModuleType listingType, Guid listingId, string? listingTitle,
        string reasonName, CancellationToken cancellationToken = default) =>

        SendOnceAsync(
            NotificationCatalog.AdminAlerts.NewReport(
                reportId, listingType, listingId, listingTitle, reasonName),
            reportId, createdAfterUtc: null, cancellationToken);

    public Task NotifyNewBannerRequestAsync(
        Guid bookingId, string? title, string advertiserName, decimal price,
        CancellationToken cancellationToken = default) =>
        SendOnceAsync(
            NotificationCatalog.AdminAlerts.NewBannerRequest(bookingId, title, advertiserName, price),
            bookingId, createdAfterUtc: null, cancellationToken);

    public Task NotifyNewPaymentAsync(
        Guid paymentId, decimal amount, string currency, string? payerName,
        CancellationToken cancellationToken = default) =>
        SendOnceAsync(
            NotificationCatalog.AdminAlerts.NewPayment(paymentId, amount, currency, payerName),
            paymentId, createdAfterUtc: null, cancellationToken);

    public Task NotifyNewAdPaymentAsync(
        Guid paymentId, decimal amount, string currency, string? payerName, string? listingTitle,
        ListingModuleType listingType, bool isResubmission, CancellationToken cancellationToken = default) =>
        SendOnceAsync(
            NotificationCatalog.AdminAlerts.NewAdPayment(
                paymentId, amount, currency, payerName, listingTitle, listingType, isResubmission),
            paymentId, createdAfterUtc: null, cancellationToken);

    public Task NotifyUserModerationAsync(
        string targetUserId, string? targetUserName, UserAccountStatus status, string? reason,
        CancellationToken cancellationToken = default) =>

        SendAsync(
            NotificationCatalog.AdminAlerts.UserModeration(targetUserId, targetUserName, status, reason),
            referenceId: null, cancellationToken);

    public Task NotifyBannerExpiringAsync(
        Guid bookingId, string? title, DateTime endDate, int daysLeft,
        CancellationToken cancellationToken = default) =>

        SendOnceAsync(
            NotificationCatalog.AdminAlerts.BannerExpiring(bookingId, title, endDate, daysLeft),
            bookingId, createdAfterUtc: null, cancellationToken);

    private async Task SendOnceAsync(
        NotificationContent content, Guid referenceId, DateTime? createdAfterUtc,
        CancellationToken cancellationToken)
    {
        try
        {
            var admins = await GetAdminIdsAsync(cancellationToken);

            if (admins.Count == 0)
                return;

            await _notifications.CreateManyIfNotExistAsync(
                admins, content, referenceId, createdAfterUtc,
                action: NotificationAction.Created, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            LogFailure(content, exception);
        }
    }

    private async Task SendAsync(
        NotificationContent content, Guid? referenceId, CancellationToken cancellationToken)
    {
        try
        {
            var admins = await GetAdminIdsAsync(cancellationToken);

            if (admins.Count == 0)
                return;

            await _notifications.CreateManyAsync(
                admins, content, referenceId, NotificationAction.Created, cancellationToken);
        }
        catch (Exception exception)
        {
            LogFailure(content, exception);
        }
    }

    private Task<IReadOnlyList<string>> GetAdminIdsAsync(CancellationToken cancellationToken) =>
        _users.GetUserIdsInRoleAsync(AppRoles.Admin, cancellationToken);

    private void LogFailure(NotificationContent content, Exception exception) =>
        _logger.LogError(
            exception, "Failed to raise the administrator alert {Type} ({Title}).",
            content.Type, content.Title);
}
