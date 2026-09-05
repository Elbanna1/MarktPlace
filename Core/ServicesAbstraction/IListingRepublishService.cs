using Shared.DTOs.Listings;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IListingRepublishService
{
    Task<ListingRepublishResultDto> RepublishAsync(
        string userId, ListingModuleType type, Guid listingId,
        CancellationToken cancellationToken = default);
}
