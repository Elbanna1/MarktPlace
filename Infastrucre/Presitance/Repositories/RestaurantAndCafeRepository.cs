using Domain.Entities;
using Persistence.Data;
using Shared.Constants;
using Shared.DTOs.RestaurantsAndCafes;

namespace Persistence.Repositories;

public sealed class RestaurantAndCafeRepository
    : DirectoryRepository<RestaurantAndCafe, RestaurantAndCafeImage, RestaurantAndCafeFilterParams>
{
    public RestaurantAndCafeRepository(AppDbContext context)
        : base(context, RestaurantsAndCafesCatalog.RestaurantsAndCafes) { }
}
