using Shared.Enums;

namespace Domain.Entities;

public class CommercialShop : DirectoryListing<CommercialShopImage>
{
    public ShopSuitableActivity? ActivityType { get; set; }

    public string? OtherActivityType { get; set; }
}

public class CommercialShopImage : DirectoryListingImage
{
    public CommercialShop Listing { get; set; } = default!;
}

public class GeneralService : DirectoryListing<GeneralServiceImage>
{
    public GeneralServiceType ServiceType { get; set; }

    public GeneralServiceSubtype ServiceSubtype { get; set; }
}

public class GeneralServiceImage : DirectoryListingImage
{
    public GeneralService Listing { get; set; } = default!;
}

public class ResortHotelAmusement : DirectoryListing<ResortHotelAmusementImage>;

public class ResortHotelAmusementImage : DirectoryListingImage
{
    public ResortHotelAmusement Listing { get; set; } = default!;
}
