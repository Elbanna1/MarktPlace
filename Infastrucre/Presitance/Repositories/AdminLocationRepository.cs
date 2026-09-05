using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;

namespace Persistence.Repositories;

public class AdminLocationRepository : IAdminLocationRepository
{
    private readonly AppDbContext _context;

    public AdminLocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Governorate>> GetGovernoratesAsync(
        CancellationToken cancellationToken = default) =>
        await _context.Governorates
            .AsNoTracking()
            .Include(governorate => governorate.Centers)
                .ThenInclude(center => center.Projects)
            .OrderBy(governorate => governorate.SortOrder)
            .ThenBy(governorate => governorate.Id)
            .ToListAsync(cancellationToken);

    public Task<Governorate?> FindGovernorateAsync(int id, CancellationToken cancellationToken = default) =>
        _context.Governorates
            .Include(governorate => governorate.Centers)
            .FirstOrDefaultAsync(governorate => governorate.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Center>> GetCentersAsync(
        int? governorateId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Centers
            .AsNoTracking()
            .Include(center => center.Governorate)
            .Include(center => center.Projects)
            .AsQueryable();

        if (governorateId is { } id)
            query = query.Where(center => center.GovernorateId == id);

        return await query
            .OrderBy(center => center.SortOrder)
            .ThenBy(center => center.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<Center?> FindCenterAsync(int id, CancellationToken cancellationToken = default) =>
        _context.Centers
            .Include(center => center.Governorate)
            .Include(center => center.Projects)
            .FirstOrDefaultAsync(center => center.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Project>> GetProjectsAsync(
        int? centerId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Projects
            .AsNoTracking()
            .Include(project => project.Center)
                .ThenInclude(center => center.Governorate)
            .AsQueryable();

        if (centerId is { } id)
            query = query.Where(project => project.CenterId == id);

        return await query
            .OrderBy(project => project.SortOrder)
            .ThenBy(project => project.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Project?> FindProjectAsync(int id, CancellationToken cancellationToken = default) =>
        _context.Projects
            .Include(project => project.Center)
                .ThenInclude(center => center.Governorate)
            .FirstOrDefaultAsync(project => project.Id == id, cancellationToken);

    public Task<bool> GovernorateNameExistsAsync(
        string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Governorates.AsNoTracking().Where(item => item.Name == name);

        if (excludeId is { } id)
            query = query.Where(item => item.Id != id);

        return query.AnyAsync(cancellationToken);
    }

    public Task<bool> CenterNameExistsAsync(
        int governorateId, string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Centers
            .AsNoTracking()
            .Where(item => item.GovernorateId == governorateId && item.Name == name);

        if (excludeId is { } id)
            query = query.Where(item => item.Id != id);

        return query.AnyAsync(cancellationToken);
    }

    public Task<bool> ProjectNameExistsAsync(
        int centerId, string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Projects
            .AsNoTracking()
            .Where(item => item.CenterId == centerId && item.Name == name);

        if (excludeId is { } id)
            query = query.Where(item => item.Id != id);

        return query.AnyAsync(cancellationToken);
    }

    public Task<int> CountUsersInGovernorateAsync(
        string name, CancellationToken cancellationToken = default) =>
        _context.Users.CountAsync(user => user.Governorate == name, cancellationToken);

    public Task<int> CountUsersInCenterAsync(
        string name, CancellationToken cancellationToken = default) =>
        _context.Users.CountAsync(user => user.Center == name, cancellationToken);

    public async Task<int> NextGovernorateIdAsync(CancellationToken cancellationToken = default) =>
        await _context.Governorates.MaxAsync(item => (int?)item.Id, cancellationToken) is { } max
            ? max + 1
            : 1;

    public async Task<int> NextCenterIdAsync(CancellationToken cancellationToken = default) =>
        await _context.Centers.MaxAsync(item => (int?)item.Id, cancellationToken) is { } max
            ? max + 1
            : 1;

    public async Task<int> NextProjectIdAsync(CancellationToken cancellationToken = default) =>
        await _context.Projects.MaxAsync(item => (int?)item.Id, cancellationToken) is { } max
            ? max + 1
            : 1;

    public void AddGovernorate(Governorate governorate) => _context.Governorates.Add(governorate);

    public void RemoveGovernorate(Governorate governorate) => _context.Governorates.Remove(governorate);

    public void AddCenter(Center center) => _context.Centers.Add(center);

    public void RemoveCenter(Center center) => _context.Centers.Remove(center);

    public void AddProject(Project project) => _context.Projects.Add(project);

    public void RemoveProject(Project project) => _context.Projects.Remove(project);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
