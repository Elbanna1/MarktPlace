using Domain.Entities;
using Persistence.Data;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class CommercialShopListingSource(AppDbContext context)
    : DirectoryListingSource<CommercialShop, CommercialShopImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.CommercialShop;
}

public sealed class GeneralServiceListingSource(AppDbContext context)
    : DirectoryListingSource<GeneralService, GeneralServiceImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.GeneralService;
}

public sealed class ResortHotelAmusementListingSource(AppDbContext context)
    : DirectoryListingSource<ResortHotelAmusement, ResortHotelAmusementImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.ResortHotelAmusement;
}
