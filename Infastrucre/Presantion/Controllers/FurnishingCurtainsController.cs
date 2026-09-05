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
[Route(HomeFurnishingRoutes.FurnishingCurtains)]
[Authorize]
[Produces("application/json")]
public class FurnishingCurtainsController : ControllerBase
{
    private readonly IFurnishingCurtainService _service;

    public FurnishingCurtainsController(IFurnishingCurtainService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxHomeFurnishingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<FurnishingCurtainDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<FurnishingCurtainDetailsDto>>> Create(
        [FromForm] CreateFurnishingCurtainRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(CurrentUserId, request, images, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<FurnishingCurtainDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<FurnishingCurtainListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<FurnishingCurtainListItemDto>>>> GetList(
        [FromQuery] FurnishingCurtainFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<FurnishingCurtainListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<FurnishingCurtainDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FurnishingCurtainDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, countView: true, HttpContext.RequestAborted);
        return Ok(ApiResponse<FurnishingCurtainDetailsDto>.Ok(
            result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxHomeFurnishingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<FurnishingCurtainDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FurnishingCurtainDetailsDto>>> Update(
        Guid id, [FromForm] UpdateFurnishingCurtainRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, video, cancellationToken);

        return Ok(ApiResponse<FurnishingCurtainDetailsDto>.Ok(
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
    [ProducesResponseType(typeof(ApiResponse<FurnishingCurtainDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FurnishingCurtainDetailsDto>>> ReorderImages(
        Guid id, [FromBody] ReorderHomeFurnishingImagesRequest request)
    {
        var result = await _service.ReorderImagesAsync(
            CurrentUserId, IsAdmin, id, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<FurnishingCurtainDetailsDto>.Ok(result, UserMessages.Listings.ImagesReordered));
    }

    [HttpPut("{id:guid}/promotion")]
    [ProducesResponseType(typeof(ApiResponse<FurnishingCurtainDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FurnishingCurtainDetailsDto>>> SetPromotion(
        Guid id, [FromBody] PromoteHomeFurnishingRequest request)
    {
        if (!IsAdmin)
            throw new ForbiddenException("الترويج للإعلان متاح للإدارة بس.");

        var result = await _service.SetPromotionAsync(id, request, HttpContext.RequestAborted);
        return Ok(ApiResponse<FurnishingCurtainDetailsDto>.Ok(result, UserMessages.Listings.PromotionUpdated));
    }

    [HttpGet("{id:guid}/similar")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FurnishingCurtainListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FurnishingCurtainListItemDto>>>> GetSimilar(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetSimilarAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<FurnishingCurtainListItemDto>>.Ok(
            result, UserMessages.Listings.SimilarLoaded));
    }

    [HttpGet("{id:guid}/related")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FurnishingCurtainListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FurnishingCurtainListItemDto>>>> GetRelated(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetRelatedAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<FurnishingCurtainListItemDto>>.Ok(
            result, UserMessages.Listings.RelatedLoaded));
    }

    [HttpGet("recently-added")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FurnishingCurtainListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FurnishingCurtainListItemDto>>>> GetRecentlyAdded(
        [FromQuery] int count = 8)
    {
        var result = await _service.GetRecentlyAddedAsync(count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<FurnishingCurtainListItemDto>>.Ok(
            result, UserMessages.Listings.RecentlyAddedLoaded));
    }

    [HttpGet("price-statistics")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<HomeFurnishingPriceStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<HomeFurnishingPriceStatisticsDto>>> GetPriceStatistics(
        [FromQuery] FurnishingCurtainFilterParams filter)
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

    [HttpGet("product-types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>>> GetProductTypes()
    {
        var result = await _service.GetProductTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("sizes")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>>> GetSizes()
    {
        var result = await _service.GetSizesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("materials")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HomeFurnishingLookupItemDto>>>> GetMaterials()
    {
        var result = await _service.GetMaterialsAsync(HttpContext.RequestAborted);
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
