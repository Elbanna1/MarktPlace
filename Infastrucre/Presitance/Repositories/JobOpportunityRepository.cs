using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.JobOpportunities;

namespace Persistence.Repositories;

public class JobOpportunityRepository : IJobOpportunityRepository
{
    private readonly AppDbContext _context;

    public JobOpportunityRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<JobOpportunity?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<JobOpportunity> query = _context.JobOpportunities.Include(j => j.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
    }

    public Task<JobOpportunity?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.JobOpportunities
            .Include(j => j.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<JobOpportunity> Items, int TotalCount)> GetPagedAsync(
        JobOpportunityFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.JobOpportunities.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(j =>
                EF.Functions.Like(j.JobTitle, $"%{search}%") ||
                EF.Functions.Like(j.EmployerName, $"%{search}%") ||
                EF.Functions.Like(j.Title, $"%{search}%") ||
                EF.Functions.Like(j.Description, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.JobTitle))
        {
            var jobTitle = filter.JobTitle.Trim();
            query = query.Where(j => EF.Functions.Like(j.JobTitle, $"%{jobTitle}%"));
        }

        if (filter.JobField is { } jobField)
            query = query.Where(j => j.JobField == jobField);

        if (filter.WorkType is { } workType)
            query = query.Where(j => j.WorkType == workType);

        if (filter.RequiredExperience is { } requiredExperience)
            query = query.Where(j => j.RequiredExperience == requiredExperience);

        if (!string.IsNullOrWhiteSpace(filter.Center))
        {
            var center = filter.Center.Trim();
            query = query.Where(j => j.Center == center);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<JobOpportunity>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(j => j.CreatedAt)
                .ThenByDescending(j => j.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(j => j.Images)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<JobFieldLookup>> GetJobFieldsAsync(CancellationToken cancellationToken = default) =>
        _context.JobFields.AsNoTracking().OrderBy(j => j.Id).ToListAsync(cancellationToken);

    public Task<List<JobExperienceLevelLookup>> GetExperienceLevelsAsync(
        CancellationToken cancellationToken = default) =>
        _context.JobExperienceLevels.AsNoTracking().OrderBy(e => e.Id).ToListAsync(cancellationToken);

    public Task<List<WorkTypeLookup>> GetWorkTypesAsync(CancellationToken cancellationToken = default) =>
        _context.WorkTypes.AsNoTracking().OrderBy(w => w.Id).ToListAsync(cancellationToken);

    public Task<List<SalaryTypeLookup>> GetSalaryTypesAsync(CancellationToken cancellationToken = default) =>
        _context.SalaryTypes.AsNoTracking().OrderBy(s => s.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(JobOpportunity jobOpportunity) =>
        await _context.JobOpportunities.AddAsync(jobOpportunity);

    public void Update(JobOpportunity jobOpportunity) =>
        _context.JobOpportunities.Update(jobOpportunity);

    public void RemoveImage(JobOpportunityImage image) =>
        _context.JobOpportunityImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<JobOpportunityImage> images) =>
        await _context.JobOpportunityImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
