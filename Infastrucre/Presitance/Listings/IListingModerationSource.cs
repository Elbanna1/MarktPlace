using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Data;
using Shared.Enums;

namespace Persistence.Listings;

public interface IListingModerationSource
{
    ListingModuleType Type { get; }

    Task<IModeratedListing?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public sealed class ListingModerationSource<TEntity> : IListingModerationSource
    where TEntity : class, IModeratedListing
{
    private readonly AppDbContext _context;

    public ListingModerationSource(AppDbContext context, ListingModuleType type)
    {
        _context = context;
        Type = type;
    }

    public ListingModuleType Type { get; }

    public async Task<IModeratedListing?> FindAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        await _context.Set<TEntity>()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
