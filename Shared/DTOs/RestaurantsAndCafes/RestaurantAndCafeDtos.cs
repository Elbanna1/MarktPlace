using Shared.DTOs.Directory;
using Shared.Enums;

namespace Shared.DTOs.RestaurantsAndCafes;

public class CreateRestaurantAndCafeRequest : CreateDirectoryListingRequest;

public class UpdateRestaurantAndCafeRequest : CreateRestaurantAndCafeRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class RestaurantAndCafeDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.RestaurantAndCafe;
}

public class RestaurantAndCafeFilterParams : DirectoryFilterParams;
