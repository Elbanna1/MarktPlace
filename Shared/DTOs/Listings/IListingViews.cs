using Shared.Enums;

namespace Shared.DTOs.Listings;

public interface IListingViews
{
    ListingModuleType ViewsListingType { get; }

    Guid ViewsListingId { get; }

    int? Views { get; set; }
}
