using Shared.DTOs.Lookups;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IAdvertiserLookupRepository
{
    Task<(IReadOnlyList<AdvertiserOptionDto> Items, int TotalCount)> GetPagedAsync(
        SubCategoryType subCategory,
        string? search,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);
}
