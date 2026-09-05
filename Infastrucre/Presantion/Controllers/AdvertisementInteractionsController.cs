using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Listings;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/advertisements")]
[Authorize]
[Produces("application/json")]
public class AdvertisementInteractionsController : ControllerBase
{
    private readonly IListingInteractionService _interactions;
    private readonly IAdvertisementService _advertisements;
    private readonly IListingRepublishService _republish;

    public AdvertisementInteractionsController(
        IListingInteractionService interactions,
        IAdvertisementService advertisements,
        IListingRepublishService republish)
    {
        _interactions = interactions;
        _advertisements = advertisements;
        _republish = republish;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private string? CurrentUserIdOrNull => User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpPost("{id:guid}/view")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ListingViewResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingViewResultDto>>> RecordView(
        Guid id, [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        var result = await _interactions.RecordViewAsync(
            type, id, CurrentUserIdOrNull,
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingViewResultDto>.Ok(result, "تم تسجيل المشاهدة."));
    }

    [HttpGet("recently-viewed")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingCardDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingCardDto>>>> GetRecentlyViewed(
        [FromQuery] ListingCardFilterParams filter)
    {
        var result = await _interactions.GetRecentlyViewedAsync(
            CurrentUserId, filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingCardDto>>.Ok(
            result, "تم استرجاع الإعلانات التي شوهدت مؤخرًا."));
    }

    [HttpDelete("recently-viewed")]
    [ProducesResponseType(typeof(ApiResponse<ListingHistoryClearedDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingHistoryClearedDto>>> ClearRecentlyViewed(
        [FromQuery] ListingModuleType? type = null)
    {
        var result = await _interactions.ClearRecentlyViewedAsync(
            CurrentUserId, type, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingHistoryClearedDto>.Ok(result, "تم مسح سجل المشاهدات."));
    }

    [HttpDelete("{id:guid}/recently-viewed")]
    [ProducesResponseType(typeof(ApiResponse<ListingHistoryClearedDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingHistoryClearedDto>>> RemoveRecentlyViewed(
        Guid id, [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        var result = await _interactions.RemoveRecentlyViewedAsync(
            CurrentUserId, type, id, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingHistoryClearedDto>.Ok(result, "تم حذف الإعلان من سجل المشاهدات."));
    }

    [HttpPost("{id:guid}/favorite")]
    [ProducesResponseType(typeof(ApiResponse<ListingFavoriteResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingFavoriteResultDto>>> AddFavorite(
        Guid id, [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        var result = await _interactions.AddFavoriteAsync(
            CurrentUserId, type, id, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingFavoriteResultDto>.Ok(result, "تمت الإضافة إلى المفضلة."));
    }

    [HttpDelete("{id:guid}/favorite")]
    [ProducesResponseType(typeof(ApiResponse<ListingFavoriteResultDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingFavoriteResultDto>>> RemoveFavorite(
        Guid id, [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        var result = await _interactions.RemoveFavoriteAsync(
            CurrentUserId, type, id, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingFavoriteResultDto>.Ok(result, "تمت الإزالة من المفضلة."));
    }

    [HttpGet("favorites")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingCardDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingCardDto>>>> GetFavorites(
        [FromQuery] ListingCardFilterParams filter)
    {
        var result = await _interactions.GetFavoritesAsync(
            CurrentUserId, filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingCardDto>>.Ok(result, "تم استرجاع المفضلة."));
    }

    [HttpGet("{id:guid}/similar")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingCardDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingCardDto>>>> GetSimilar(
        Guid id, [FromQuery] SimilarListingFilterParams filter)
    {
        var result = await _interactions.GetSimilarAsync(
            filter.Type, id, filter, CurrentUserIdOrNull, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingCardDto>>.Ok(
            result, "تم استرجاع الإعلانات المشابهة."));
    }

    [HttpGet("{id:guid}/actions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ListingActionsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingActionsDto>>> GetActions(
        Guid id, [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        var result = await _interactions.GetActionsAsync(
            type, id, CurrentUserIdOrNull, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingActionsDto>.Ok(result, "تم استرجاع إجراءات الإعلان."));
    }

    [HttpGet("metadata")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ListingInteractionMetadataDto>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<ListingInteractionMetadataDto>> GetMetadata() =>
        Ok(ApiResponse<ListingInteractionMetadataDto>.Ok(
            _interactions.GetMetadata(), "تم استرجاع بيانات التفاعلات."));

    [HttpPost("{id:guid}/report")]
    [ProducesResponseType(typeof(ApiResponse<ListingReportDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingReportDto>>> Report(
        Guid id, [FromBody] CreateListingReportRequest request)
    {
        var result = await _interactions.ReportAsync(
            CurrentUserId, request.Type, id, request, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<ListingReportDto>.Ok(result, "تم استلام البلاغ وسيتم مراجعته."));
    }

    [HttpPost("{id:guid}/rating")]
    [ProducesResponseType(typeof(ApiResponse<ListingRatingResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingRatingResultDto>>> Rate(
        Guid id,
        [FromBody] RateListingRequest request,
        [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        var result = await _interactions.RateAsync(
            CurrentUserId, type, id, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingRatingResultDto>.Ok(
            result, result.Created ? "تم إرسال تقييمك." : "تم تحديث تقييمك."));
    }

    [HttpDelete("{id:guid}/rating")]
    [ProducesResponseType(typeof(ApiResponse<ListingRatingSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingRatingSummaryDto>>> RemoveRating(
        Guid id, [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        var result = await _interactions.RemoveRatingAsync(
            CurrentUserId, type, id, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingRatingSummaryDto>.Ok(result, "تم حذف تقييمك."));
    }

    [HttpGet("{id:guid}/rating")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ListingRatingSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingRatingSummaryDto>>> GetRatingSummary(
        Guid id, [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        var result = await _interactions.GetRatingSummaryAsync(
            type, id, CurrentUserIdOrNull, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingRatingSummaryDto>.Ok(result, "تم استرجاع تقييم الإعلان."));
    }

    [HttpGet("ratings/my")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingRatingDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingRatingDto>>>> GetMyRatings(
        [FromQuery] ListingRatingFilterParams filter)
    {
        filter.ReviewerUserId = CurrentUserId;

        var result = await _interactions.GetRatingsAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingRatingDto>>.Ok(result, "تم استرجاع تقييماتك."));
    }

    [HttpGet("{id:guid}/ratings")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingRatingDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingRatingDto>>>> GetListingRatings(
        Guid id,
        [FromQuery] ListingRatingFilterParams filter,
        [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        filter.ListingType = type;
        filter.ListingId = id;
        filter.ReviewerUserId = null;

        var result = await _interactions.GetRatingsAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingRatingDto>>.Ok(result, "تم استرجاع تقييمات الإعلان."));
    }

    [HttpPost("{id:guid}/republish")]
    [ProducesResponseType(typeof(ApiResponse<ListingRepublishResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingRepublishResultDto>>> Republish(
        Guid id,
        [FromQuery] ListingModuleType type = ListingModuleType.Advertisement)
    {
        var result = await _republish.RepublishAsync(
            CurrentUserId, type, id, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingRepublishResultDto>.Ok(result, result.Message));
    }
}
