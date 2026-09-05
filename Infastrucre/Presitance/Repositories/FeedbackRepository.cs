using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Feedback;

namespace Persistence.Repositories;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly AppDbContext _context;

    public FeedbackRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Feedback?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Feedback> query = _context.Feedbacks
            .Include(f => f.Images)
            .Include(f => f.User);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Feedback> Items, int TotalCount)> GetPagedAsync(
        string? ownerId, FeedbackFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Feedbacks.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(ownerId))
            query = query.Where(f => f.UserId == ownerId);
        else if (!string.IsNullOrWhiteSpace(filter.UserId))
            query = query.Where(f => f.UserId == filter.UserId);

        if (filter.Type is { } type)
            query = query.Where(f => f.Type == type);

        if (filter.Status is { } status)
            query = query.Where(f => f.Status == status);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(f =>
                EF.Functions.Like(f.Title, $"%{term}%") ||
                EF.Functions.Like(f.Description, $"%{term}%"));
        }

        if (filter.FromDate is { } from)
            query = query.Where(f => f.CreatedAt >= from);

        if (filter.ToDate is { } to)
            query = query.Where(f => f.CreatedAt <= to);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Feedback>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(f => f.CreatedAt)
                .ThenByDescending(f => f.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(f => f.Images)
                .Include(f => f.User)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Feedback feedback) =>
        await _context.Feedbacks.AddAsync(feedback);

    public void Update(Feedback feedback) =>
        _context.Feedbacks.Update(feedback);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
