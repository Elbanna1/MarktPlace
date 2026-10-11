using System.Text.Json.Serialization;
using Shared.DTOs.Directory;
using Shared.Enums;

namespace Shared.DTOs.GovernorateGuide;

public class CreateCommercialShopRequest : CreateDirectoryListingRequest
{
    public ShopSuitableActivity? ActivityType { get; set; }

    public string? OtherActivityType { get; set; }
}

public class UpdateCommercialShopRequest : CreateCommercialShopRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class CommercialShopDto : DirectoryListingDto
{
    public ShopSuitableActivity? ActivityType { get; set; }

    public string? ActivityTypeName { get; set; }

    public string? OtherActivityType { get; set; }

    public override ListingModuleType StatsListingType => ListingModuleType.CommercialShop;
}

public class CommercialShopFilterParams : DirectoryFilterParams
{
    public ShopSuitableActivity? ActivityType { get; set; }
}

public class CreateGeneralServiceRequest : CreateDirectoryListingRequest
{
    public GeneralServiceType? ServiceType { get; set; }

    public GeneralServiceSubtype? FixedServiceSubtype { get; set; }

    public GeneralServiceSubtype? MobileServiceSubtype { get; set; }

    [JsonIgnore]
    public GeneralServiceSubtype? SelectedSubtype => ServiceType switch
    {
        GeneralServiceType.Fixed => FixedServiceSubtype,
        GeneralServiceType.Mobile => MobileServiceSubtype,
        _ => null
    };
}

public class UpdateGeneralServiceRequest : CreateGeneralServiceRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class GeneralServiceDto : DirectoryListingDto
{
    public GeneralServiceType ServiceType { get; set; }

    public string ServiceTypeName { get; set; } = default!;

    public GeneralServiceSubtype ServiceSubtype { get; set; }

    public string ServiceSubtypeName { get; set; } = default!;

    public override ListingModuleType StatsListingType => ListingModuleType.GeneralService;
}

public class GeneralServiceFilterParams : DirectoryFilterParams
{
    public GeneralServiceType? ServiceType { get; set; }

    public GeneralServiceSubtype? ServiceSubtype { get; set; }
}

public class CreateResortHotelAmusementRequest : CreateDirectoryListingRequest;

public class UpdateResortHotelAmusementRequest : CreateResortHotelAmusementRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class ResortHotelAmusementDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.ResortHotelAmusement;
}

public class ResortHotelAmusementFilterParams : DirectoryFilterParams;
