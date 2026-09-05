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
    private readonly ILogger<AdminAlertService> _logger;

    public AdminAlertService(
        INotificationService notifications,
        IUserRepository users,
        IUserListingRepository listings,
        ILookupService lookups,
        ILogger<AdminAlertService> logger)
    {
        _notifications = notifications;
        _users = users;
        _listings = listings;
        _lookups = lookups;
        _logger = logger;
    }

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
