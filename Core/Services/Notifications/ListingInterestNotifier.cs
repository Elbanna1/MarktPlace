using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Services.Listings;
using Shared.Constants;
using Shared.Enums;

namespace Services.Notifications;

public class ListingInterestNotifier : IListingInterestNotifier
{
    private const int BatchSize = 500;

    private readonly INotificationInterestRepository _interests;
    private readonly INotificationService _notifications;
    private readonly IUserListingRepository _listings;
    private readonly ILookupService _lookups;
    private readonly IUserRepository _users;
    private readonly ILogger<ListingInterestNotifier> _logger;

    public ListingInterestNotifier(
        INotificationInterestRepository interests,
        INotificationService notifications,
        IUserListingRepository listings,
        ILookupService lookups,
        IUserRepository users,
        ILogger<ListingInterestNotifier> logger)
    {
        _interests = interests;
        _notifications = notifications;
        _listings = listings;
        _lookups = lookups;
        _users = users;
        _logger = logger;
    }

    public async Task<int> AnnounceAsync(
        ListingModuleType listingType, Guid listingId, CancellationToken cancellationToken = default)
    {
        var listing = await _listings.GetByKeyAsync(listingType, listingId, DateTime.UtcNow, cancellationToken);

        if (listing is null || listing.Status != ListingStatus.Active)
        {
            _logger.LogInformation(
                "No interest fan-out for {Module} {ListingId}: it is not publicly visible ({Status}).",
                listingType, listingId, listing?.Status.ToString() ?? "missing");

            return 0;
        }

        var (categoryId, subCategoryId) = ListingInteractionService.ResolveCategory(listing);

        if (categoryId == 0)
        {
            _logger.LogWarning(
                "No interest fan-out for {Module} {ListingId}: the module has no category registered in ListingModuleCatalog.",
                listingType, listingId);

            return 0;
        }

        if (!await _interests.TryBeginDispatchAsync(listingType, listingId, cancellationToken))
        {
            _logger.LogInformation(
                "Interest fan-out for {Module} {ListingId} skipped: already announced.",
                listingType, listingId);

            return 0;
        }

        var context = await ListingContextResolver.FromRowAsync(
            _lookups, _users, listing, cancellationToken);

        var content = NotificationCatalog.Interests.NewListing(
            listingType,
            context.CategoryName,
            context.SubCategoryName,
            listingId,
            listing.Title,
            categoryId,
            subCategoryId,
            listing.MainImageUrl,
            ownerId: context.OwnerId,
            ownerName: context.OwnerName);

        var total = 0;
        string? cursor = null;

        while (true)
        {
            var recipients = await _interests.GetMatchingUserIdsAsync(
                categoryId,
                subCategoryId,
                excludeUserId: listing.OwnerId,
                afterUserId: cursor,
                batchSize: BatchSize,
                cancellationToken);

            if (recipients.Count == 0)
                break;

            total += await _notifications.CreateManyAsync(
                recipients, content, listingId, NotificationAction.Created, cancellationToken);

            cursor = recipients[^1];

            if (recipients.Count < BatchSize)
                break;
        }

        await _interests.CompleteDispatchAsync(listingType, listingId, total, cancellationToken);

        _logger.LogInformation(
            "Interest fan-out for {Module} {ListingId} (category {CategoryId}/{SubCategoryId}) reached {Count} users.",
            listingType, listingId, categoryId, subCategoryId, total);

        return total;
    }
}
