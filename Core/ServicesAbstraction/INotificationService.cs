using Shared.Constants;
using Shared.DTOs.Notifications;
using Shared.Enums;
using Shared.Responses;

namespace ServicesAbstraction;

public interface INotificationService
{
    Task NotifyAsync(
        string userId,
        ListingModuleType module,
        NotificationAction action,
        Guid entityId,
        string? entityTitle = null);

    Task NotifyAsync(
        string userId,
        NotificationSubject subject,
        NotificationAction action,
        Guid? entityId = null,
        string? entityTitle = null);

    Task<NotificationDto> CreateAsync(
        string userId,
        NotificationContent content,
        Guid? referenceId = null,
        NotificationAction action = NotificationAction.Created);

    Task<int> CreateManyAsync(
        IReadOnlyCollection<string> userIds,
        NotificationContent content,
        Guid? referenceId = null,
        NotificationAction action = NotificationAction.Created,
        CancellationToken cancellationToken = default);

    Task<NotificationDto> CreateAsync(
        string userId,
        string title,
        string message,
        NotificationType type,
        Guid? referenceId = null,
        string? referenceType = null,
        NotificationAction action = NotificationAction.Created,
        string? icon = null,
        string? entityName = null,
        string? deepLink = null,
        ListingModuleType? listingType = null,
        int? categoryId = null,
        int? subCategoryId = null,
        string? imageUrl = null);

    Task<bool> CreateIfNotExistsAsync(
        string userId,
        NotificationContent content,
        Guid? referenceId,
        DateTime? createdAfterUtc = null,
        NotificationAction action = NotificationAction.Created,
        CancellationToken cancellationToken = default);

    Task<int> CreateManyIfNotExistAsync(
        IReadOnlyCollection<string> userIds,
        NotificationContent content,
        Guid? referenceId,
        DateTime? createdAfterUtc = null,
        NotificationAction action = NotificationAction.Created,
        CancellationToken cancellationToken = default);

    Task<bool> CreateIfNotExistsAsync(
        string userId,
        string title,
        string message,
        NotificationType type,
        Guid? referenceId,
        string? referenceType = null,
        DateTime? createdAfterUtc = null,
        NotificationAction action = NotificationAction.Created,
        string? icon = null,
        string? entityName = null,
        string? deepLink = null);

    Task<PaginatedResult<NotificationDto>> GetMyNotificationsAsync(
        string userId, NotificationFilterParams filter, CancellationToken cancellationToken = default);

    Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default);

    Task MarkAsReadAsync(string userId, Guid notificationId);

    Task<int> MarkAllAsReadAsync(string userId);

    Task DeleteAsync(string userId, Guid notificationId);

    Task<int> DeleteAllAsync(string userId);
}
