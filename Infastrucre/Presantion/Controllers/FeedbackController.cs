using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Listings;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/feedback")]
[Authorize]
[Produces("application/json")]
public class FeedbackController : ControllerBase
{
    private readonly IListingInteractionService _interactions;

    public FeedbackController(IListingInteractionService interactions)
    {
        _interactions = interactions;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingRatingDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingRatingDto>>>> GetMine(
        [FromQuery] ListingRatingFilterParams filter)
    {
        filter.ReviewerUserId = CurrentUserId;

        var result = await _interactions.GetRatingsAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingRatingDto>>.Ok(result, "تم استرجاع تقييماتك."));
    }

    [HttpGet("metadata")]
    [AllowAnonymous]
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
