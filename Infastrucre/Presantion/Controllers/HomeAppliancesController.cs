using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;
using Shared.Exceptions;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route(HomeFurnishingRoutes.HomeAppliances)]
[Authorize]
[Produces("application/json")]
public class HomeAppliancesController : ControllerBase
{
    private readonly IHomeApplianceService _service;

    public HomeAppliancesController(IHomeApplianceService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxHomeFurnishingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<HomeApplianceDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<HomeApplianceDetailsDto>>> Create(
        [FromForm] CreateHomeApplianceRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(CurrentUserId, request, images, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<HomeApplianceDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<HomeApplianceListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<HomeApplianceListItemDto>>>> GetList(
        [FromQuery] HomeApplianceFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<HomeApplianceListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<HomeApplianceDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<HomeApplianceDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, countView: true, HttpContext.RequestAborted);
        return Ok(ApiResponse<HomeApplianceDetailsDto>.Ok(
            result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxHomeFurnishingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<HomeApplianceDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<HomeApplianceDetailsDto>>> Update(
        Guid id, [FromForm] UpdateHomeApplianceRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, video, cancellationToken);

        return Ok(ApiResponse<HomeApplianceDetailsDto>.Ok(
            result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpPut("{id:guid}/images/order")]
    [ProducesResponseType(typeof(ApiResponse<HomeApplianceDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<HomeApplianceDetailsDto>>> ReorderImages(
        Guid id, [FromBody] ReorderHomeFurnishingImagesRequest request)
    {
        var result = await _service.ReorderImagesAsync(
            CurrentUserId, IsAdmin, id, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<HomeApplianceDetailsDto>.Ok(result, UserMessages.Listings.ImagesReordered));
    }

    [HttpPut("{id:guid}/promotion")]
    [ProducesResponseType(typeof(ApiResponse<HomeApplianceDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<HomeApplianceDetailsDto>>> SetPromotion(
        Guid id, [FromBody] PromoteHomeFurnishingRequest request)
    {
        if (!IsAdmin)
            throw new ForbiddenException("الترويج للإعلان متاح للإدارة بس.");

        var result = await _service.SetPromotionAsync(id, request, HttpContext.RequestAborted);
        return Ok(ApiResponse<HomeApplianceDetailsDto>.Ok(result, UserMessages.Listings.PromotionUpdated));
    }

    [HttpGet("{id:guid}/similar")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeApplianceListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeApplianceListItemDto>>>> GetSimilar(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetSimilarAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeApplianceListItemDto>>.Ok(
            result, UserMessages.Listings.SimilarLoaded));
    }

    [HttpGet("{id:guid}/related")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeApplianceListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeApplianceListItemDto>>>> GetRelated(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetRelatedAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeApplianceListItemDto>>.Ok(
            result, UserMessages.Listings.RelatedLoaded));
    }

    [HttpGet("recently-added")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeApplianceListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeApplianceListItemDto>>>> GetRecentlyAdded(
        [FromQuery] int count = 8)
    {
        var result = await _service.GetRecentlyAddedAsync(count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeApplianceListItemDto>>.Ok(
            result, UserMessages.Listings.RecentlyAddedLoaded));
    }

    [HttpGet("price-statistics")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<HomeFurnishingPriceStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<HomeFurnishingPriceStatisticsDto>>> GetPriceStatistics(
        [FromQuery] HomeApplianceFilterParams filter)
    {
        var result = await _service.GetPriceStatisticsAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<HomeFurnishingPriceStatisticsDto>.Ok(
            result, UserMessages.Listings.PriceStatisticsLoaded));
    }

    [HttpGet("search-suggestions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeFurnishingSuggestionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeFurnishingSuggestionDto>>>> GetSuggestions(
        [FromQuery] string? term, [FromQuery] int count = 10)
    {
        var result = await _service.GetSuggestionsAsync(term, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeFurnishingSuggestionDto>>.Ok(
            result, UserMessages.Listings.SearchSuggestionsLoaded));
    }

    [HttpGet("device-types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>>> GetDeviceTypes()
    {
        var result = await _service.GetDeviceTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("brands")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>>> GetBrands()
    {
        var result = await _service.GetBrandsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("conditions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>>> GetConditions()
    {
        var result = await _service.GetConditionsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("warranties")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>>> GetWarranties()
    {
        var result = await _service.GetWarrantiesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("colors")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>>> GetColors()
    {
        var result = await _service.GetColorsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
