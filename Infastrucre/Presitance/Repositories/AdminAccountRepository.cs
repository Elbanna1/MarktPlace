using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Admin;

namespace Persistence.Repositories;

public class AdminAccountRepository : IAdminAccountRepository
{
    private readonly AppDbContext _context;

    public AdminAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AdminPageGrant>> GetGrantsAsync(
        string adminUserId, CancellationToken cancellationToken = default) =>
        await _context.AdminPageGrants
            .AsNoTracking()
            .Include(grant => grant.Permissions)
            .Where(grant => grant.AdminUserId == adminUserId)
            .ToListAsync(cancellationToken);

    public async Task<int> GetPermissionMaskAsync(
        string adminUserId, string pageKey, CancellationToken cancellationToken = default)
    {
        var permissions = await _context.AdminPagePermissions
            .AsNoTracking()
            .Where(permission =>
                permission.Grant!.AdminUserId == adminUserId &&
                permission.Grant.PageKey == pageKey)
            .Select(permission => permission.Permission)
            .ToListAsync(cancellationToken);

        var mask = 0;

        foreach (var permission in permissions)
            mask |= permission;

        return mask;
    }

    public async Task<IReadOnlyDictionary<string, int>> CountPagesAsync(
        IReadOnlyCollection<string> adminUserIds, CancellationToken cancellationToken = default)
    {
        if (adminUserIds.Count == 0)
            return new Dictionary<string, int>();

        var counts = await _context.AdminPageGrants
            .AsNoTracking()
            .Where(grant => adminUserIds.Contains(grant.AdminUserId))
            .GroupBy(grant => grant.AdminUserId)
            .Select(group => new { AdminUserId = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        return counts.ToDictionary(entry => entry.AdminUserId, entry => entry.Count);
    }

    public async Task ReplaceGrantsAsync(
        string adminUserId, IReadOnlyList<(string PageKey, IReadOnlyList<int> Permissions)> grants,
        string? grantedBy, CancellationToken cancellationToken = default)
    {
        var existing = await _context.AdminPageGrants
            .Include(grant => grant.Permissions)
            .Where(grant => grant.AdminUserId == adminUserId)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var requested = grants.ToDictionary(grant => grant.PageKey, StringComparer.OrdinalIgnoreCase);

        foreach (var grant in existing.Where(grant => !requested.ContainsKey(grant.PageKey)))
            _context.AdminPageGrants.Remove(grant);

        foreach (var (pageKey, permissions) in grants)
        {
            var grant = existing.FirstOrDefault(candidate =>
                string.Equals(candidate.PageKey, pageKey, StringComparison.OrdinalIgnoreCase));

            if (grant is null)
            {
                grant = new AdminPageGrant
                {
                    AdminUserId = adminUserId,
                    PageKey = pageKey,
                    CreatedAt = now,
                    GrantedBy = grantedBy
                };

                _context.AdminPageGrants.Add(grant);
            }
            else
            {
                grant.UpdatedAt = now;
                grant.GrantedBy = grantedBy;
            }

            foreach (var permission in grant.Permissions
                         .Where(permission => !permissions.Contains(permission.Permission))
                         .ToList())
            {
                grant.Permissions.Remove(permission);
                _context.AdminPagePermissions.Remove(permission);
            }

            foreach (var value in permissions.Where(value =>
                         grant.Permissions.All(permission => permission.Permission != value)))
            {
                var permission = new AdminPagePermission
                {
                    Grant = grant,
                    Permission = value,
                    CreatedAt = now
                };

                _context.AdminPagePermissions.Add(permission);
                grant.Permissions.Add(permission);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAllGrantsAsync(string adminUserId, CancellationToken cancellationToken = default)
    {
        var grants = await _context.AdminPageGrants
            .Where(grant => grant.AdminUserId == adminUserId)
            .ToListAsync(cancellationToken);

        if (grants.Count == 0)
            return;

        _context.AdminPageGrants.RemoveRange(grants);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ApplicationUser>> SearchUsersAsync(
        AdminCandidateSearchParams filter, IReadOnlyCollection<string> excludedUserIds,
        CancellationToken cancellationToken = default)
    {
        var term = (filter.Q ?? string.Empty).Trim();

        if (term.Length < AdminCandidateSearchParams.MinimumTermLength)
            return Array.Empty<ApplicationUser>();

        var query = _context.Users.AsNoTracking();

        if (excludedUserIds.Count > 0)
            query = query.Where(user => !excludedUserIds.Contains(user.Id));

        query = query.Where(user =>
            (user.UserName != null && EF.Functions.Like(user.UserName, $"%{term}%")) ||
            (user.Email != null && EF.Functions.Like(user.Email, $"%{term}%")) ||
            (user.PhoneNumber != null && EF.Functions.Like(user.PhoneNumber, $"%{term}%")) ||
            EF.Functions.Like(user.FirstName, $"%{term}%") ||
            EF.Functions.Like(user.SecondName, $"%{term}%") ||
            EF.Functions.Like(user.FirstName + " " + user.SecondName, $"%{term}%"));

        return await query
            .OrderBy(user => user.UserName)
            .ThenBy(user => user.Id)
            .Take(filter.Limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<ApplicationUser> Items, int TotalCount)> GetPagedAsync(
        AdminAccountFilterParams filter, IReadOnlyCollection<string> adminUserIds,
        CancellationToken cancellationToken = default)
    {
        if (adminUserIds.Count == 0)
            return (Array.Empty<ApplicationUser>(), 0);

        var query = _context.Users
            .AsNoTracking()
            .Where(user => adminUserIds.Contains(user.Id));

        if (filter.IsActive is { } isActive)
        {
            query = isActive
                ? query.Where(user => user.Status == Shared.Enums.UserAccountStatus.Active)
                : query.Where(user => user.Status != Shared.Enums.UserAccountStatus.Active);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();

            query = query.Where(user =>
                EF.Functions.Like(user.FirstName, $"%{term}%") ||
                EF.Functions.Like(user.SecondName, $"%{term}%") ||
                (user.UserName != null && EF.Functions.Like(user.UserName, $"%{term}%")) ||
                (user.PhoneNumber != null && EF.Functions.Like(user.PhoneNumber, $"%{term}%")) ||
                (user.Email != null && EF.Functions.Like(user.Email, $"%{term}%")));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<ApplicationUser>(), 0);

        var items = await query
            .OrderByDescending(user => user.CreatedAt)
            .ThenBy(user => user.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
