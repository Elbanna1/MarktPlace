using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.RestaurantsAndCafes;

namespace Presentation.Controllers;

[Route(RestaurantsAndCafesRoutes.RestaurantsAndCafes)]
[Tags("Restaurants & Cafes")]
public class RestaurantsAndCafesController(IRestaurantAndCafeService service)
    : DirectoryControllerBase<CreateRestaurantAndCafeRequest, UpdateRestaurantAndCafeRequest,
        RestaurantAndCafeDto, RestaurantAndCafeFilterParams>(service);
