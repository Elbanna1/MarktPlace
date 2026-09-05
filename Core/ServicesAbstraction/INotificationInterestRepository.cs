using Domain.Entities;
using Shared.Enums;

namespace ServicesAbstraction;

public interface INotificationInterestRepository
{
    Task<IReadOnlyList<UserNotificationInterest>> GetForUserAsync(
        string userId, CancellationToken cancellationToken = default);

    Task<UserNotificationInterest?> FindAsync(
        string userId, Guid interestId, CancellationToken cancellationToken = default);

    Task<UserNotificationInterest?> FindByKeyAsync(
        string userId, int categoryId, int? subCategoryId, CancellationToken cancellationToken = default);

    Task AddAsync(UserNotificationInterest interest, CancellationToken cancellationToken = default);

    void Remove(UserNotificationInterest interest);

    void RemoveRange(IEnumerable<UserNotificationInterest> interests);

    Task<UserNotificationPreference?> GetPreferenceAsync(
        string userId, CancellationToken cancellationToken = default);

    Task AddPreferenceAsync(
        UserNotificationPreference preference, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetMatchingUserIdsAsync(
        int categoryId,
        int? subCategoryId,
        string excludeUserId,
        string? afterUserId,
        int batchSize,
        CancellationToken cancellationToken = default);

    Task<bool> TryBeginDispatchAsync(
        ListingModuleType listingType, Guid listingId, CancellationToken cancellationToken = default);

    Task CompleteDispatchAsync(
        ListingModuleType listingType, Guid listingId, int recipientCount,
        CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
