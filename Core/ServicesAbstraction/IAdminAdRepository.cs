using Domain.Entities;
using Shared.DTOs.Admin;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace ServicesAbstraction;

public record AdminAdOwner(string UserId, string? Name, string? PhoneNumber);

public record AdminModuleCount(ListingModuleType Type, int? SubCategoryId, int Count);

public record AdminAdStatusCount(ListingStatus Status, ModerationStatus ModerationStatus, int Count);

public record AdminListingSnapshot(
    IModeratedListing Listing,
    IReadOnlyDictionary<string, object?> Values,
    IReadOnlyList<string> ImageUrls,
    IReadOnlyList<string> VideoUrls);

public interface IAdminAdRepository
{
    Task<(IReadOnlyList<UserListingRow> Items, int TotalCount)> GetPagedAsync(
        AdminAdFilterParams filter, DateTime utcNow, CancellationToken cancellationToken = default,
        bool includeTotal = true);

    Task<IReadOnlyList<AdminAdStatusCount>> GetStatusBreakdownAsync(
        DateTime utcNow, CancellationToken cancellationToken = default);

    Task<int> CountByStatusAsync(
        ModerationStatus status, DateTime utcNow, CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        AdminAdFilterParams filter, DateTime utcNow, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, int>> CountByOwnersAsync(
        IReadOnlyCollection<string> ownerIds, DateTime utcNow,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminModuleCount>> CountByModuleAsync(
        DateTime utcNow, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Shared.DTOs.Admin.AdminTimeSeriesPointDto>> CountByDayAsync(
        DateTime from, DateTime to, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, AdminAdOwner>> GetOwnersAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> FindUserIdsAsync(
        string search, CancellationToken cancellationToken = default);

    Task<IModeratedListing?> FindForModerationAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default);

    Task SaveModerationAsync(ListingModuleType type, CancellationToken cancellationToken = default);

    Task<AdminListingSnapshot?> LoadListingAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default);

    Task<bool> DeleteListingAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default);
}
