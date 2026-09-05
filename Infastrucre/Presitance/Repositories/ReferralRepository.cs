using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Referrals;
using Shared.Enums;

namespace Persistence.Repositories;

public class ReferralRepository : IReferralRepository
{
    private readonly AppDbContext _context;

    public ReferralRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken = default) =>
        _context.Users.AnyAsync(u => u.ReferralCode == code, cancellationToken);

    public Task<ApplicationUser?> FindUserByCodeAsync(
        string code, CancellationToken cancellationToken = default) =>
        _context.Users.FirstOrDefaultAsync(u => u.ReferralCode == code, cancellationToken);

    public Task<ApplicationUser?> FindUserAsync(
        string userId, CancellationToken cancellationToken = default) =>
        _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

    public async Task AddAsync(Referral referral, CancellationToken cancellationToken = default) =>
        await _context.Referrals.AddAsync(referral, cancellationToken);

    public Task<Referral?> FindByReferredUserAsync(
        string referredUserId, CancellationToken cancellationToken = default) =>
        _context.Referrals.FirstOrDefaultAsync(r => r.ReferredUserId == referredUserId, cancellationToken);

    public Task<Referral?> FindAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Referrals.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public async Task<IReadOnlyDictionary<ReferralStatus, int>> CountByStatusAsync(
        string referrerUserId, CancellationToken cancellationToken = default)
    {
        var rows = await _context.Referrals
            .Where(r => r.ReferrerUserId == referrerUserId)
            .GroupBy(r => r.Status)
            .Select(group => new { Status = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(row => row.Status, row => row.Count);
    }

    public async Task<IReadOnlyDictionary<ReferralStatus, int>> CountAllByStatusAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _context.Referrals
            .GroupBy(r => r.Status)
            .Select(group => new { Status = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(row => row.Status, row => row.Count);
    }

    public Task<int> CountSinceAsync(
        string referrerUserId, DateTime fromUtc, CancellationToken cancellationToken = default) =>
        _context.Referrals
            .CountAsync(r => r.ReferrerUserId == referrerUserId && r.CreatedAt >= fromUtc, cancellationToken);

    public Task<int> CountAllSinceAsync(DateTime fromUtc, CancellationToken cancellationToken = default) =>
        _context.Referrals.CountAsync(r => r.CreatedAt >= fromUtc, cancellationToken);

    public Task<int> CountDistinctReferrersAsync(CancellationToken cancellationToken = default) =>
        _context.Referrals
            .Select(r => r.ReferrerUserId)
            .Distinct()
            .CountAsync(cancellationToken);

    public async Task<(IReadOnlyList<ReferredUserDto> Items, int Total)> GetReferredUsersAsync(
        string referrerUserId, MyReferralFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Referrals
            .AsNoTracking()
            .Where(r => r.ReferrerUserId == referrerUserId);

        if (filter.Status is not null)
            query = query.Where(r => r.Status == filter.Status);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .ThenByDescending(r => r.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(r => new ReferredUserDto
            {
                Id = r.Id,
                Name = (r.Referred.FirstName + " " + r.Referred.SecondName).Trim(),
                RegisteredAt = r.CreatedAt,
                Status = r.Status,
                CompletedAt = r.CompletedAt
            })
            .ToListAsync(cancellationToken);

        foreach (var item in items)
            item.StatusName = ReferralCatalog.NameOf(item.Status);

        return (items, total);
    }

    public async Task<(IReadOnlyList<AdminReferralDto> Items, int Total)> GetForAdminAsync(
        AdminReferralFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Referrals.AsNoTracking(), filter);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .ThenByDescending(r => r.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(ProjectForAdmin)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
            item.StatusName = ReferralCatalog.NameOf(item.Status);

        return (items, total);
    }

    public async Task<AdminReferralDto?> GetAdminReferralAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _context.Referrals
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(ProjectForAdmin)
            .FirstOrDefaultAsync(cancellationToken);

        if (item is not null)
            item.StatusName = ReferralCatalog.NameOf(item.Status);

        return item;
    }

    public async Task<IReadOnlyList<AdminTopReferrerDto>> GetTopReferrersAsync(
        int count, CancellationToken cancellationToken = default)
    {
        var rows = await _context.Referrals
            .AsNoTracking()
            .GroupBy(r => r.ReferrerUserId)
            .Select(group => new
            {
                UserId = group.Key,
                Total = group.Count(),
                Completed = group.Count(r => r.Status == ReferralStatus.Completed)
            })
            .OrderByDescending(row => row.Total)
            .ThenBy(row => row.UserId)
            .Take(count)
            .ToListAsync(cancellationToken);

        if (rows.Count == 0)
            return Array.Empty<AdminTopReferrerDto>();

        var ids = rows.Select(row => row.UserId).ToList();

        var users = await _context.Users
            .AsNoTracking()
            .Where(u => ids.Contains(u.Id))
            .Select(u => new
            {
                u.Id,
                Name = (u.FirstName + " " + u.SecondName).Trim(),
                u.UserName,
                u.ReferralCode
            })
            .ToDictionaryAsync(u => u.Id, cancellationToken);

        return rows
            .Select(row =>
            {
                users.TryGetValue(row.UserId, out var user);

                return new AdminTopReferrerDto
                {
                    UserId = row.UserId,
                    Name = user?.Name ?? string.Empty,
                    UserName = user?.UserName,
                    ReferralCode = user?.ReferralCode,
                    TotalReferrals = row.Total,
                    CompletedReferrals = row.Completed
                };
            })
            .ToList();
    }

    public async Task<(IReadOnlyList<AdminReferrerDto> Items, int Total)> GetReferrersForAdminAsync(
        AdminReferrerFilterParams filter, CancellationToken cancellationToken = default)
    {
        var minimum = filter.MinimumReferrals is > 0 ? filter.MinimumReferrals.Value : 1;

        var grouped = _context.Referrals
            .AsNoTracking()
            .GroupBy(r => r.ReferrerUserId)
            .Select(g => new
            {
                UserId = g.Key,
                Total = g.Count(),
                Completed = g.Count(r => r.Status == ReferralStatus.Completed),
                Pending = g.Count(r => r.Status == ReferralStatus.Pending),
                LastAt = (DateTime?)g.Max(r => r.CreatedAt)
            })
            .Where(row => row.Total >= minimum);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();

            var matching = _context.Users
                .Where(u =>
                    (u.FirstName + " " + u.SecondName).Contains(term) ||
                    (u.UserName != null && u.UserName.Contains(term)) ||
                    (u.ReferralCode != null && u.ReferralCode.Contains(term)))
                .Select(u => u.Id);

            grouped = grouped.Where(row => matching.Contains(row.UserId));
        }

        if (!string.IsNullOrWhiteSpace(filter.UserId))
            grouped = grouped.Where(row => row.UserId == filter.UserId);

        var total = await grouped.CountAsync(cancellationToken);

        if (filter.SortBy == AdminReferrerSortBy.NewestReferrer)
        {
            var newest = _context.Users.Select(u => new { u.Id, u.CreatedAt });

            grouped = grouped
                .OrderByDescending(row => newest.Where(u => u.Id == row.UserId)
                    .Select(u => u.CreatedAt).FirstOrDefault())
                .ThenBy(row => row.UserId);
        }
        else
        {
            grouped = grouped
                .OrderByDescending(row => row.Total)
                .ThenBy(row => row.UserId);
        }

        var rows = await grouped
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        if (rows.Count == 0)
            return (Array.Empty<AdminReferrerDto>(), total);

        var ids = rows.Select(row => row.UserId).ToList();

        var users = await _context.Users
            .AsNoTracking()
            .Where(u => ids.Contains(u.Id))
            .Select(u => new
            {
                u.Id,
                Name = (u.FirstName + " " + u.SecondName).Trim(),
                u.UserName,
                u.Email,
                u.PhoneNumber,
                u.Status,
                u.CreatedAt,
                u.ReferralCode
            })
            .ToDictionaryAsync(u => u.Id, cancellationToken);

        var events = await _context.ReferralLinkEvents
            .AsNoTracking()
            .Where(e => ids.Contains(e.ReferrerUserId))
            .GroupBy(e => new { e.ReferrerUserId, e.EventType })
            .Select(g => new { g.Key.ReferrerUserId, g.Key.EventType, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var shares = events
            .Where(e => e.EventType == ReferralLinkEventType.Share)
            .ToDictionary(e => e.ReferrerUserId, e => e.Count);

        var clicks = events
            .Where(e => e.EventType == ReferralLinkEventType.Click)
            .ToDictionary(e => e.ReferrerUserId, e => e.Count);

        var items = rows
            .Select(row =>
            {
                users.TryGetValue(row.UserId, out var user);

                var clickCount = clicks.GetValueOrDefault(row.UserId);

                return new AdminReferrerDto
                {
                    UserId = row.UserId,
                    Name = user?.Name ?? string.Empty,
                    UserName = user?.UserName,
                    Email = user?.Email,
                    PhoneNumber = user?.PhoneNumber,
                    AccountStatus = user?.Status ?? UserAccountStatus.Active,
                    RegisteredAt = user?.CreatedAt ?? default,
                    ReferralCode = user?.ReferralCode,
                    TotalReferrals = row.Total,
                    CompletedReferrals = row.Completed,
                    PendingReferrals = row.Pending,
                    TotalShares = shares.GetValueOrDefault(row.UserId),
                    TotalClicks = clickCount,
                    LastReferralAt = row.LastAt
                };
            })
            .ToList();

        return (items, total);
    }

    public async Task AddLinkEventAsync(
        ReferralLinkEvent linkEvent, CancellationToken cancellationToken = default)
    {
        await _context.ReferralLinkEvents.AddAsync(linkEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyDictionary<ReferralLinkEventType, int>> CountLinkEventsAsync(
        string referrerUserId, CancellationToken cancellationToken = default) =>
        await _context.ReferralLinkEvents
            .AsNoTracking()
            .Where(e => e.ReferrerUserId == referrerUserId)
            .GroupBy(e => e.EventType)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToDictionaryAsync(row => row.Type, row => row.Count, cancellationToken);

    public async Task<IReadOnlyDictionary<ReferralLinkEventType, int>> CountAllLinkEventsAsync(
        CancellationToken cancellationToken = default) =>
        await _context.ReferralLinkEvents
            .AsNoTracking()
            .GroupBy(e => e.EventType)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToDictionaryAsync(row => row.Type, row => row.Count, cancellationToken);

    private static IQueryable<Referral> Filter(IQueryable<Referral> query, AdminReferralFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.ReferrerUserId))
            query = query.Where(r => r.ReferrerUserId == filter.ReferrerUserId);

        if (!string.IsNullOrWhiteSpace(filter.ReferredUserId))
            query = query.Where(r => r.ReferredUserId == filter.ReferredUserId);

        var code = ReferralCatalog.Normalize(filter.ReferralCode);
        if (code is not null)
            query = query.Where(r => r.ReferralCode == code);

        if (filter.Status is not null)
            query = query.Where(r => r.Status == filter.Status);

        if (filter.FromDate is not null)
            query = query.Where(r => r.CreatedAt >= filter.FromDate);

        if (filter.ToDate is not null)
            query = query.Where(r => r.CreatedAt <= filter.ToDate);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();

            query = query.Where(r =>
                r.Referrer.FirstName.Contains(term) ||
                r.Referrer.SecondName.Contains(term) ||
                (r.Referrer.UserName != null && r.Referrer.UserName.Contains(term)) ||
                r.Referred.FirstName.Contains(term) ||
                r.Referred.SecondName.Contains(term) ||
                (r.Referred.UserName != null && r.Referred.UserName.Contains(term)) ||
                r.ReferralCode.Contains(term));
        }

        return query;
    }

    private static readonly Expression<Func<Referral, AdminReferralDto>> ProjectForAdmin =
        r => new AdminReferralDto
        {
            Id = r.Id,
            ReferrerUserId = r.ReferrerUserId,
            ReferrerName = (r.Referrer.FirstName + " " + r.Referrer.SecondName).Trim(),
            ReferrerUserName = r.Referrer.UserName,
            ReferredUserId = r.ReferredUserId,
            ReferredName = (r.Referred.FirstName + " " + r.Referred.SecondName).Trim(),
            ReferredUserName = r.Referred.UserName,
            ReferralCode = r.ReferralCode,
            Status = r.Status,
            CreatedAt = r.CreatedAt,
            CompletedAt = r.CompletedAt
        };
}
