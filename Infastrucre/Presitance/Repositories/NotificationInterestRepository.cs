using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Enums;

namespace Persistence.Repositories;

public class NotificationInterestRepository : INotificationInterestRepository
{
    private readonly AppDbContext _context;

    public NotificationInterestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<UserNotificationInterest>> GetForUserAsync(
        string userId, CancellationToken cancellationToken = default) =>
        await _context.UserNotificationInterests
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.CategoryId)
            .ThenBy(i => i.SubCategoryId ?? 0)
            .ToListAsync(cancellationToken);

    public Task<UserNotificationInterest?> FindAsync(
        string userId, Guid interestId, CancellationToken cancellationToken = default) =>
        _context.UserNotificationInterests
            .FirstOrDefaultAsync(i => i.Id == interestId && i.UserId == userId, cancellationToken);

    public Task<UserNotificationInterest?> FindByKeyAsync(
        string userId, int categoryId, int? subCategoryId, CancellationToken cancellationToken = default) =>
        _context.UserNotificationInterests.FirstOrDefaultAsync(
            i => i.UserId == userId && i.CategoryId == categoryId && i.SubCategoryId == subCategoryId,
            cancellationToken);

    public async Task AddAsync(
        UserNotificationInterest interest, CancellationToken cancellationToken = default) =>
        await _context.UserNotificationInterests.AddAsync(interest, cancellationToken);

    public void Remove(UserNotificationInterest interest) =>
        _context.UserNotificationInterests.Remove(interest);

    public void RemoveRange(IEnumerable<UserNotificationInterest> interests) =>
        _context.UserNotificationInterests.RemoveRange(interests);

    public Task<UserNotificationPreference?> GetPreferenceAsync(
        string userId, CancellationToken cancellationToken = default) =>
        _context.UserNotificationPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

    public async Task AddPreferenceAsync(
        UserNotificationPreference preference, CancellationToken cancellationToken = default) =>
        await _context.UserNotificationPreferences.AddAsync(preference, cancellationToken);

    public async Task<IReadOnlyList<string>> GetMatchingUserIdsAsync(
        int categoryId,
        int? subCategoryId,
        string excludeUserId,
        string? afterUserId,
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        var muted = _context.UserNotificationPreferences
            .Where(p => !p.NewListingsEnabled)
            .Select(p => p.UserId);

        var followers = _context.UserNotificationInterests
            .Where(i =>
                i.IsEnabled &&
                i.CategoryId == categoryId &&
                (i.SubCategoryId == null || i.SubCategoryId == subCategoryId))
            .Select(i => i.UserId);

        var haveChosen = _context.UserNotificationInterests
            .Where(i => i.IsEnabled)
            .Select(i => i.UserId);

        return await _context.Users
            .AsNoTracking()
            .Where(u =>
                u.Status == UserAccountStatus.Active &&
                u.Id != excludeUserId &&
                !muted.Contains(u.Id) &&
                (followers.Contains(u.Id) || !haveChosen.Contains(u.Id)) &&

                (afterUserId == null || string.Compare(u.Id, afterUserId) > 0))
            .OrderBy(u => u.Id)
            .Take(batchSize)
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> TryBeginDispatchAsync(
        ListingModuleType listingType, Guid listingId, CancellationToken cancellationToken = default)
    {
        var alreadyDispatched = await _context.ListingNotificationDispatches
            .AsNoTracking()
            .AnyAsync(d => d.ListingType == listingType && d.ListingId == listingId, cancellationToken);

        if (alreadyDispatched)
            return false;

        var entry = _context.ListingNotificationDispatches.Add(new ListingNotificationDispatch
        {
            ListingType = listingType,
            ListingId = listingId,
            DispatchedAt = DateTime.UtcNow,
            RecipientCount = 0
        });

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException)
        {
            entry.State = EntityState.Detached;
            return false;
        }
    }

    public Task CompleteDispatchAsync(
        ListingModuleType listingType, Guid listingId, int recipientCount,
        CancellationToken cancellationToken = default) =>
        _context.ListingNotificationDispatches
            .Where(d => d.ListingType == listingType && d.ListingId == listingId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(d => d.RecipientCount, recipientCount),
                cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
