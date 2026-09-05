using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.JobRequests;

namespace Persistence.Repositories;

public class JobRequestRepository : IJobRequestRepository
{
    private readonly AppDbContext _context;

    public JobRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<JobRequest?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<JobRequest> query = _context.JobRequests;

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
    }

    public Task<JobRequest?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.JobRequests
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<JobRequest> Items, int TotalCount)> GetPagedAsync(
        JobRequestFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.JobRequests.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(j =>
                EF.Functions.Like(j.ApplicantName, $"%{search}%") ||
                EF.Functions.Like(j.Skills, $"%{search}%") ||
                EF.Functions.Like(j.Title, $"%{search}%") ||
                EF.Functions.Like(j.Description, $"%{search}%"));
        }

        if (filter.JobField is { } jobField)
            query = query.Where(j => j.JobField == jobField);

        if (filter.Experience is { } experience)
            query = query.Where(j => j.Experience == experience);

        if (filter.Education is { } education)
            query = query.Where(j => j.Education == education);

        if (!string.IsNullOrWhiteSpace(filter.Center))
        {
            var center = filter.Center.Trim();
            query = query.Where(j => j.Center == center);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<JobRequest>(), 0);

        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .ThenByDescending(j => j.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<List<JobFieldLookup>> GetJobFieldsAsync(CancellationToken cancellationToken = default) =>
        _context.JobFields.AsNoTracking().OrderBy(j => j.Id).ToListAsync(cancellationToken);

    public Task<List<JobExperienceLevelLookup>> GetExperienceLevelsAsync(
        CancellationToken cancellationToken = default) =>
        _context.JobExperienceLevels.AsNoTracking().OrderBy(e => e.Id).ToListAsync(cancellationToken);

    public Task<List<EducationLevelLookup>> GetEducationLevelsAsync(
        CancellationToken cancellationToken = default) =>
        _context.EducationLevels.AsNoTracking().OrderBy(e => e.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(JobRequest jobRequest) =>
        await _context.JobRequests.AddAsync(jobRequest);

    public void Update(JobRequest jobRequest) =>
        _context.JobRequests.Update(jobRequest);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
