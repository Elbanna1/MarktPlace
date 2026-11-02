using Domain.Entities;
using Shared.DTOs.Notifications;
using Shared.Enums;

namespace ServicesAbstraction;

public sealed record NotificationDelivery(string UserId, Guid ReferenceId, DateTime LastCreatedAt);

public interface INotificationRepository
{
    Task<IReadOnlyList<NotificationDelivery>> FindDeliveriesAsync(
        IReadOnlyCollection<string> userIds,
        NotificationType type,
        IReadOnlyCollection<Guid> referenceIds,
        CancellationToken cancellationToken = default);

    Task AddAsync(Notification notification);

    Task AddRangeAsync(
        IEnumerable<Notification> notifications, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetPagedForUserAsync(
        string userId, NotificationFilterParams filter, CancellationToken cancellationToken = default);

    Task<Notification?> GetAsync(Guid id, string userId);

    Task<int> CountUnreadAsync(string userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, int>> CountUnreadAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default);

    Task<int> MarkAllAsReadAsync(string userId, DateTime readAt);

    void Remove(Notification notification);

    Task<int> DeleteAllAsync(string userId);

    Task<bool> ExistsAsync(
        string userId, NotificationType type, Guid? referenceId, DateTime? createdAfterUtc = null);

    Task<IReadOnlySet<string>> FindRecipientsWithAsync(
        IReadOnlyCollection<string> userIds,
        NotificationType type,
        Guid? referenceId,
        DateTime? createdAfterUtc = null,
        CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync();
}
