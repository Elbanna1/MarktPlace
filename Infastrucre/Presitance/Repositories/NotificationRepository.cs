using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Notifications;
using Shared.Enums;

namespace Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification) =>
        await _context.Notifications.AddAsync(notification);

    public async Task AddRangeAsync(
        IEnumerable<Notification> notifications, CancellationToken cancellationToken = default) =>
        await _context.Notifications.AddRangeAsync(notifications, cancellationToken);

    public async Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetPagedForUserAsync(
        string userId, NotificationFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications.AsNoTracking()
            .Where(n => n.UserId == userId);

        if (filter.UnreadOnly)
            query = query.Where(n => !n.IsRead);

        if (filter.Type is { } type)
            query = query.Where(n => n.Type == type);

        if (!string.IsNullOrWhiteSpace(filter.ReferenceType))
        {
            var referenceType = filter.ReferenceType.Trim();
            query = query.Where(n => n.ReferenceType == referenceType);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(n =>
                EF.Functions.Like(n.Title, $"%{search}%") ||
                EF.Functions.Like(n.Message, $"%{search}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Notification>(), 0);

        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .ThenByDescending(n => n.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<Notification?> GetAsync(Guid id, string userId) =>
        _context.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

    public Task<int> CountUnreadAsync(string userId, CancellationToken cancellationToken = default) =>
        _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

    public async Task<IReadOnlyDictionary<string, int>> CountUnreadAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
            return new Dictionary<string, int>();

        var counts = await _context.Notifications
            .AsNoTracking()
            .Where(n => userIds.Contains(n.UserId) && !n.IsRead)
            .GroupBy(n => n.UserId)
            .Select(group => new { UserId = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        return counts.ToDictionary(row => row.UserId, row => row.Count);
    }

    public Task<int> MarkAllAsReadAsync(string userId, DateTime readAt) =>
        _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(n => n.IsRead, true)
                .SetProperty(n => n.ReadAt, readAt));

    public void Remove(Notification notification) =>
        _context.Notifications.Remove(notification);

    public Task<int> DeleteAllAsync(string userId) =>
        _context.Notifications.Where(n => n.UserId == userId).ExecuteDeleteAsync();

    public Task<bool> ExistsAsync(
        string userId, NotificationType type, Guid? referenceId, DateTime? createdAfterUtc = null) =>
        _context.Notifications.AnyAsync(n =>
            n.UserId == userId &&
            n.Type == type &&
            n.ReferenceId == referenceId &&
            (createdAfterUtc == null || n.CreatedAt > createdAfterUtc));

    public async Task<IReadOnlySet<string>> FindRecipientsWithAsync(
        IReadOnlyCollection<string> userIds,
        NotificationType type,
        Guid? referenceId,
        DateTime? createdAfterUtc = null,
        CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
            return new HashSet<string>(StringComparer.Ordinal);

        var found = await _context.Notifications
            .AsNoTracking()
            .Where(n =>
                userIds.Contains(n.UserId) &&
                n.Type == type &&
                n.ReferenceId == referenceId &&
                (createdAfterUtc == null || n.CreatedAt > createdAfterUtc))
            .Select(n => n.UserId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return found.ToHashSet(StringComparer.Ordinal);
    }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
