using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Enums;

namespace Persistence.Repositories;

public class CatalogConfigurationRepository : ICatalogConfigurationRepository
{
    private readonly AppDbContext _context;

    public CatalogConfigurationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CatalogConfiguration>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.CatalogConfigurations.AsNoTracking().ToListAsync(cancellationToken);

    public Task<List<CatalogConfiguration>> GetTrackedAsync(
        CatalogConfigurationKind kind, CancellationToken cancellationToken = default) =>
        _context.CatalogConfigurations.Where(configuration => configuration.Kind == kind).ToListAsync(cancellationToken);

    public void Add(CatalogConfiguration configuration) => _context.CatalogConfigurations.Add(configuration);

    public void Remove(CatalogConfiguration configuration) => _context.CatalogConfigurations.Remove(configuration);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
