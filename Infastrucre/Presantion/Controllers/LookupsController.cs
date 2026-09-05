using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.Lookups.Read;
using Shared.Responses;

using Shared.Constants;
namespace Presentation.Controllers;

[ApiController]
[Route("api/lookups")]
[AllowAnonymous]
[Produces("application/json")]
public class LookupsController : ControllerBase
{
    private readonly ILookupService _lookupService;
    private readonly ICreateAdFormService _createAdFormService;
    private readonly IReadConfigService _readConfigService;

    public LookupsController(
        ILookupService lookupService,
        ICreateAdFormService createAdFormService,
        IReadConfigService readConfigService)
    {
        _lookupService = lookupService;
        _createAdFormService = createAdFormService;
        _readConfigService = readConfigService;
    }

    [HttpGet("create-ad-form/{categoryId:int}/{subCategoryId:int?}")]
    [ProducesResponseType(typeof(ApiResponse<CreateAdFormDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CreateAdFormDto>>> GetCreateAdForm(
        int categoryId, int? subCategoryId = null)
    {
        var result = await _createAdFormService.GetCreateAdFormAsync(
            categoryId, subCategoryId, HttpContext.RequestAborted);
        return Ok(ApiResponse<CreateAdFormDto>.Ok(result, UserMessages.Lookups.FormLoaded));
    }

    [HttpGet("read-config/{categoryId:int}/{subCategoryId:int?}")]
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

    [HttpGet("categories")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CategoryDto>>>> GetCategories()
    {
        var result = await _lookupService.GetCategoriesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<CategoryDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("categories-tree")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CategoryTreeDto>>>> GetCategoriesTree()
    {
        var result = await _lookupService.GetCategoriesTreeAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<CategoryTreeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("subcategories/{categoryId:int}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SubCategoryDto>>>> GetSubCategories(int categoryId)
    {
        var result = await _lookupService.GetSubCategoriesAsync(categoryId, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<SubCategoryDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("features")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FeatureDto>>>> GetFeatures(
        [FromQuery] int? subCategoryId = null)
    {
        var result = await _lookupService.GetFeaturesAsync(subCategoryId, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<FeatureDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("listing-types")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ListingTypeDto>>>> GetListingTypes()
    {
        var result = await _lookupService.GetListingTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ListingTypeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("governorates")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GovernorateDto>>>> GetGovernorates()
    {
        var result = await _lookupService.GetGovernoratesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<GovernorateDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("centers/{governorateId:int}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CenterDto>>>> GetCenters(int governorateId)
    {
        var result = await _lookupService.GetCentersAsync(governorateId, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<CenterDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }
}
