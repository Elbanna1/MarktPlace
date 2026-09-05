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
[Route(OnlineShoppingRoutes.Accessories)]
[Authorize]
[Produces("application/json")]
public class AccessoriesController : ControllerBase
{
    private readonly IAccessoryService _service;

    public AccessoriesController(IAccessoryService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxOnlineShoppingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<AccessoryDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<AccessoryDetailsDto>>> Create(
        [FromForm] CreateAccessoryRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var logo = await request.Logo.ToUploadModelAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(
            CurrentUserId, request, images, logo, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<AccessoryDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AccessoryListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AccessoryListItemDto>>>> GetList(
        [FromQuery] AccessoryFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<AccessoryListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AccessoryDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AccessoryDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<AccessoryDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxOnlineShoppingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<AccessoryDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AccessoryDetailsDto>>> Update(
        Guid id, [FromForm] UpdateAccessoryRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var logo = await request.Logo.ToUploadModelAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, logo, video, cancellationToken);

        return Ok(ApiResponse<AccessoryDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetTypes()
    {
        var result = await _service.GetAccessoryTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("categories")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetCategories()
    {
        var result = await _service.GetCategoriesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("materials")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetMaterials()
    {
        var result = await _service.GetMaterialsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("colors")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>>> GetColors()
    {
        var result = await _service.GetColorsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<OnlineShoppingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
