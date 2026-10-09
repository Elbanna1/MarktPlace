using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Directory;
using Shared.Enums;

namespace Persistence.Repositories;

public abstract class DirectoryRepository<TListing, TImage, TFilter> : IDirectoryRepository<TListing, TImage, TFilter>
    where TListing : DirectoryListing<TImage>
    where TImage : DirectoryListingImage
    where TFilter : DirectoryFilterParams
{
    protected const string EscapeCharacter = "\\";

    protected readonly AppDbContext Context;

    private readonly DirectoryModule _module;

    protected DirectoryRepository(AppDbContext context, DirectoryModule module)
    {
        Context = context;
        _module = module;
    }

    private DbSet<TListing> Set => Context.Set<TListing>();

    private static IQueryable<TListing> Include(IQueryable<TListing> query) =>
        query.Include(x => x.Images);

    private static async Task<List<TListing>> ReadAsync(IQueryable<TListing> query, CancellationToken cancellationToken)
    {
        var rows = await query
            .Select(x => new
            {
                Listing = x,
                Images = x.Images.OrderBy(image => image.SortOrder).ToList(),
                x.User.FirstName,
                x.User.SecondName
            })
            .ToListAsync(cancellationToken);

        return rows.Select(row =>
        {
            row.Listing.Images = row.Images;
            row.Listing.User = new ApplicationUser
            {
                Id = row.Listing.UserId,
                FirstName = row.FirstName,
                SecondName = row.SecondName
            };
            return row.Listing;
        }).ToList();
    }

    protected virtual IQueryable<TListing> Filter(IQueryable<TListing> query, TFilter filter) => query;

    protected virtual IQueryable<TListing> ApplySearch(IQueryable<TListing> query, string pattern) =>
        query.Where(x =>
            EF.Functions.Like(x.Name, pattern, EscapeCharacter) ||
            EF.Functions.Like(x.Title, pattern, EscapeCharacter) ||
            EF.Functions.Like(x.Description, pattern, EscapeCharacter) ||
            EF.Functions.Like(x.Address, pattern, EscapeCharacter));

    public async Task<TListing?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default,
        bool includeUnmoderated = false)
    {
        var visible = Set.VisibleToViewer(includeUnmoderated, Context.ViewerUserId).Where(x => x.Id == id);

        if (!asNoTracking)
            return await Include(visible).FirstOrDefaultAsync(cancellationToken);

        return (await ReadAsync(visible.AsNoTracking().Take(1), cancellationToken)).FirstOrDefault();
    }

    public Task<TListing?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default) =>
        Include(Set)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<TListing> Items, int TotalCount)> GetPagedAsync(
        TFilter filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(Set.AsNoTracking(), filter);

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            if (_module.NameFilterIsExact)
            {
                var name = filter.Name.Trim();
                query = query.Where(x => x.Name == name);
            }
            else
            {
                var name = Contains(filter.Name);
                query = query.Where(x => EF.Functions.Like(x.Name, name, EscapeCharacter));
            }
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = ApplySearch(query, Contains(filter.Search));

        var total = await query.CountAsync(cancellationToken);

        if (total == 0)
            return (Array.Empty<TListing>(), 0);

        var skip = (int)Math.Clamp((long)(filter.PageIndex - 1) * filter.PageSize, 0, int.MaxValue);

        var items = await ReadAsync(
            Sort(query, filter.SortBy).Skip(skip).Take(filter.PageSize), cancellationToken);

        return (items, total);
    }

    private static IQueryable<TListing> Sort(IQueryable<TListing> query, DirectorySortBy sortBy) =>
        sortBy switch
        {
            DirectorySortBy.Oldest => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            DirectorySortBy.MostViewed => query.OrderByDescending(x => x.ViewCount)
                .ThenByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id),
            _ => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

    protected static string Contains(string value) => $"%{Escape(value.Trim())}%";

    private static string Escape(string value) => value
        .Replace(EscapeCharacter, EscapeCharacter + EscapeCharacter, StringComparison.Ordinal)
        .Replace("%", EscapeCharacter + "%", StringComparison.Ordinal)
        .Replace("_", EscapeCharacter + "_", StringComparison.Ordinal)
        .Replace("[", EscapeCharacter + "[", StringComparison.Ordinal);

    public async Task AddAsync(TListing listing, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(listing, cancellationToken);

    public async Task AddImagesAsync(IEnumerable<TImage> images, CancellationToken cancellationToken = default) =>
        await Context.Set<TImage>().AddRangeAsync(images, cancellationToken);

    public void RemoveImage(TImage image) => Context.Set<TImage>().Remove(image);

    public void Update(TListing listing) => Set.Update(listing);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        Context.SaveChangesAsync(cancellationToken);
}
