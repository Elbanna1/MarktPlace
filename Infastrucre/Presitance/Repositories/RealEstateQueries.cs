using Microsoft.EntityFrameworkCore;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Persistence.Repositories;

internal static class RealEstateQueries
{
    public static IQueryable<TListing> ApplyCommonFilters<TListing>(
        IQueryable<TListing> query, RealEstateFilterParamsBase filter)
        where TListing : class
    {
        if (filter.ListingType is { } listingType)
            query = query.Where(x => EF.Property<RealEstateListingType>(x, "ListingType") == listingType);

        if (!string.IsNullOrWhiteSpace(filter.Governorate))
        {
            var governorate = filter.Governorate.Trim();
            query = query.Where(x => EF.Property<string>(x, "Governorate") == governorate);
        }

        if (!string.IsNullOrWhiteSpace(filter.Center))
        {
            var center = filter.Center.Trim();
            query = query.Where(x => EF.Property<string>(x, "Center") == center);
        }

        if (filter.Project is { } project)
            query = query.Where(x => EF.Property<RealEstateProject?>(x, "Project") == project);

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
        IQueryable<TListing> query, RealEstateSortBy sortBy, string priceColumn)
        where TListing : class
    {
        var promoted = query
            .OrderByDescending(x => EF.Property<bool>(x, "IsPremium"))
            .ThenByDescending(x => EF.Property<bool>(x, "IsFeatured"));

        return sortBy switch
        {
            RealEstateSortBy.Oldest => promoted
                .ThenBy(x => EF.Property<DateTime>(x, "CreatedAt"))
                .ThenBy(x => EF.Property<Guid>(x, "Id")),

            RealEstateSortBy.PriceAsc => promoted
                .ThenBy(x => EF.Property<decimal>(x, priceColumn))
                .ThenByDescending(x => EF.Property<Guid>(x, "Id")),

            RealEstateSortBy.PriceDesc => promoted
                .ThenByDescending(x => EF.Property<decimal>(x, priceColumn))
                .ThenByDescending(x => EF.Property<Guid>(x, "Id")),

            RealEstateSortBy.MostViewed => promoted
                .ThenByDescending(x => EF.Property<int>(x, "ViewCount"))
                .ThenByDescending(x => EF.Property<Guid>(x, "Id")),

            _ => promoted
                .ThenByDescending(x => EF.Property<DateTime>(x, "CreatedAt"))
                .ThenByDescending(x => EF.Property<Guid>(x, "Id"))
        };
    }
}
