using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Data;
using Persistence.Listings;
using ServicesAbstraction;
using Shared.Enums;

namespace Persistence.Repositories;

public class ListingLifecycleRepository : IListingLifecycleRepository
{
    private readonly AppDbContext _context;
    private readonly IEnumerable<IListingModerationSource> _sources;

    public ListingLifecycleRepository(
        AppDbContext context, IEnumerable<IListingModerationSource> sources)
    {
        _context = context;
        _sources = sources;
    }

    public async Task<IExpiringListing?> FindAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default)
    {
        var source = _sources.FirstOrDefault(s => s.Type == type);

        if (source is null)
            return null;

        return await source.FindAsync(id, cancellationToken) as IExpiringListing;
    }

    public Task SaveAsync(ListingModuleType type, CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public async Task<IReadOnlyList<ExpiringListingRow>> GetWindowsClosingBetweenAsync(
        DateTime fromUtc, DateTime toUtc, CancellationToken cancellationToken = default)
    {
        var rows = new List<ExpiringListingRow>();

        foreach (var (module, entityType) in ModerationModuleRegistry.EntityTypes)
        {
            if (!typeof(IExpiringListing).IsAssignableFrom(entityType))
                continue;

            var method = ScanMethod.MakeGenericMethod(entityType);

            var task = (Task<List<ExpiringListingRow>>)method.Invoke(
                this, new object[] { module, fromUtc, toUtc, cancellationToken })!;

            rows.AddRange(await task);
        }

        return rows;
    }

    private static readonly System.Reflection.MethodInfo ScanMethod =
        typeof(ListingLifecycleRepository).GetMethod(
            nameof(ScanAsync),
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;

    private async Task<List<ExpiringListingRow>> ScanAsync<TEntity>(
        ListingModuleType module, DateTime fromUtc, DateTime toUtc,
        CancellationToken cancellationToken)
        where TEntity : class, IModeratedListing, IExpiringListing
    {
        var entities = await _context.Set<TEntity>()
            .AsNoTracking()
            .IncludingUnmoderated()
            .Where(x =>
                x.ModerationStatus == ModerationStatus.Approved &&
                x.ExpireAt != null &&
                x.ExpireAt >= fromUtc &&
                x.ExpireAt < toUtc)
            .ToListAsync(cancellationToken);

        return entities
            .Select(x => new ExpiringListingRow(
                module,
                (Guid)_context.Entry(x).Property("Id").CurrentValue!,
                x.OwnerUserId,
                x.ListingTitle,
                x.ExpireAt!.Value,
                x.PublishedAt))
            .ToList();
    }
}
