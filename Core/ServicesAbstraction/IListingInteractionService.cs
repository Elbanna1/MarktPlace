using Shared.DTOs.Listings;
using Shared.Enums;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IListingInteractionService
{
    Task<ListingViewResultDto> RecordViewAsync(
        ListingModuleType type,
        Guid listingId,
        string? viewerUserId,
        string? ipAddress,
        CancellationToken cancellationToken = default);

    Task<ListingFavoriteResultDto> AddFavoriteAsync(
        string userId, ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    Task<ListingFavoriteResultDto> RemoveFavoriteAsync(
        string userId, ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    Task<PaginatedResult<ListingCardDto>> GetRecentlyViewedAsync(
        string userId, ListingCardFilterParams filter, CancellationToken cancellationToken = default);

    Task<ListingHistoryClearedDto> ClearRecentlyViewedAsync(
        string userId, ListingModuleType? type, CancellationToken cancellationToken = default);

    Task<ListingHistoryClearedDto> RemoveRecentlyViewedAsync(
        string userId, ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    Task<PaginatedResult<ListingCardDto>> GetFavoritesAsync(
        string userId, ListingCardFilterParams filter, CancellationToken cancellationToken = default);

    Task<PaginatedResult<ListingCardDto>> GetSimilarAsync(
        ListingModuleType type,
        Guid listingId,
        SimilarListingFilterParams filter,
        string? viewerUserId,
        CancellationToken cancellationToken = default);

    Task<ListingCountersDto> GetCountersAsync(
        ListingModuleType type, Guid listingId, string? viewerUserId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<(ListingModuleType Type, Guid ListingId), ListingCountersDto>> GetCountersAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        string? viewerUserId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlySet<(ListingModuleType Type, Guid ListingId)>> GetOwnedAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        string ownerUserId,
        CancellationToken cancellationToken = default);

    Task<ListingActionsDto> GetActionsAsync(
        ListingModuleType type, Guid listingId, string? viewerUserId,
        CancellationToken cancellationToken = default);

    ListingInteractionMetadataDto GetMetadata();

    Task<ListingRatingResultDto> RateAsync(
        string reviewerUserId,
        ListingModuleType type,
        Guid listingId,
        RateListingRequest request,
        CancellationToken cancellationToken = default);

    Task<ListingRatingSummaryDto> RemoveRatingAsync(
        string reviewerUserId, ListingModuleType type, Guid listingId,
        CancellationToken cancellationToken = default);

    Task<ListingRatingSummaryDto> GetRatingSummaryAsync(
        ListingModuleType type, Guid listingId, string? viewerUserId,
        CancellationToken cancellationToken = default);

    Task<PaginatedResult<ListingRatingDto>> GetRatingsAsync(
        ListingRatingFilterParams filter, CancellationToken cancellationToken = default);

    Task<ListingRatingDto> GetRatingAsync(Guid id, CancellationToken cancellationToken = default);

    Task DeleteRatingAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ListingReportDto> ReportAsync(
        string reporterUserId,
        ListingModuleType type,
        Guid listingId,
        CreateListingReportRequest request,
        CancellationToken cancellationToken = default);

    Task<PaginatedResult<ListingReportDto>> GetReportsAsync(
        ListingReportFilterParams filter, CancellationToken cancellationToken = default);

    Task<ListingReportDto> UpdateReportAsync(
        Guid reportId, string adminUserId, UpdateListingReportRequest request,
        CancellationToken cancellationToken = default);

    Task PurgeListingAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);
}
