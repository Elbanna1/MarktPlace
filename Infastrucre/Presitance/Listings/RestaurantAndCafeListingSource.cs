using Domain.Entities;
using Persistence.Data;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class RestaurantAndCafeListingSource(AppDbContext context)
    : DirectoryListingSource<RestaurantAndCafe, RestaurantAndCafeImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.RestaurantAndCafe;
}
