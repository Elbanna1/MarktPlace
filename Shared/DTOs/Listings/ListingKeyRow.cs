using Shared.Enums;

namespace Shared.DTOs.Listings;

public class ListingKeyRow
{
    public ListingModuleType Type { get; set; }

    public Guid Id { get; set; }
}

public class UserListingStatusRow
{
    public ListingModuleType Type { get; set; }

    public ListingStatus Status { get; set; }
}
