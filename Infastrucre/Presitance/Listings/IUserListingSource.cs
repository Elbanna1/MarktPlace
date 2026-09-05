using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public interface IUserListingSource
{
    ListingModuleType Type { get; }

    IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false);
}
