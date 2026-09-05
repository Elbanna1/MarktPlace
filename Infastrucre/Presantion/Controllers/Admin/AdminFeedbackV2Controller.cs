using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Listings;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/feedback")]
[Tags("Admin Reports")]
[AdminPage(AdminPageCatalog.Keys.Feedback)]
public class AdminFeedbackV2Controller : AdminApiController
{
    private readonly IListingInteractionService _interactions;

    public AdminFeedbackV2Controller(IListingInteractionService interactions)
    {
        _interactions = interactions;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingRatingDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingRatingDto>>>> GetAll(
        [FromQuery] ListingRatingFilterParams filter)
    {
        var result = await _interactions.GetRatingsAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingRatingDto>>.Ok(result, "تم استرجاع التقييمات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ListingRatingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingRatingDto>>> GetById(Guid id)
    {
        var result = await _interactions.GetRatingAsync(id, RequestAborted);

        return Ok(ApiResponse<ListingRatingDto>.Ok(result, "تم استرجاع التقييم."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("listing/{type}/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ListingRatingSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingRatingSummaryDto>>> GetListingSummary(
        Shared.Enums.ListingModuleType type, Guid id)
    {
        var result = await _interactions.GetRatingSummaryAsync(type, id, viewerUserId: null, RequestAborted);

        return Ok(ApiResponse<ListingRatingSummaryDto>.Ok(result, "تم استرجاع تقييم الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _interactions.DeleteRatingAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف التقييم."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("metadata")]
    [ProducesResponseType(typeof(ApiResponse<ListingRatingMetadataDto>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<ListingRatingMetadataDto>> GetMetadata() =>
        Ok(ApiResponse<ListingRatingMetadataDto>.Ok(
            new ListingRatingMetadataDto
            {
                MinRating = ListingInteractionCatalog.MinRating,
                MaxRating = ListingInteractionCatalog.MaxRating,
                Modules = _interactions.GetMetadata().Modules
            },
            "تم استرجاع بيانات التقييمات."));
}
