using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Feedback;
using Shared.DTOs.Listings;
using Shared.DTOs.Profile;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
[Produces("application/json")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IListingInteractionService _interactions;

    public ProfileController(IProfileService profileService, IListingInteractionService interactions)
    {
        _profileService = profileService;
        _interactions = interactions;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ProfileDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ProfileDto>>> GetProfile()
    {
        var result = await _profileService.GetProfileAsync(CurrentUserId, HttpContext.RequestAborted);
        return Ok(ApiResponse<ProfileDto>.Ok(result, UserMessages.Profile.Loaded));
    }

    [HttpPut]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfile([FromForm] UpdateProfileRequest request)
    {
        var profileImage = await request.ProfileImage.ToUploadModelAsync(HttpContext.RequestAborted);

        var result = await _profileService.UpdateProfileAsync(
            CurrentUserId, request, profileImage, HttpContext.RequestAborted);

        return Ok(ApiResponse<UserDto>.Ok(result, UserMessages.Profile.Updated));
    }

    [HttpPost("image")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfileImage(
        [FromForm] UpdateProfileImageRequest request)
    {
        var profileImage = await request.ProfileImage.ToUploadModelAsync(HttpContext.RequestAborted);

        var result = await _profileService.UpdateProfileImageAsync(
            CurrentUserId, profileImage, HttpContext.RequestAborted);

        return Ok(ApiResponse<UserDto>.Ok(result, UserMessages.Profile.PictureUpdated));
    }

    [HttpGet("my-listings")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<UserListingDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<UserListingDto>>>> GetMyListings(
        [FromQuery] UserListingFilterParams filter)
    {
        var result = await _profileService.GetMyListingsAsync(CurrentUserId, filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<UserListingDto>>.Ok(result, UserMessages.Listings.MineLoaded));
    }

    [HttpGet("expired-advertisements")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<UserListingDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<UserListingDto>>>> GetExpiredListings(
        [FromQuery] UserListingFilterParams filter)
    {
        var result = await _profileService.GetExpiredListingsAsync(
            CurrentUserId, filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<UserListingDto>>.Ok(
            result, "تم استرجاع الإعلانات المنتهية."));
    }

    [HttpGet("feedback")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingRatingDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingRatingDto>>>> GetMyRatings(
        [FromQuery] ListingRatingFilterParams filter)
    {
        filter.ReviewerUserId = CurrentUserId;

        var result = await _interactions.GetRatingsAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingRatingDto>>.Ok(result, "تم استرجاع تقييماتك."));
    }

    [HttpPost("change-password")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        await _profileService.ChangePasswordAsync(CurrentUserId, request);
        return Ok(ApiResponse.Ok(UserMessages.Auth.PasswordChanged));
    }
}
