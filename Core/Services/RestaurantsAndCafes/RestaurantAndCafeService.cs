using AutoMapper;
using Domain.Entities;
using Services.Directory;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.RestaurantsAndCafes;

namespace Services.RestaurantsAndCafes;

public sealed class RestaurantAndCafeService
    : DirectoryListingService<RestaurantAndCafe, RestaurantAndCafeImage, CreateRestaurantAndCafeRequest,
            UpdateRestaurantAndCafeRequest, RestaurantAndCafeDto, RestaurantAndCafeFilterParams>,
        IRestaurantAndCafeService
{
    public RestaurantAndCafeService(
        IDirectoryRepository<RestaurantAndCafe, RestaurantAndCafeImage, RestaurantAndCafeFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => RestaurantsAndCafesCatalog.RestaurantsAndCafes;
}
