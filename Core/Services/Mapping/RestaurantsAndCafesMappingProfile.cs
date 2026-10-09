using AutoMapper;
using Domain.Entities;
using Shared.DTOs.RestaurantsAndCafes;

namespace Services.Mapping;

public class RestaurantsAndCafesMappingProfile : Profile
{
    public RestaurantsAndCafesMappingProfile()
    {
        this.MapDirectoryListing<RestaurantAndCafe, RestaurantAndCafeImage, RestaurantAndCafeDto>();
    }
}
