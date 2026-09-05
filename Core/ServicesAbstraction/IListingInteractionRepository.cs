using Domain.Entities.Listings;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IListingInteractionRepository
{
    Task<ListingViewer?> GetViewerAsync(
        ListingModuleType type, Guid listingId, string viewerKey, CancellationToken cancellationToken = default);

    Task<ListingViewCounter?> GetCounterAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    void AddViewer(ListingViewer viewer);

    void AddCounter(ListingViewCounter counter);

    Task<int> IncrementViewsAsync(
        ListingModuleType type, Guid listingId, int delta, DateTime viewedAt,
        CancellationToken cancellationToken = default);

    Task TrimRecentlyViewedAsync(string userId, int keep, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<(ListingModuleType Type, Guid ListingId, DateTime LastViewedAt)> Items, int TotalCount)>
        GetRecentlyViewedAsync(
            string userId, ListingModuleType? type, int pageIndex, int pageSize,
            CancellationToken cancellationToken = default);

    Task<int> ClearRecentlyViewedAsync(
        string userId, ListingModuleType? type, CancellationToken cancellationToken = default);

    Task<int> RemoveRecentlyViewedAsync(
        string userId, ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    Task<ListingFavorite?> GetFavoriteAsync(
        string userId, ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    void AddFavorite(ListingFavorite favorite);

    void RemoveFavorite(ListingFavorite favorite);

    Task<int> CountFavoritesAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<(ListingModuleType Type, Guid ListingId, DateTime CreatedAt)> Items, int TotalCount)>
        GetFavoritesAsync(
            string userId, ListingModuleType? type, int pageIndex, int pageSize,
            CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<(ListingModuleType Type, Guid ListingId), ListingCountersDto>> GetCountersAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        string? userId,
        CancellationToken cancellationToken = default);

    Task<(int TotalViews, int TotalFavorites)> GetOwnerTotalsAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> ownedKeys,
        CancellationToken cancellationToken = default);

    Task<ListingReport?> GetReportAsync(
        ListingModuleType type, Guid listingId, string reporterUserId,
        CancellationToken cancellationToken = default);

    Task<ListingReport?> GetReportByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void AddReport(ListingReport report);

    Task<ListingRating?> GetRatingAsync(
        ListingModuleType type, Guid listingId, string reviewerUserId,
        CancellationToken cancellationToken = default);

    void AddRating(ListingRating rating);

    void RemoveRating(ListingRating rating);

    Task<ListingRatingSummaryDto> GetRatingSummaryAsync(
        ListingModuleType type, Guid listingId, string? viewerUserId,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ListingRating> Items, int TotalCount)> GetRatingsAsync(
        ListingRatingFilterParams filter, CancellationToken cancellationToken = default);

    Task<ListingRating?> GetRatingByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ListingReport> Items, int TotalCount)> GetReportsAsync(
        ListingReportFilterParams filter, CancellationToken cancellationToken = default);

    Task PurgeListingAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ListingReport>> CloseOpenReportsForListingAsync(
        ListingModuleType type, Guid listingId, string adminNote, DateTime reviewedAt,
        CancellationToken cancellationToken = default);

    Task<int> PurgeOrphanInteractionsAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
