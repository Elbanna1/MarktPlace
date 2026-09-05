using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.OnlineShopping;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route(OnlineShoppingRoutes.HomemadeFood)]
[Authorize]
[Produces("application/json")]
public class HomemadeFoodController : ControllerBase
{
    private readonly IHomemadeFoodService _service;

    public HomemadeFoodController(IHomemadeFoodService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxOnlineShoppingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<HomemadeFoodDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<HomemadeFoodDetailsDto>>> Create(
        [FromForm] CreateHomemadeFoodRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(CurrentUserId, request, images, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<HomemadeFoodDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<HomemadeFoodListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<HomemadeFoodListItemDto>>>> GetList(
        [FromQuery] HomemadeFoodFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<HomemadeFoodListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<HomemadeFoodDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<HomemadeFoodDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<HomemadeFoodDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxOnlineShoppingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<HomemadeFoodDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<HomemadeFoodDetailsDto>>> Update(
        Guid id, [FromForm] UpdateHomemadeFoodRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, video, cancellationToken);

        return Ok(ApiResponse<HomemadeFoodDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("sections")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetSections()
    {
        var result = await _service.GetSectionsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("delivery-areas")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetDeliveryAreas()
    {
        var result = await _service.GetDeliveryAreasAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
