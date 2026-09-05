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
[Route(OnlineShoppingRoutes.ShoppingElectronics)]
[Authorize]
[Produces("application/json")]
public class ShoppingElectronicsController : ControllerBase
{
    private readonly IShoppingElectronicService _service;

    public ShoppingElectronicsController(IShoppingElectronicService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxOnlineShoppingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<ShoppingElectronicDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<ShoppingElectronicDetailsDto>>> Create(
        [FromForm] CreateShoppingElectronicRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(CurrentUserId, request, images, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<ShoppingElectronicDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ShoppingElectronicListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ShoppingElectronicListItemDto>>>> GetList(
        [FromQuery] ShoppingElectronicFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<ShoppingElectronicListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ShoppingElectronicDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ShoppingElectronicDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<ShoppingElectronicDetailsDto>.Ok(
            result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxOnlineShoppingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<ShoppingElectronicDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ShoppingElectronicDetailsDto>>> Update(
        Guid id, [FromForm] UpdateShoppingElectronicRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, video, cancellationToken);

        return Ok(ApiResponse<ShoppingElectronicDetailsDto>.Ok(
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

    [HttpGet("sections")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetSections()
    {
        var result = await _service.GetSectionsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("compatibilities")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetCompatibilities()
    {
        var result = await _service.GetCompatibilitiesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("conditions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetConditions()
    {
        var result = await _service.GetConditionsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("warranties")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetWarranties()
    {
        var result = await _service.GetWarrantiesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
