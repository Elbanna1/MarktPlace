using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Lookups;
using Shared.Enums;

namespace Persistence.Repositories;

public class AdvertiserLookupRepository : IAdvertiserLookupRepository
{
    private const string EscapeCharacter = "\\";

    private readonly AppDbContext _context;

    public AdvertiserLookupRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<(IReadOnlyList<AdvertiserOptionDto> Items, int TotalCount)> GetPagedAsync(
        SubCategoryType subCategory,
        string? search,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default) =>
        subCategory switch
        {
            SubCategoryType.Livestock =>
                QueryAsync(_context.Livestock, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.SheepAndGoats =>
                QueryAsync(_context.SheepGoats, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.Horses =>
                QueryAsync(_context.Horses, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.Camels =>
                QueryAsync(_context.Camels, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.Birds =>
                QueryAsync(_context.Birds, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.Pets =>
                QueryAsync(_context.Pets, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.Fish =>
                QueryAsync(_context.Fishes, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.Bees =>
                QueryAsync(_context.Bees, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.OtherAnimals =>
                QueryAsync(_context.OtherAnimals, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),

            SubCategoryType.DecorAntiques =>
                QueryAsync(_context.DecorAntiques, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.Antiques =>
                QueryAsync(_context.Antiques, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.Paintings =>
                QueryAsync(_context.Paintings, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.Handmade =>
                QueryAsync(_context.Handmades, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),
            SubCategoryType.CoinsAndStamps =>
                QueryAsync(_context.CoinStamps, x => x.SellerName, search, pageIndex, pageSize, cancellationToken),

            _ => Task.FromResult<(IReadOnlyList<AdvertiserOptionDto>, int)>(
                (Array.Empty<AdvertiserOptionDto>(), 0))
        };

    private static async Task<(IReadOnlyList<AdvertiserOptionDto> Items, int TotalCount)> QueryAsync<TEntity>(
        IQueryable<TEntity> set,
        System.Linq.Expressions.Expression<Func<TEntity, string>> advertiser,
        string? search,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        var names = set.AsNoTracking().Select(advertiser);

        if (!string.IsNullOrEmpty(search))
        {
            var pattern = $"%{Escape(search)}%";
            names = names.Where(name => EF.Functions.Like(name, pattern, EscapeCharacter));
        }

        var grouped = names.GroupBy(name => name);

        var totalCount = await grouped.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<AdvertiserOptionDto>(), 0);

        var skip = (int)Math.Clamp((long)(pageIndex - 1) * pageSize, 0, int.MaxValue);

        if (skip >= totalCount)
            return (Array.Empty<AdvertiserOptionDto>(), totalCount);

        var items = await grouped
            .Select(group => new AdvertiserOptionDto
            {
                Id = group.Key,
                Name = group.Key,
                ListingsCount = group.Count()
            })
            .OrderBy(option => option.Name)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    private static string Escape(string search) => search
        .Replace(EscapeCharacter, EscapeCharacter + EscapeCharacter, StringComparison.Ordinal)
        .Replace("%", EscapeCharacter + "%", StringComparison.Ordinal)
        .Replace("_", EscapeCharacter + "_", StringComparison.Ordinal)
        .Replace("[", EscapeCharacter + "[", StringComparison.Ordinal);
}
