using Shared.DTOs.RestaurantsAndCafes;

namespace ServicesAbstraction;

public interface IRestaurantAndCafeService
    : IDirectoryListingService<CreateRestaurantAndCafeRequest, UpdateRestaurantAndCafeRequest,
        RestaurantAndCafeDto, RestaurantAndCafeFilterParams>;
