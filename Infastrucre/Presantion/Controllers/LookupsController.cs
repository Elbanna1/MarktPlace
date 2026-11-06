using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using ServicesAbstraction;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.Lookups.Read;
using Shared.Responses;
using Shared.Catalog;
using Shared.Enums;
using Shared.Exceptions;

using Shared.Constants;
namespace Presentation.Controllers;

[ApiController]
[Route("api/lookups")]
[AllowAnonymous]
[Produces("application/json")]
public class LookupsController : ControllerBase
{
    private const int ReferenceDataMaxAgeSeconds = 300;

    private const int AdvertiserOptionsMaxAgeSeconds = 60;

    private readonly ILookupService _lookupService;
    private readonly ICreateAdFormService _createAdFormService;
    private readonly IReadConfigService _readConfigService;
    private readonly IAdvertiserLookupService _advertiserLookupService;
    private readonly ICatalogConfigurationProvider? _catalog;

    public LookupsController(
        ILookupService lookupService,
        ICreateAdFormService createAdFormService,
        IReadConfigService readConfigService,
        IAdvertiserLookupService advertiserLookupService,
        ICatalogConfigurationProvider? catalog = null)
    {
        _catalog = catalog;
        _lookupService = lookupService;
        _createAdFormService = createAdFormService;
        _readConfigService = readConfigService;
        _advertiserLookupService = advertiserLookupService;
    }

    [HttpGet("create-ad-form/{categoryId:int}/{subCategoryId:int?}")]
    [HttpCache(MaxAgeSeconds = ReferenceDataMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    [ProducesResponseType(typeof(ApiResponse<CreateAdFormDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CreateAdFormDto>>> GetCreateAdForm(
        int categoryId, int? subCategoryId = null)
    {
        if (_catalog is not null)
        {
            var snapshot = await _catalog.GetSnapshotAsync(HttpContext.RequestAborted);

            var context = new CatalogContext(
                subCategoryId is { } sub ? ListingModuleCatalog.ModuleOf(sub) : null, subCategoryId, categoryId);

            if (!snapshot.IsVisible(context, CatalogArea.AdCreation, DateTime.UtcNow))
                throw new NotFoundException(UserMessages.Lookups.CreationUnavailable);
        }

        var result = await _createAdFormService.GetCreateAdFormAsync(
            categoryId, subCategoryId, HttpContext.RequestAborted);
        return Ok(ApiResponse<CreateAdFormDto>.Ok(result, UserMessages.Lookups.FormLoaded));
    }

    [HttpGet("read-config/{categoryId:int}/{subCategoryId:int?}")]
    [HttpCache(MaxAgeSeconds = ReferenceDataMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    [ProducesResponseType(typeof(ApiResponse<ReadConfigDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ReadConfigDto>>> GetReadConfig(
        int categoryId, int? subCategoryId = null)
    {
        var result = await _readConfigService.GetReadConfigAsync(
            categoryId, subCategoryId, HttpContext.RequestAborted);
        return Ok(ApiResponse<ReadConfigDto>.Ok(result, UserMessages.Lookups.ReadConfigLoaded));
    }

    [HttpGet("advertisers/{categoryId:int}/{subCategoryId:int}")]
    [HttpCache(MaxAgeSeconds = AdvertiserOptionsMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdvertiserOptionDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdvertiserOptionDto>>>> GetAdvertisers(
        int categoryId, int subCategoryId, [FromQuery] AdvertiserLookupParams parameters)
    {
        var result = await _advertiserLookupService.GetAdvertisersAsync(
            categoryId, subCategoryId, parameters, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdvertiserOptionDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("categories")]
    [HttpCache(MaxAgeSeconds = ReferenceDataMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CategoryDto>>>> GetCategories()
    {
        var result = await _lookupService.GetCategoriesAsync(HttpContext.RequestAborted);

        if (_catalog is not null)
        {
            var snapshot = await _catalog.GetSnapshotAsync(HttpContext.RequestAborted);
            var now = DateTime.UtcNow;

            result = result
                .Where(category => snapshot.IsVisible(
                    CatalogContext.ForCategory(category.Id), CatalogArea.CategoryListing, now))
                .ToList();
        }

        return Ok(ApiResponse<IReadOnlyList<CategoryDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("categories-tree")]
    [HttpCache(MaxAgeSeconds = ReferenceDataMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CategoryTreeDto>>>> GetCategoriesTree()
    {
        var result = await _lookupService.GetCategoriesTreeAsync(HttpContext.RequestAborted);

        if (_catalog is not null)
        {
            var snapshot = await _catalog.GetSnapshotAsync(HttpContext.RequestAborted);
            var now = DateTime.UtcNow;

            result = result
                .Where(category => snapshot.IsVisible(
                    CatalogContext.ForCategory(category.Id), CatalogArea.CategoryListing, now))
                .Select(category => new CategoryTreeDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    NameAr = category.NameAr,
                    Icon = category.Icon,
                    SortOrder = category.SortOrder,
                    OpensFormDirectly = category.OpensFormDirectly,
                    DirectSubCategoryId = category.DirectSubCategoryId,
                    SubCategories = category.SubCategories
                        .Where(sub => snapshot.IsVisible(
                            new CatalogContext(null, sub.Id, category.Id), CatalogArea.SubCategoryListing, now))
                        .ToList()
                })
                .ToList();
        }

        return Ok(ApiResponse<IReadOnlyList<CategoryTreeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("subcategories/{categoryId:int}")]
    [HttpCache(MaxAgeSeconds = ReferenceDataMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SubCategoryDto>>>> GetSubCategories(int categoryId)
    {
        var result = await _lookupService.GetSubCategoriesAsync(categoryId, HttpContext.RequestAborted);

        if (_catalog is not null)
        {
            var snapshot = await _catalog.GetSnapshotAsync(HttpContext.RequestAborted);
            var now = DateTime.UtcNow;

            result = result
                .Where(sub => snapshot.IsVisible(
                    new CatalogContext(null, sub.Id, sub.CategoryId), CatalogArea.SubCategoryListing, now))
                .ToList();
        }

        return Ok(ApiResponse<IReadOnlyList<SubCategoryDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("features")]
    [HttpCache(MaxAgeSeconds = ReferenceDataMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FeatureDto>>>> GetFeatures(
        [FromQuery] int? subCategoryId = null)
    {
        var result = await _lookupService.GetFeaturesAsync(subCategoryId, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<FeatureDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("listing-types")]
    [HttpCache(MaxAgeSeconds = ReferenceDataMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ListingTypeDto>>>> GetListingTypes()
    {
        var result = await _lookupService.GetListingTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ListingTypeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("governorates")]
    [HttpCache(MaxAgeSeconds = ReferenceDataMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GovernorateDto>>>> GetGovernorates()
    {
        var result = await _lookupService.GetGovernoratesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<GovernorateDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("centers/{governorateId:int}")]
    [HttpCache(MaxAgeSeconds = ReferenceDataMaxAgeSeconds, Scope = HttpCacheScope.Shared)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CenterDto>>>> GetCenters(int governorateId)
    {
        var result = await _lookupService.GetCentersAsync(governorateId, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<CenterDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }
}
