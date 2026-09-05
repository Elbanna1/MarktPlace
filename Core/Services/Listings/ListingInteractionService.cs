using Domain.Entities;
using Domain.Entities.Listings;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Listings;
using Shared.DTOs.Lookups;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;
using Shared.Settings;

namespace Services.Listings;

public class ListingInteractionService : IListingInteractionService
{
    private readonly IAdminAuditService _audit;
    private readonly IAdminAlertService _alerts;
    private readonly IListingInteractionRepository _repository;
    private readonly IUserListingRepository _listings;
    private readonly ILookupService _lookups;
    private readonly INotificationService _notifications;
    private readonly ListingInteractionSettings _settings;
    private readonly IUnitOfWork _unitOfWork;

    public ListingInteractionService(
        IListingInteractionRepository repository,
        IUserListingRepository listings,
        ILookupService lookups,
        INotificationService notifications,
        IOptions<ListingInteractionSettings> settings,
        IAdminAuditService audit,
        IAdminAlertService alerts,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _listings = listings;
        _lookups = lookups;
        _notifications = notifications;
        _settings = settings.Value;
        _audit = audit;
        _alerts = alerts;
    }

    public async Task<ListingViewResultDto> RecordViewAsync(
        ListingModuleType type,
        Guid listingId,
        string? viewerUserId,
        string? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var listing = await GetListingOrThrowAsync(type, listingId, now, cancellationToken);

        var viewerKey = BuildViewerKey(viewerUserId, ipAddress);

        if (viewerKey is null)
        {
            return new ListingViewResultDto
            {
                ViewsListingType = type,
                ViewsListingId = listingId,
                TotalViews = await GetViewsAsync(type, listingId, cancellationToken),
                LastViewedAt = null,
                Counted = false
            };
        }

        var viewer = await _repository.GetViewerAsync(type, listingId, viewerKey, cancellationToken);
        var counter = await _repository.GetCounterAsync(type, listingId, cancellationToken);

        var isOwnerViewing = viewerUserId is not null && viewerUserId == listing.OwnerId;

        var counts = !isOwnerViewing &&
                     (viewer is null || viewer.LastViewedAt + _settings.ViewDedupeWindow <= now);

        if (viewer is null)
        {
            viewer = new ListingViewer
            {
                ListingType = type,
                ListingId = listingId,
                ViewerKey = viewerKey,
                UserId = viewerUserId,
                FirstViewedAt = now,
                LastViewedAt = now,
                ViewCount = counts ? 1 : 0
            };
            _repository.AddViewer(viewer);
        }
        else
        {
            viewer.LastViewedAt = now;

            if (counts)
                viewer.ViewCount++;
        }

        if (counter is null)
        {
            counter = new ListingViewCounter
            {
                ListingType = type,
                ListingId = listingId,
                TotalViews = counts ? 1 : 0,
                LastViewedAt = now
            };
            _repository.AddCounter(counter);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _repository.SaveChangesAsync(cancellationToken);

            if (counts)
                await _repository.IncrementViewsAsync(type, listingId, 1, now, cancellationToken);
        }

        if (viewerUserId is not null && viewer.ViewCount <= 1 && counts)
            await _repository.TrimRecentlyViewedAsync(
                viewerUserId, _settings.RecentlyViewedRetentionCount, cancellationToken);

        return new ListingViewResultDto
        {
            ViewsListingType = type,
            ViewsListingId = listingId,

            TotalViews = await GetViewsAsync(type, listingId, cancellationToken),

            LastViewedAt = viewerUserId is null ? null : now,
            Counted = counts
        };
    }

    private async Task<int> GetViewsAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken) =>
        (await _repository.GetCounterAsync(type, listingId, cancellationToken))?.TotalViews ?? 0;

    private static string? BuildViewerKey(string? viewerUserId, string? ipAddress)
    {
        if (!string.IsNullOrWhiteSpace(viewerUserId))
            return Truncate(viewerUserId);

        if (!string.IsNullOrWhiteSpace(ipAddress))
            return Truncate(ListingInteractionCatalog.AnonymousViewerKeyPrefix + ipAddress);

        return null;
    }

    private static string Truncate(string value) =>
        value.Length <= ListingInteractionCatalog.ViewerKeyMaxLength
            ? value
            : value[..ListingInteractionCatalog.ViewerKeyMaxLength];

    public async Task<ListingFavoriteResultDto> AddFavoriteAsync(
        string userId, ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default)
    {
        await GetListingOrThrowAsync(type, listingId, DateTime.UtcNow, cancellationToken);

        var existing = await _repository.GetFavoriteAsync(userId, type, listingId, cancellationToken);

        if (existing is null)
        {
            _repository.AddFavorite(new ListingFavorite
            {
                UserId = userId,
                ListingType = type,
                ListingId = listingId,
                CreatedAt = DateTime.UtcNow
            });

            try
            {
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception) when (_unitOfWork.IsUniqueConstraintViolation(exception))
            {
            }
        }

        return new ListingFavoriteResultDto
        {
            IsFavorite = true,
            FavoriteCount = await _repository.CountFavoritesAsync(type, listingId, cancellationToken)
        };
    }

    public async Task<ListingFavoriteResultDto> RemoveFavoriteAsync(
        string userId, ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetFavoriteAsync(userId, type, listingId, cancellationToken);

        if (existing is not null)
        {
            _repository.RemoveFavorite(existing);
            await _repository.SaveChangesAsync(cancellationToken);
        }

        return new ListingFavoriteResultDto
        {
            IsFavorite = false,
            FavoriteCount = await _repository.CountFavoritesAsync(type, listingId, cancellationToken)
        };
    }

    public async Task<PaginatedResult<ListingCardDto>> GetRecentlyViewedAsync(
        string userId, ListingCardFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (rows, totalCount) = await _repository.GetRecentlyViewedAsync(
            userId, filter.Type, filter.PageIndex, filter.PageSize, cancellationToken);

        var lastViewed = rows.ToDictionary(r => (r.Type, r.ListingId), r => r.LastViewedAt);

        var keys = rows.Select(r => (r.Type, r.ListingId)).ToList();
        var cards = await BuildCardsAsync(keys, userId, cancellationToken);

        totalCount -= await ReconcileOrphansAsync(keys, cards, totalCount, cancellationToken);

        foreach (var card in cards)
            card.LastViewedAt = lastViewed.GetValueOrDefault((card.Type, card.Id));

        var ordered = cards.OrderByDescending(c => c.LastViewedAt).ToList();

        return new PaginatedResult<ListingCardDto>(ordered, totalCount, filter.PageIndex, filter.PageSize);
    }

    private async Task<int> ReconcileOrphansAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        IReadOnlyCollection<ListingCardDto> cards,
        int totalCount,
        CancellationToken cancellationToken)
    {
        if (keys.Count == cards.Count)
            return 0;

        var found = cards.Select(card => (card.Type, card.Id)).ToHashSet();

        var orphans = keys
            .Where(key => _listings.SupportedModules.Contains(key.Type) && !found.Contains(key))
            .ToList();

        if (orphans.Count == 0)
            return 0;

        await _repository.PurgeOrphanInteractionsAsync(orphans, cancellationToken);

        return Math.Min(orphans.Count, totalCount);
    }

    public async Task<ListingHistoryClearedDto> ClearRecentlyViewedAsync(
        string userId, ListingModuleType? type, CancellationToken cancellationToken = default) =>
        new()
        {
            RemovedCount = await _repository.ClearRecentlyViewedAsync(userId, type, cancellationToken)
        };

    public async Task<ListingHistoryClearedDto> RemoveRecentlyViewedAsync(
        string userId, ListingModuleType type, Guid listingId,
        CancellationToken cancellationToken = default) =>

        new()
        {
            RemovedCount = await _repository.RemoveRecentlyViewedAsync(
                userId, type, listingId, cancellationToken)
        };

    public async Task<PaginatedResult<ListingCardDto>> GetFavoritesAsync(
        string userId, ListingCardFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (rows, totalCount) = await _repository.GetFavoritesAsync(
            userId, filter.Type, filter.PageIndex, filter.PageSize, cancellationToken);

        var favoritedAt = rows.ToDictionary(r => (r.Type, r.ListingId), r => r.CreatedAt);

        var keys = rows.Select(r => (r.Type, r.ListingId)).ToList();
        var cards = await BuildCardsAsync(keys, userId, cancellationToken);

        totalCount -= await ReconcileOrphansAsync(keys, cards, totalCount, cancellationToken);

        var ordered = cards
            .OrderByDescending(c => favoritedAt.GetValueOrDefault((c.Type, c.Id)))
            .ToList();

        return new PaginatedResult<ListingCardDto>(ordered, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<PaginatedResult<ListingCardDto>> GetSimilarAsync(
        ListingModuleType type,
        Guid listingId,
        SimilarListingFilterParams filter,
        string? viewerUserId,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var listing = await GetListingOrThrowAsync(type, listingId, now, cancellationToken);

        var (rows, totalCount) = await _listings.GetSimilarAsync(
            listing, filter.PageIndex, filter.PageSize, now, cancellationToken);

        var cards = await ToCardsAsync(rows, viewerUserId, cancellationToken);

        return new PaginatedResult<ListingCardDto>(cards, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<ListingCountersDto> GetCountersAsync(
        ListingModuleType type, Guid listingId, string? viewerUserId,
        CancellationToken cancellationToken = default)
    {
        var counters = await _repository.GetCountersAsync(
            new[] { (type, listingId) }, viewerUserId, cancellationToken);

        return counters.TryGetValue((type, listingId), out var found) ? found : new ListingCountersDto();
    }

    public Task<IReadOnlyDictionary<(ListingModuleType Type, Guid ListingId), ListingCountersDto>> GetCountersAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        string? viewerUserId,
        CancellationToken cancellationToken = default) =>
        _repository.GetCountersAsync(keys, viewerUserId, cancellationToken);

    public async Task<IReadOnlySet<(ListingModuleType Type, Guid ListingId)>> GetOwnedAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        string ownerUserId,
        CancellationToken cancellationToken = default)
    {
        if (keys.Count == 0 || string.IsNullOrEmpty(ownerUserId))
            return new HashSet<(ListingModuleType, Guid)>();

        var owned = await _listings.GetOwnedKeysAsync(
            keys, ownerUserId, DateTime.UtcNow, cancellationToken);

        return owned.ToHashSet();
    }

    public async Task<ListingActionsDto> GetActionsAsync(
        ListingModuleType type, Guid listingId, string? viewerUserId,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var listing = await GetListingOrThrowAsync(type, listingId, now, cancellationToken);

        var counters = await GetCountersAsync(type, listingId, viewerUserId, cancellationToken);

        var isSignedIn = !string.IsNullOrEmpty(viewerUserId);
        var isOwner = isSignedIn && listing.OwnerId == viewerUserId;

        var hasReported = !isOwner && isSignedIn &&
            await _repository.GetReportAsync(type, listingId, viewerUserId!, cancellationToken) is not null;

        var subject = NotificationCatalog.AllListingSubjects.GetValueOrDefault(type);

        var (categoryId, subCategoryId) = ResolveCategory(listing);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);
        var (categoryName, subCategoryName) = ResolveNames(tree, categoryId, subCategoryId);

        return new ListingActionsDto
        {
            Id = listing.Id,
            Type = type,
            TypeName = type.ToString(),
            TypeNameAr = ListingModuleCatalog.NameOf(type),
            Route = ListingModuleCatalog.RouteOf(type),
            Title = listing.Title,

            CategoryId = categoryId,
            CategoryName = categoryName,
            SubCategoryId = subCategoryId,
            SubCategoryName = subCategoryName,

            IsOwner = isOwner,
            Status = listing.Status,
            StatusName = ListingInteractionCatalog.NameOf(listing.Status),

            Views = counters.Views,
            FavoriteCount = counters.FavoriteCount,
            IsFavorite = counters.IsFavorite,
            HasReported = hasReported,

            CanEdit = isOwner,
            CanDelete = isOwner,

            CanFavorite = isSignedIn,
            CanReport = isSignedIn && !isOwner && !hasReported,

            CanRepublish = isOwner && listing.SupportsRepublish && listing.Status == ListingStatus.Expired,
            CanShare = true,

            DeepLink = subject?.Route is { } route ? $"{route}/{listing.Id}" : null
        };
    }

    public ListingInteractionMetadataDto GetMetadata() =>
        new()
        {
            Modules = ListingModuleCatalog.All
                .Where(module => NotificationCatalog.AllListingSubjects.ContainsKey(module))
                .Select(module =>
                {
                    var subCategory = ListingModuleCatalog.SubCategoryOf(module);
                    var category = subCategory is { } sub ? ListingModuleCatalog.CategoryOf(sub) : null;

                    return new ListingModuleDto
                    {
                        Id = (int)module,
                        Name = module.ToString(),
                        NameAr = ListingModuleCatalog.NameOf(module),
                        Route = ListingModuleCatalog.RouteOf(module),

                        CategoryId = (int)(category ?? 0),
                        SubCategoryId = (int)(subCategory ?? 0),
                        SupportsRepublish = module == ListingModuleType.Advertisement
                    };
                })
                .ToList(),

            ReportReasons = ListingInteractionCatalog.ReportReasonNames
                .Select(entry => new ListingReportOptionDto { Id = (int)entry.Key, Name = entry.Value })
                .ToList(),

            ReportStatuses = ListingInteractionCatalog.ReportStatusNames
                .Select(entry => new ListingReportOptionDto { Id = (int)entry.Key, Name = entry.Value })
                .ToList(),

            ViewDedupeWindowMinutes = _settings.ViewDedupeWindowMinutes,
            RecentlyViewedRetentionCount = _settings.RecentlyViewedRetentionCount
        };

    public async Task<ListingReportDto> ReportAsync(
        string reporterUserId,
        ListingModuleType type,
        Guid listingId,
        CreateListingReportRequest request,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var listing = await GetListingOrThrowAsync(type, listingId, now, cancellationToken);

        if (listing.OwnerId == reporterUserId)
            throw new BadRequestException("لا يمكنك الإبلاغ عن إعلانك الخاص.");

        var existing = await _repository.GetReportAsync(type, listingId, reporterUserId, cancellationToken);

        if (existing is not null)
            throw new BadRequestException("لقد قمت بالإبلاغ عن هذا الإعلان من قبل.");

        var report = new ListingReport
        {
            Id = Guid.NewGuid(),
            ListingType = type,
            ListingId = listingId,
            ReporterUserId = reporterUserId,

            ListingOwnerId = listing.OwnerId,
            ListingTitle = listing.Title,
            Reason = request.Reason,
            Details = request.Details?.Trim(),
            Status = ListingReportStatus.Pending,
            CreatedAt = now
        };

        _repository.AddReport(report);
        await _repository.SaveChangesAsync(cancellationToken);

        await _notifications.NotifyAsync(
            listing.OwnerId, type, NotificationAction.Reported, listingId, listing.Title);

        await _alerts.NotifyNewReportAsync(
            report.Id, type, listingId, listing.Title,
            ListingInteractionCatalog.NameOf(request.Reason), cancellationToken);

        return ToDto(report, includeAdminFields: false, reporterName: null);
    }

    public async Task<ListingRatingResultDto> RateAsync(
        string reviewerUserId,
        ListingModuleType type,
        Guid listingId,
        RateListingRequest request,
        CancellationToken cancellationToken = default)
    {
        var stars = request.Rating ?? 0;

        if (!ListingInteractionCatalog.IsValidRating(stars))
            throw new BadRequestException(ListingInteractionCatalog.RatingRangeMessage);

        var now = DateTime.UtcNow;
        var listing = await GetListingOrThrowAsync(type, listingId, now, cancellationToken);

        if (listing.OwnerId == reviewerUserId)
            throw new BadRequestException("لا يمكنك تقييم إعلانك الخاص.");

        var existing = await _repository.GetRatingAsync(type, listingId, reviewerUserId, cancellationToken);
        var created = existing is null;

        if (existing is null)
        {
            existing = new ListingRating
            {
                Id = Guid.NewGuid(),
                ListingType = type,
                ListingId = listingId,
                ReviewerUserId = reviewerUserId,

                ListingOwnerId = listing.OwnerId,
                ListingTitle = listing.Title,
                Rating = stars,
                CreatedAt = now
            };

            _repository.AddRating(existing);
        }
        else
        {
            existing.Rating = stars;
            existing.UpdatedAt = now;
            existing.ListingTitle = listing.Title;
            existing.ListingOwnerId = listing.OwnerId;
        }

        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception) when (created && _unitOfWork.IsUniqueConstraintViolation(exception))
        {
        }

        if (!string.IsNullOrEmpty(listing.OwnerId))
        {
            await _notifications.NotifyAsync(
                listing.OwnerId, type, NotificationAction.Rated, listingId, listing.Title);
        }

        var summary = await _repository.GetRatingSummaryAsync(type, listingId, reviewerUserId, cancellationToken);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        return new ListingRatingResultDto
        {
            Rating = ToDto(existing, listing, tree),
            AverageRating = summary.AverageRating,
            RatingsCount = summary.RatingsCount,
            Created = created
        };
    }

    public async Task<ListingRatingSummaryDto> RemoveRatingAsync(
        string reviewerUserId, ListingModuleType type, Guid listingId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetRatingAsync(type, listingId, reviewerUserId, cancellationToken);

        if (existing is not null)
        {
            _repository.RemoveRating(existing);
            await _repository.SaveChangesAsync(cancellationToken);
        }

        return await _repository.GetRatingSummaryAsync(type, listingId, reviewerUserId, cancellationToken);
    }

    public Task<ListingRatingSummaryDto> GetRatingSummaryAsync(
        ListingModuleType type, Guid listingId, string? viewerUserId,
        CancellationToken cancellationToken = default) =>
        _repository.GetRatingSummaryAsync(type, listingId, viewerUserId, cancellationToken);

    public async Task<PaginatedResult<ListingRatingDto>> GetRatingsAsync(
        ListingRatingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _repository.GetRatingsAsync(filter, cancellationToken);

        var keys = items.Select(r => (r.ListingType, r.ListingId)).Distinct().ToList();
        var rows = keys.Count == 0
            ? Array.Empty<UserListingRow>()
            : (IReadOnlyList<UserListingRow>)await _listings.GetByKeysAsync(keys, DateTime.UtcNow, cancellationToken);

        var byKey = rows.ToDictionary(row => (row.Type, row.Id));
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        var mapped = items
            .Select(rating => ToDto(
                rating, byKey.GetValueOrDefault((rating.ListingType, rating.ListingId)), tree))
            .ToList();

        return new PaginatedResult<ListingRatingDto>(mapped, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<ListingRatingDto> GetRatingAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rating = await _repository.GetRatingByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("التقييم غير موجود.");

        var listing = await _listings.GetByKeyAsync(
            rating.ListingType, rating.ListingId, DateTime.UtcNow, cancellationToken);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        return ToDto(rating, listing, tree);
    }

    public async Task DeleteRatingAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rating = await _repository.GetRatingByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("التقييم غير موجود.");

        _repository.RemoveRating(rating);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static ListingRatingDto ToDto(
        ListingRating rating, UserListingRow? listing, IReadOnlyList<CategoryTreeDto> tree)
    {
        var route = ListingModuleCatalog.RouteOf(rating.ListingType);

        var (categoryId, subCategoryId) = listing is not null
            ? ResolveCategory(listing)
            : ((int)(ListingModuleCatalog.SubCategoryOf(rating.ListingType) is { } s
                    ? ListingModuleCatalog.CategoryOf(s) ?? 0
                    : 0),
               (int)(ListingModuleCatalog.SubCategoryOf(rating.ListingType) ?? 0));

        var (categoryName, subCategoryName) = ResolveNames(tree, categoryId, subCategoryId);

        return new ListingRatingDto
        {
            Id = rating.Id,
            Rating = rating.Rating,
            Reviewer = new ListingRatingReviewerDto
            {
                Id = rating.ReviewerUserId,
                Name = FullNameOf(rating.Reviewer)
            },
            Target = new ListingRatingTargetDto
            {
                Type = rating.ListingType,
                TypeName = rating.ListingType.ToString(),
                TypeNameAr = ListingModuleCatalog.NameOf(rating.ListingType),
                Id = rating.ListingId,
                Title = listing?.Title ?? rating.ListingTitle,
                CategoryId = categoryId == 0 ? null : categoryId,
                CategoryName = string.IsNullOrEmpty(categoryName) ? null : categoryName,
                SubCategoryId = subCategoryId == 0 ? null : subCategoryId,
                SubCategoryName = string.IsNullOrEmpty(subCategoryName) ? null : subCategoryName,
                ImageUrl = listing?.MainImageUrl,
                Route = route,

                DetailsUrl = $"/{route}/{rating.ListingId}",
                IsAvailable = listing is not null
            },
            CreatedAt = rating.CreatedAt,
            UpdatedAt = rating.UpdatedAt
        };
    }

    private static string FullNameOf(ApplicationUser? user) =>
        user is null
            ? string.Empty
            : string.Join(' ', new[] { user.FirstName, user.SecondName }
                    .Where(part => !string.IsNullOrWhiteSpace(part)))
                .Trim();

    public async Task<PaginatedResult<ListingReportDto>> GetReportsAsync(
        ListingReportFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _repository.GetReportsAsync(filter, cancellationToken);

        var dtos = items
            .Select(report => ToDto(
                report,
                includeAdminFields: true,
                reporterName: report.Reporter is null
                    ? null
                    : $"{report.Reporter.FirstName} {report.Reporter.SecondName}".Trim()))
            .ToList();

        return new PaginatedResult<ListingReportDto>(dtos, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<ListingReportDto> UpdateReportAsync(
        Guid reportId, string adminUserId, UpdateListingReportRequest request,
        CancellationToken cancellationToken = default)
    {
        var report = await _repository.GetReportByIdAsync(reportId, cancellationToken)
            ?? throw new NotFoundException("البلاغ غير موجود.");

        var previousStatus = report.Status;

        report.Status = request.Status;
        report.AdminNote = request.AdminNote?.Trim();
        report.ReviewedAt = DateTime.UtcNow;
        report.ReviewedByUserId = adminUserId;

        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            request.Status == ListingReportStatus.Dismissed
                ? AdminAuditAction.DismissReport
                : AdminAuditAction.ResolveReport,
            AdminAuditCatalog.Targets.Report, reportId.ToString(),
            $"{ListingInteractionCatalog.NameOf(request.Status)}: {report.ListingTitle}",
            oldValue: ListingInteractionCatalog.NameOf(previousStatus),
            newValue: ListingInteractionCatalog.NameOf(request.Status),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        if (previousStatus != request.Status)
            await NotifyReportResolvedAsync(report);

        return ToDto(report, includeAdminFields: true, reporterName: null);
    }

    private async Task NotifyReportResolvedAsync(ListingReport report)
    {
        var action = report.Status switch
        {
            ListingReportStatus.UnderReview => NotificationAction.ReportUnderReview,
            ListingReportStatus.ActionTaken => NotificationAction.ReportActionTaken,
            ListingReportStatus.Dismissed => NotificationAction.ReportDismissed,

            _ => (NotificationAction?)null
        };

        if (action is not { } resolved)
            return;

        await _notifications.NotifyAsync(
            report.ReporterUserId, report.ListingType, resolved, report.ListingId, report.ListingTitle);

        if (resolved == NotificationAction.ReportActionTaken &&
            !string.IsNullOrEmpty(report.ListingOwnerId) &&
            report.ListingOwnerId != report.ReporterUserId)
        {
            await _notifications.NotifyAsync(
                report.ListingOwnerId, report.ListingType, resolved, report.ListingId, report.ListingTitle);
        }
    }

    public async Task PurgeListingAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default)
    {
        await _repository.PurgeListingAsync(type, listingId, cancellationToken);

        var closed = await _repository.CloseOpenReportsForListingAsync(
            type, listingId, ListingInteractionCatalog.ReportClosedOnListingRemovedNote,
            DateTime.UtcNow, cancellationToken);

        foreach (var report in closed)
        {
            await _notifications.NotifyAsync(
                report.ReporterUserId, type, NotificationAction.ReportActionTaken,
                listingId, report.ListingTitle);
        }
    }

    private async Task<UserListingRow> GetListingOrThrowAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow, CancellationToken cancellationToken) =>
        await _listings.GetByKeyAsync(type, listingId, utcNow, cancellationToken)
        ?? throw new NotFoundException("الإعلان غير موجود.");

    private async Task<List<ListingCardDto>> BuildCardsAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        string? viewerUserId,
        CancellationToken cancellationToken)
    {
        if (keys.Count == 0)
            return new List<ListingCardDto>();

        var rows = await _listings.GetByKeysAsync(keys, DateTime.UtcNow, cancellationToken);

        return await ToCardsAsync(rows, viewerUserId, cancellationToken);
    }

    private async Task<List<ListingCardDto>> ToCardsAsync(
        IReadOnlyList<UserListingRow> rows,
        string? viewerUserId,
        CancellationToken cancellationToken)
    {
        if (rows.Count == 0)
            return new List<ListingCardDto>();

        var keys = rows.Select(r => (r.Type, r.Id)).ToList();

        var counters = await _repository.GetCountersAsync(keys, viewerUserId, cancellationToken);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        return rows.Select(row =>
        {
            var counter = counters.GetValueOrDefault((row.Type, row.Id)) ?? new ListingCountersDto();
            var (categoryId, subCategoryId) = ResolveCategory(row);
            var (categoryName, subCategoryName) = ResolveNames(tree, categoryId, subCategoryId);

            return new ListingCardDto
            {
                Id = row.Id,
                Type = row.Type,
                TypeName = row.Type.ToString(),
                TypeNameAr = ListingModuleCatalog.NameOf(row.Type),
                Route = ListingModuleCatalog.RouteOf(row.Type),
                Title = row.Title,
                MainImageUrl = row.MainImageUrl,
                Price = row.Price,
                CategoryId = categoryId,
                CategoryName = categoryName,
                SubCategoryId = subCategoryId,
                SubCategoryName = subCategoryName,
                CreatedAt = row.CreatedAt,
                Views = counter.Views,
                FavoriteCount = counter.FavoriteCount,
                AverageRating = counter.AverageRating,
                RatingsCount = counter.RatingsCount,
                IsFavorite = counter.IsFavorite
            };
        }).ToList();
    }

    internal static (int CategoryId, int SubCategoryId) ResolveCategory(UserListingRow row)
    {
        if (row.CategoryId is { } categoryId && row.SubCategoryId is { } subCategoryId)
            return (categoryId, subCategoryId);

        var subCategory = ListingModuleCatalog.SubCategoryOf(row.Type);
        var category = subCategory is { } sub ? ListingModuleCatalog.CategoryOf(sub) : null;

        return ((int)(category ?? 0), (int)(subCategory ?? 0));
    }

    internal static (string CategoryName, string SubCategoryName) ResolveNames(
        IReadOnlyList<CategoryTreeDto> tree, int categoryId, int subCategoryId)
    {
        var category = tree.FirstOrDefault(c => c.Id == categoryId);

        if (category is null)
            return (string.Empty, string.Empty);

        var subCategory = category.SubCategories.FirstOrDefault(s => s.Id == subCategoryId);

        return (category.NameAr, subCategory?.NameAr ?? string.Empty);
    }

    private static ListingReportDto ToDto(
        ListingReport report, bool includeAdminFields, string? reporterName) =>
        new()
        {
            Id = report.Id,
            ListingType = report.ListingType,
            ListingTypeName = ListingModuleCatalog.NameOf(report.ListingType),
            ListingId = report.ListingId,
            ListingTitle = report.ListingTitle,
            Reason = report.Reason,
            ReasonName = ListingInteractionCatalog.NameOf(report.Reason),
            Details = report.Details,
            Status = report.Status,
            StatusName = ListingInteractionCatalog.NameOf(report.Status),
            CreatedAt = report.CreatedAt,
            ReporterUserId = includeAdminFields ? report.ReporterUserId : null,
            ReporterName = includeAdminFields ? reporterName : null,
            ListingOwnerId = includeAdminFields ? report.ListingOwnerId : null,
            ReviewedAt = includeAdminFields ? report.ReviewedAt : null,
            AdminNote = includeAdminFields ? report.AdminNote : null
        };
}
