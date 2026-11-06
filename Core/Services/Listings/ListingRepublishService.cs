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
    private readonly IAdPaymentGate? _paymentGate;

    public ListingRepublishService(
        IListingLifecycleRepository lifecycle,
        IAdvertisementService advertisements,
        INotificationService notifications,
        IAdminAlertService alerts,
        ILogger<ListingRepublishService> logger,
        IAdPaymentGate? paymentGate = null)
    {
        _paymentGate = paymentGate;
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
        var result = type == ListingModuleType.Advertisement
            ? await RepublishAdvertisementAsync(userId, listingId)
            : await RepublishModuleAsync(userId, type, listingId, cancellationToken);

        if (_paymentGate is not null && await _paymentGate.IsAwaitingPaymentAsync(type, listingId, cancellationToken))
        {
            result.PaymentRequired = true;
            result.PaymentEndpoint = AdPaymentCatalog.SubmitRouteOf(type, listingId);
            result.Message += " رسوم النشر مفعلة، فلازم ترفع إثبات دفع جديد قبل ما الإدارة توافق على الإعلان.";
        }

        return result;
    }

    private async Task<ListingRepublishResultDto> RepublishModuleAsync(
        string userId, ListingModuleType type, Guid listingId, CancellationToken cancellationToken)
    {

        var (listing, moderated) = await _lifecycle.RunExclusiveAsync(type, listingId, async () =>
        {
            var found = await _lifecycle.FindAsync(type, listingId, cancellationToken)
                ?? throw new NotFoundException("الإعلان غير موجود.");

            if (found is not IModeratedListing owned || owned.OwnerUserId != userId)
                throw new NotFoundException("الإعلان غير موجود.");

            var now = DateTime.UtcNow;

            if (found.ExpireAt is not { } expireAt)
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

            found.PublishedAt = null;
            found.ExpireAt = null;
            found.RepublishCount++;

            owned.ModerationStatus = ModerationStatus.Pending;
            owned.ModeratedAt = null;
            owned.ModeratedBy = null;
            owned.RejectionReason = null;
            owned.ModerationNotes = null;

            _lifecycle.MarkAsNew(found, now);

            await _lifecycle.SaveAsync(type, cancellationToken);

            return (found, owned);
        }, cancellationToken);

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
