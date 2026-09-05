using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.DTOs.Advertisements;
using Shared.Responses;

using Shared.Constants;
namespace Presentation.Controllers;

[ApiController]
[Route("api/ads")]
[Authorize]
[Produces("application/json")]
public class AdvertisementsController : ControllerBase
{
    private readonly IAdvertisementService _advertisementService;

    public AdvertisementsController(IAdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(Shared.Constants.FileUploadConstants.MaxAdvertisementRequestBodySizeBytes)]
    public async Task<ActionResult<ApiResponse<AdvertisementDetailsDto>>> Create(
        [FromForm] CreateAdvertisementRequest request)
    {
        var images = await request.Images.ToUploadModelsAsync(HttpContext.RequestAborted);
        var video = await request.Video.ToUploadModelAsync(HttpContext.RequestAborted);
        var result = await _advertisementService.CreateAsync(CurrentUserId, request, images, video);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<AdvertisementDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<AdvertisementDetailsDto>>> Update(
        Guid id, [FromBody] UpdateAdvertisementRequest request)
    {
        var result = await _advertisementService.UpdateAsync(CurrentUserId, id, request);
        return Ok(ApiResponse<AdvertisementDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _advertisementService.DeleteAsync(CurrentUserId, id);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdvertisementListItemDto>>>> GetAll(
        [FromQuery] AdvertisementFilterParams filter)
    {
        var result = await _advertisementService.GetAllAsync(
            filter, User.FindFirstValue(ClaimTypes.NameIdentifier), HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<AdvertisementListItemDto>>.Ok(result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AdvertisementDetailsDto>>> GetById(Guid id)
    {
        var viewerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _advertisementService.GetByIdAsync(
            id, viewerUserId, ip, HttpContext.RequestAborted);
        return Ok(ApiResponse<AdvertisementDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpGet("my/statistics")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AdvertisementStatisticsDto>>> GetMyStatistics()
    {
        var result = await _advertisementService.GetMyStatisticsAsync(
            CurrentUserId, HttpContext.RequestAborted);
        return Ok(ApiResponse<AdvertisementStatisticsDto>.Ok(result, UserMessages.Listings.StatisticsLoaded));
    }

    [HttpGet("my")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdvertisementListItemDto>>>> GetMyAds(
        [FromQuery] AdvertisementFilterParams filter)
    {
        var result = await _advertisementService.GetMyAdsAsync(
            CurrentUserId, filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<AdvertisementListItemDto>>.Ok(result, UserMessages.Listings.MineLoaded));
    }

    [HttpPost("{id:guid}/republish")]
    public async Task<ActionResult<ApiResponse<AdvertisementDetailsDto>>> Republish(
        Guid id,
        [FromBody(EmptyBodyBehavior = Microsoft.AspNetCore.Mvc.ModelBinding.EmptyBodyBehavior.Allow)]
        UpdateAdvertisementRequest? request = null)
    {
        var result = await _advertisementService.RepublishAsync(CurrentUserId, id, request);
        return Ok(ApiResponse<AdvertisementDetailsDto>.Ok(result, UserMessages.Listings.Republished));
    }

    [HttpPost("{id:guid}/images")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(Shared.Constants.ImageConstants.MaxRequestBodySizeBytes)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdvertisementImageDto>>>> AddImages(
        Guid id, [FromForm] UploadImagesRequest request)
    {
        var files = request.Images.Count > 0 ? (IEnumerable<IFormFile>)request.Images : Request.FormFilesOrEmpty();
        var images = await files.ToUploadModelsAsync(HttpContext.RequestAborted);

        if (images.Count == 0)
            return BadRequest(ApiResponse.Fail("لازم ترفع صورة واحدة على الأقل."));

        var result = await _advertisementService.AddImagesAsync(CurrentUserId, id, images);
        return Ok(ApiResponse<IReadOnlyList<AdvertisementImageDto>>.Ok(result, UserMessages.Listings.ImagesUploaded));
    }

    [HttpDelete("image/{imageId:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteImage(Guid imageId)
    {
        await _advertisementService.DeleteImageAsync(CurrentUserId, imageId);
        return Ok(ApiResponse.Ok(UserMessages.Listings.ImageDeleted));
    }
}
