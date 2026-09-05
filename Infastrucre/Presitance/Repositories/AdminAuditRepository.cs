using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Admin;

namespace Persistence.Repositories;

public class AdminAuditRepository : IAdminAuditRepository
{
    private readonly AppDbContext _context;

    public AdminAuditRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AdminAuditLog entry, CancellationToken cancellationToken = default)
    {
        _context.AdminAuditLogs.Add(entry);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<AdminAuditLog> Items, int TotalCount)> GetPagedAsync(
        AdminAuditLogFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.AdminAuditLogs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.AdminUserId))
        {
            var adminId = filter.AdminUserId.Trim();
            query = query.Where(log => log.AdminUserId == adminId);
        }

        if (filter.Action is { } action)
            query = query.Where(log => log.Action == action);

        if (!string.IsNullOrWhiteSpace(filter.TargetType))
        {
            var target = filter.TargetType.Trim();
            query = query.Where(log => log.TargetType == target);
        }

        if (!string.IsNullOrWhiteSpace(filter.TargetId))
        {
            var targetId = filter.TargetId.Trim();
            query = query.Where(log => log.TargetId == targetId);
        }

        if (filter.DateFrom is { } from)
            query = query.Where(log => log.CreatedAt >= from);

        if (filter.DateTo is { } to)
            query = query.Where(log => log.CreatedAt <= to);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();

            query = query.Where(log =>
                EF.Functions.Like(log.Description, $"%{term}%") ||
                (log.AdminName != null && EF.Functions.Like(log.AdminName, $"%{term}%")) ||
                (log.TargetId != null && EF.Functions.Like(log.TargetId, $"%{term}%")));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<AdminAuditLog>(), 0);

        query = ApplySort(query, filter.Sort);

        var items = await query
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<AdminAuditLog?> FindAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.AdminAuditLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(log => log.Id == id, cancellationToken);

    public async Task<IReadOnlyList<AdminAuditAuthorDto>> GetAuthorsAsync(
        CancellationToken cancellationToken = default) =>
        await _context.AdminAuditLogs
            .AsNoTracking()
            .GroupBy(log => log.AdminUserId)
            .Select(group => new AdminAuditAuthorDto
            {
                AdminUserId = group.Key,

                AdminName = group.OrderByDescending(log => log.CreatedAt).First().AdminName,
                Count = group.Count()
            })
            .OrderByDescending(author => author.Count)
            .ToListAsync(cancellationToken);

    private static IQueryable<AdminAuditLog> ApplySort(IQueryable<AdminAuditLog> query, string? sort)
    {
        var descending = string.IsNullOrWhiteSpace(sort) || sort.StartsWith('-');
        var field = (sort ?? string.Empty).TrimStart('-').Trim().ToLowerInvariant();

        return field switch
        {
            "action" => descending
                ? query.OrderByDescending(log => log.Action).ThenByDescending(log => log.Id)
                : query.OrderBy(log => log.Action).ThenBy(log => log.Id),

            _ => descending
                ? query.OrderByDescending(log => log.CreatedAt).ThenByDescending(log => log.Id)
                : query.OrderBy(log => log.CreatedAt).ThenBy(log => log.Id)
        };
    }
}
