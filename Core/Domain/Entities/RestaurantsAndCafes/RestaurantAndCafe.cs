namespace Domain.Entities;

public class RestaurantAndCafe : DirectoryListing<RestaurantAndCafeImage>;

public class RestaurantAndCafeImage : DirectoryListingImage
{
    public RestaurantAndCafe Listing { get; set; } = default!;
}
