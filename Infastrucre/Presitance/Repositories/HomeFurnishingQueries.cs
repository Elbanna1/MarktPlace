using Microsoft.EntityFrameworkCore;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Persistence.Repositories;

internal static class HomeFurnishingQueries
{
    public static IQueryable<TListing> ApplyCommonFilters<TListing>(
        IQueryable<TListing> query, HomeFurnishingFilterParamsBase filter)
        where TListing : class
    {
        if (!string.IsNullOrWhiteSpace(filter.Center))
        {
            var center = filter.Center.Trim();
            query = query.Where(x => EF.Property<string>(x, "Center") == center);
        }

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => EF.Property<decimal>(x, "Price") >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => EF.Property<decimal>(x, "Price") <= priceTo);

        if (filter.Negotiable is { } negotiable)
            query = query.Where(x => EF.Property<bool>(x, "Negotiable") == negotiable);

        if (filter.IsFeatured is { } isFeatured)
            query = query.Where(x => EF.Property<bool>(x, "IsFeatured") == isFeatured);

        if (filter.IsPremium is { } isPremium)
            query = query.Where(x => EF.Property<bool>(x, "IsPremium") == isPremium);

        if (filter.IsUrgent is { } isUrgent)
            query = query.Where(x => EF.Property<bool>(x, "IsUrgent") == isUrgent);

        return query;
    }

    public static IQueryable<TListing> ApplyOrdering<TListing>(
        IQueryable<TListing> query, HomeFurnishingSortBy sortBy)
        where TListing : class
    {
        var promoted = query
            .OrderByDescending(x => EF.Property<bool>(x, "IsPremium"))
            .ThenByDescending(x => EF.Property<bool>(x, "IsFeatured"));

        return sortBy switch
        {
            HomeFurnishingSortBy.Oldest => promoted
                .ThenBy(x => EF.Property<DateTime>(x, "CreatedAt"))
                .ThenBy(x => EF.Property<Guid>(x, "Id")),

            HomeFurnishingSortBy.PriceAsc => promoted
                .ThenBy(x => EF.Property<decimal>(x, "Price"))
                .ThenByDescending(x => EF.Property<Guid>(x, "Id")),

            HomeFurnishingSortBy.PriceDesc => promoted
                .ThenByDescending(x => EF.Property<decimal>(x, "Price"))
                .ThenByDescending(x => EF.Property<Guid>(x, "Id")),

            HomeFurnishingSortBy.MostViewed => promoted
                .ThenByDescending(x => EF.Property<int>(x, "ViewCount"))
                .ThenByDescending(x => EF.Property<Guid>(x, "Id")),

            _ => promoted
                .ThenByDescending(x => EF.Property<DateTime>(x, "CreatedAt"))
                .ThenByDescending(x => EF.Property<Guid>(x, "Id"))
        };
    }
}
