using Domain.Entities;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Listings;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.Listings;

public class ListingRepublishService : IListingRepublishService
{
    private readonly IListingLifecycleRepository _lifecycle;
    private readonly IAdvertisementService _advertisements;
    private readonly INotificationService _notifications;
    private readonly IAdminAlertService _alerts;
    private readonly ILogger<ListingRepublishService> _logger;

    public ListingRepublishService(
        IListingLifecycleRepository lifecycle,
        IAdvertisementService advertisements,
        INotificationService notifications,
        IAdminAlertService alerts,
        ILogger<ListingRepublishService> logger)
    {
        _lifecycle = lifecycle;
        _advertisements = advertisements;
        _notifications = notifications;
        _alerts = alerts;
        _logger = logger;
    }

    public async Task<ListingRepublishResultDto> RepublishAsync(
        string userId, ListingModuleType type, Guid listingId,
        CancellationToken cancellationToken = default)
    {
        if (type == ListingModuleType.Advertisement)
            return await RepublishAdvertisementAsync(userId, listingId);

        var listing = await _lifecycle.FindAsync(type, listingId, cancellationToken)
            ?? throw new NotFoundException("الإعلان غير موجود.");

        if (listing is not IModeratedListing moderated || moderated.OwnerUserId != userId)
            throw new NotFoundException("الإعلان غير موجود.");

        var now = DateTime.UtcNow;

        if (listing.ExpireAt is not { } expireAt)
        {
            throw new BadRequestException(
                "لم يتم نشر هذا الإعلان بعد، ولذلك لا يحتاج إلى إعادة نشر.");
        }

        if (expireAt > now)
        {
            var left = ListingLifecycle.RemainingDays(expireAt, now) ?? 0;

            throw new BadRequestException(
                $"لا يمكن إعادة نشر إعلان ما زال ساريًا. يتبقى له {left} يوم.");
        }

        listing.PublishedAt = null;
        listing.ExpireAt = null;
        listing.RepublishCount++;

        moderated.ModerationStatus = ModerationStatus.Pending;
        moderated.ModeratedAt = null;
        moderated.ModeratedBy = null;
        moderated.RejectionReason = null;
        moderated.ModerationNotes = null;

        await _lifecycle.SaveAsync(type, cancellationToken);

        await _notifications.NotifyAsync(
            userId, type, NotificationAction.Republished, listingId, moderated.ListingTitle);

        await _alerts.NotifyPendingListingAsync(
            type, listingId, moderated.ListingTitle, cycleStartedUtc: null, cancellationToken);

        _logger.LogInformation(
            "{Module} {ListingId} was republished by its owner and returned to the review queue (cycle {Count}).",
            type, listingId, listing.RepublishCount);

        return new ListingRepublishResultDto
        {
            Id = listingId,
            TypeId = type,
            Type = type.ToString(),
            Route = ListingModuleCatalog.RouteOf(type),
            Title = moderated.ListingTitle,
            StartDate = null,
            EndDate = null,
            RemainingDays = null,
            Status = ListingStatus.Pending.ToString(),
            RequiresReview = true,
            RepublishCount = listing.RepublishCount,
            Message = $"تم إرسال الإعلان للمراجعة. سيعود للظهور لمدة " +
                      $"{ListingLifecycle.ActiveDurationDays} يومًا بعد موافقة الإدارة."
        };
    }

    private async Task<ListingRepublishResultDto> RepublishAdvertisementAsync(
        string userId, Guid listingId)
    {
        var details = await _advertisements.RepublishAsync(userId, listingId);

        await _alerts.NotifyPendingListingAsync(
            ListingModuleType.Advertisement, listingId, details.Title,
            cycleStartedUtc: null, CancellationToken.None);

        return new ListingRepublishResultDto
        {
            Id = details.Id,
            TypeId = ListingModuleType.Advertisement,
            Type = ListingModuleType.Advertisement.ToString(),
            Route = ListingModuleCatalog.RouteOf(ListingModuleType.Advertisement),
            Title = details.Title,
            StartDate = null,
            EndDate = null,
            RemainingDays = null,
            Status = ListingStatus.Pending.ToString(),
            RequiresReview = true,

            RepublishCount = 0,
            Message = $"تم إرسال الإعلان للمراجعة. هيرجع يظهر لمدة " +
                      $"{ListingLifecycle.ActiveDurationDays} يوم بعد موافقة الإدارة."
        };
    }
}
