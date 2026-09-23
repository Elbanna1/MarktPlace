using Shared.DTOs.Lookups;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IAdvertiserLookupService
{
    Task<PaginatedResult<AdvertiserOptionDto>> GetAdvertisersAsync(
        int categoryId,
        int subCategoryId,
        AdvertiserLookupParams parameters,
        CancellationToken cancellationToken = default);
}
