using FluentValidation;
using Shared.Constants;
using Shared.DTOs.RestaurantsAndCafes;

namespace Services.Validation;

public class CreateRestaurantAndCafeRequestValidator : AbstractValidator<CreateRestaurantAndCafeRequest>
{
    public CreateRestaurantAndCafeRequestValidator() =>
        this.AddDirectoryRules(RestaurantsAndCafesCatalog.RestaurantsAndCafes, isCreate: true);
}

public class UpdateRestaurantAndCafeRequestValidator : AbstractValidator<UpdateRestaurantAndCafeRequest>
{
    public UpdateRestaurantAndCafeRequestValidator() =>
        this.AddDirectoryRules(RestaurantsAndCafesCatalog.RestaurantsAndCafes, isCreate: false);
}

public class RestaurantAndCafeFilterParamsValidator : DirectoryFilterParamsValidator<RestaurantAndCafeFilterParams>;
