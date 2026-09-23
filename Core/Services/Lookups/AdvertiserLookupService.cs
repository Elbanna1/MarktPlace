using ServicesAbstraction;
using Shared.DTOs.Lookups;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Lookups;

public class AdvertiserLookupService : IAdvertiserLookupService
{
    private readonly CategorySelectionResolver _selectionResolver;
    private readonly IAdvertiserLookupRepository _repository;

    public AdvertiserLookupService(
        CategorySelectionResolver selectionResolver, IAdvertiserLookupRepository repository)
    {
        _selectionResolver = selectionResolver;
        _repository = repository;
    }

    public async Task<PaginatedResult<AdvertiserOptionDto>> GetAdvertisersAsync(
        int categoryId,
        int subCategoryId,
        AdvertiserLookupParams parameters,
        CancellationToken cancellationToken = default)
    {
        await _selectionResolver.ResolveAsync(categoryId, subCategoryId, cancellationToken);

        if (!AdvertiserLookupCatalog.Supports(subCategoryId))
            throw new BadRequestException(
                "القسم اللي اخترته مفيهوش فلتر بالمعلن.");

        if (parameters.IsPageIndexOutOfRange)
            return new PaginatedResult<AdvertiserOptionDto>(
                Array.Empty<AdvertiserOptionDto>(), 0, parameters.PageIndex, parameters.PageSize);

        var search = parameters.Search?.Trim();

        if (search?.Length == 0)
            search = null;

        var (items, totalCount) = await _repository.GetPagedAsync(
            (SubCategoryType)subCategoryId,
            search,
            parameters.PageIndex,
            parameters.PageSize,
            cancellationToken);

        return new PaginatedResult<AdvertiserOptionDto>(
            items, totalCount, parameters.PageIndex, parameters.PageSize);
    }
}
