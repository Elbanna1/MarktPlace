using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Notifications;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly IRealtimeNotifier _realtimeNotifier;
    private readonly IMapper _mapper;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        INotificationRepository repository,
        IRealtimeNotifier realtimeNotifier,
        IMapper mapper,
        ILogger<NotificationService> logger)
    {
        _repository = repository;
        _realtimeNotifier = realtimeNotifier;
        _mapper = mapper;
        _logger = logger;
    }

    public Task NotifyAsync(
        string userId,
        ListingModuleType module,
        NotificationAction action,
        Guid entityId,
        string? entityTitle = null)
    {
        var subCategory = ListingModuleCatalog.SubCategoryOf(module);
        var category = ListingModuleCatalog.CategoryOf(module);

        var content = NotificationCatalog
            .Build(NotificationCatalog.For(module), action, entityId, entityTitle)
            .WithListing(
                listingType: module,
                categoryId: (int?)category,
                subCategoryId: (int?)subCategory);

        return CreateAsync(userId, content, entityId, action);
    }

    public async Task NotifyAsync(
        string userId,
        NotificationSubject subject,
        NotificationAction action,
        Guid? entityId = null,
        string? entityTitle = null)
    {
        var content = NotificationCatalog.Build(subject, action, entityId, entityTitle);

        await CreateAsync(userId, content, entityId, action);
    }

    public async Task<NotificationDto> CreateAsync(
        string userId,
        NotificationContent content,
        Guid? referenceId = null,
        NotificationAction action = NotificationAction.Created)
    {
        var notification = Build(userId, content, referenceId, action, DateTime.UtcNow);

        await _repository.AddAsync(notification);
        await _repository.SaveChangesAsync();

        var dto = _mapper.Map<NotificationDto>(notification);
        await PushAsync(userId, dto);
        return dto;
    }

    public async Task<int> CreateManyAsync(
        IReadOnlyCollection<string> userIds,
        NotificationContent content,
        Guid? referenceId = null,
        NotificationAction action = NotificationAction.Created,
        CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
            return 0;

        var createdAt = DateTime.UtcNow;

        var notifications = userIds
            .Select(userId => Build(userId, content, referenceId, action, createdAt))
            .ToList();

        await _repository.AddRangeAsync(notifications, cancellationToken);
        await _repository.SaveChangesAsync();

        await PushManyAsync(notifications, cancellationToken);

        return notifications.Count;
    }

    public async Task<NotificationDto> CreateAsync(
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
        string? imageUrl = null)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            Action = action,
            Icon = icon,
            EntityName = entityName,
            DeepLink = deepLink,
            ReferenceId = referenceId,
            ReferenceType = referenceType,
            ListingType = listingType,
            CategoryId = categoryId,
            SubCategoryId = subCategoryId,
            ImageUrl = imageUrl,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(notification);
        await _repository.SaveChangesAsync();

        var dto = _mapper.Map<NotificationDto>(notification);
        await PushAsync(userId, dto);
        return dto;
    }

    public async Task<bool> CreateIfNotExistsAsync(
        string userId,
        NotificationContent content,
        Guid? referenceId,
        DateTime? createdAfterUtc = null,
        NotificationAction action = NotificationAction.Created,
        CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(userId, content.Type, referenceId, createdAfterUtc))
            return false;

        await CreateAsync(userId, content, referenceId, action);
        return true;
    }

    public async Task<int> CreateManyIfNotExistAsync(
        IReadOnlyCollection<string> userIds,
        NotificationContent content,
        Guid? referenceId,
        DateTime? createdAfterUtc = null,
        NotificationAction action = NotificationAction.Created,
        CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
            return 0;

        var alreadyNotified = await _repository.FindRecipientsWithAsync(
            userIds, content.Type, referenceId, createdAfterUtc, cancellationToken);

        var recipients = userIds
            .Distinct(StringComparer.Ordinal)
            .Where(userId => !alreadyNotified.Contains(userId))
            .ToList();

        if (recipients.Count == 0)
            return 0;

        return await CreateManyAsync(recipients, content, referenceId, action, cancellationToken);
    }

    public async Task<bool> CreateIfNotExistsAsync(
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
        string? deepLink = null)
    {
        if (await _repository.ExistsAsync(userId, type, referenceId, createdAfterUtc))
            return false;

        await CreateAsync(userId, title, message, type, referenceId, referenceType,
            action, icon, entityName, deepLink);
        return true;
    }

    public async Task<PaginatedResult<NotificationDto>> GetMyNotificationsAsync(
        string userId, NotificationFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedForUserAsync(
            userId, filter, cancellationToken);

        var mapped = _mapper.Map<IReadOnlyList<NotificationDto>>(items);
        return new PaginatedResult<NotificationDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default) =>
        _repository.CountUnreadAsync(userId, cancellationToken);

    public async Task MarkAsReadAsync(string userId, Guid notificationId)
    {
        var notification = await _repository.GetAsync(notificationId, userId)
            ?? throw new NotFoundException("الإشعار مش موجود.");

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
        }

        await PushUnreadCountAsync(userId);
    }

    public async Task<int> MarkAllAsReadAsync(string userId)
    {
        var updated = await _repository.MarkAllAsReadAsync(userId, DateTime.UtcNow);
        await PushUnreadCountAsync(userId);
        return updated;
    }

    public async Task DeleteAsync(string userId, Guid notificationId)
    {
        var notification = await _repository.GetAsync(notificationId, userId)
            ?? throw new NotFoundException("الإشعار مش موجود.");

        _repository.Remove(notification);
        await _repository.SaveChangesAsync();

        await PushUnreadCountAsync(userId);
    }

    public async Task<int> DeleteAllAsync(string userId)
    {
        var removed = await _repository.DeleteAllAsync(userId);
        await PushUnreadCountAsync(userId);
        return removed;
    }

    private async Task PushAsync(string userId, NotificationDto dto)
    {
        try
        {
            await _realtimeNotifier.NotifyAsync(userId, dto);
            await PushUnreadCountAsync(userId);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception,
                "Real-time delivery of a notification to user {UserId} failed; it is stored and will still be listed.",
                userId);
        }
    }

    private async Task PushUnreadCountAsync(string userId)
    {
        try
        {
            var count = await _repository.CountUnreadAsync(userId);
            await _realtimeNotifier.NotifyUnreadCountAsync(userId, count);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception,
                "Real-time unread-count push to user {UserId} failed.", userId);
        }
    }

    private async Task PushManyAsync(
        IReadOnlyList<Notification> notifications, CancellationToken cancellationToken)
    {
        try
        {
            var userIds = notifications.Select(n => n.UserId).Distinct().ToList();
            var unreadCounts = await _repository.CountUnreadAsync(userIds, cancellationToken);

            foreach (var notification in notifications)
            {
                var dto = _mapper.Map<NotificationDto>(notification);

                await _realtimeNotifier.NotifyAsync(notification.UserId, dto, cancellationToken);
                await _realtimeNotifier.NotifyUnreadCountAsync(
                    notification.UserId,
                    unreadCounts.GetValueOrDefault(notification.UserId),
                    cancellationToken);
            }
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception,
                "Real-time delivery of a batch of {Count} notifications failed; they are stored and will still be listed.",
                notifications.Count);
        }
    }

    private static Notification Build(
        string userId,
        NotificationContent content,
        Guid? referenceId,
        NotificationAction action,
        DateTime createdAt) => new()
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = content.Title,
        Message = content.Message,
        Type = content.Type,
        Action = action,
        Icon = content.Icon,
        EntityName = content.EntityName,
        DeepLink = content.DeepLink,
        ReferenceId = referenceId,
        ReferenceType = content.ReferenceType,
        ListingType = content.ListingType,
        CategoryId = content.CategoryId,
        SubCategoryId = content.SubCategoryId,
        ImageUrl = content.ImageUrl,
        CategoryName = content.CategoryName,
        SubCategoryName = content.SubCategoryName,
        ListingTitle = content.ListingTitle,
        OwnerId = content.OwnerId,
        OwnerName = content.OwnerName,
        IsRead = false,

        CreatedAt = createdAt
    };
}
