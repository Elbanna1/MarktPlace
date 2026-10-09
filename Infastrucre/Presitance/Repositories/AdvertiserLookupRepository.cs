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

    private static readonly IReadOnlyDictionary<SubCategoryType, Func<AppDbContext, IQueryable<string>>> Sources =
        new Dictionary<SubCategoryType, Func<AppDbContext, IQueryable<string>>>
        {
            [SubCategoryType.Livestock] = context => context.Livestock.Select(x => x.SellerName),
            [SubCategoryType.SheepAndGoats] = context => context.SheepGoats.Select(x => x.SellerName),
            [SubCategoryType.Horses] = context => context.Horses.Select(x => x.SellerName),
            [SubCategoryType.Camels] = context => context.Camels.Select(x => x.SellerName),
            [SubCategoryType.Birds] = context => context.Birds.Select(x => x.SellerName),
            [SubCategoryType.Pets] = context => context.Pets.Select(x => x.SellerName),
            [SubCategoryType.Fish] = context => context.Fishes.Select(x => x.SellerName),
            [SubCategoryType.Bees] = context => context.Bees.Select(x => x.SellerName),
            [SubCategoryType.OtherAnimals] = context => context.OtherAnimals.Select(x => x.SellerName),

            [SubCategoryType.DecorAntiques] = context => context.DecorAntiques.Select(x => x.SellerName),
            [SubCategoryType.Antiques] = context => context.Antiques.Select(x => x.SellerName),
            [SubCategoryType.Paintings] = context => context.Paintings.Select(x => x.SellerName),
            [SubCategoryType.Handmade] = context => context.Handmades.Select(x => x.SellerName),
            [SubCategoryType.CoinsAndStamps] = context => context.CoinStamps.Select(x => x.SellerName),

            [SubCategoryType.Doctors] = context => context.Doctors.Select(x => x.Name),
            [SubCategoryType.MedicalLaboratories] = context => context.MedicalLaboratories.Select(x => x.Name),
            [SubCategoryType.DentalLaboratories] = context => context.DentalLaboratories.Select(x => x.Name),
            [SubCategoryType.RadiologyLaboratories] = context => context.RadiologyLaboratories.Select(x => x.Name),
            [SubCategoryType.MedicalCenters] = context => context.MedicalCenters.Select(x => x.Name),
            [SubCategoryType.Hospitals] = context => context.Hospitals.Select(x => x.Name),

            [SubCategoryType.LawyerOffices] = context => context.LawyerOffices.Select(x => x.Name),
            [SubCategoryType.EngineeringConsultancyOffices] = context => context.EngineeringConsultancyOffices.Select(x => x.Name),
            [SubCategoryType.Contractors] = context => context.Contractors.Select(x => x.Name),
            [SubCategoryType.IndustrialConsultancyOffices] = context => context.IndustrialConsultancyOffices.Select(x => x.Name),
            [SubCategoryType.AccountantOffices] = context => context.AccountantOffices.Select(x => x.Name),

            [SubCategoryType.RestaurantsAndCafes] = context => context.RestaurantsAndCafes.Select(x => x.Name)
        };

    public static IReadOnlyCollection<SubCategoryType> SupportedSubCategories =>
        (IReadOnlyCollection<SubCategoryType>)Sources.Keys;

    public Task<(IReadOnlyList<AdvertiserOptionDto> Items, int TotalCount)> GetPagedAsync(
        SubCategoryType subCategory,
        string? search,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default) =>
        Sources.TryGetValue(subCategory, out var source)
            ? QueryAsync(source(_context), search, pageIndex, pageSize, cancellationToken)
            : Task.FromResult<(IReadOnlyList<AdvertiserOptionDto>, int)>(
                (Array.Empty<AdvertiserOptionDto>(), 0));

    private static async Task<(IReadOnlyList<AdvertiserOptionDto> Items, int TotalCount)> QueryAsync(
        IQueryable<string> names,
        string? search,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken)
    {
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
