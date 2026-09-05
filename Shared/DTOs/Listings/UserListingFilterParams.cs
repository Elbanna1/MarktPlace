using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Listings;

public class UserListingFilterParams : PaginationParams
{
    public int PageNumber
    {
        get => PageIndex;
        set => PageIndex = value;
    }

    public ListingModuleType? Type { get; set; }

    public ListingStatus? Status { get; set; }
}
