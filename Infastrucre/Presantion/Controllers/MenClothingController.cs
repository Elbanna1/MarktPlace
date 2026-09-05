using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Clothing;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/men-clothing")]
[Authorize]
[Produces("application/json")]
public class MenClothingController : ControllerBase
{
    private readonly IMenClothingService _service;

    public MenClothingController(IMenClothingService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxClothingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<MenClothingDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<MenClothingDetailsDto>>> Create(
        [FromForm] CreateMenClothingRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(CurrentUserId, request, images, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<MenClothingDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<MenClothingListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<MenClothingListItemDto>>>> GetList(
        [FromQuery] MenClothingFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<MenClothingListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<MenClothingDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<MenClothingDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<MenClothingDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxClothingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<MenClothingDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<MenClothingDetailsDto>>> Update(
        Guid id, [FromForm] UpdateMenClothingRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, video, cancellationToken);

        return Ok(ApiResponse<MenClothingDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("clothing-types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ClothingLookupItemDto>>>> GetClothingTypes()
    {
        var result = await _service.GetClothingTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("brands")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ClothingLookupItemDto>>>> GetBrands()
    {
        var result = await _service.GetBrandsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("sizes")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ClothingLookupItemDto>>>> GetSizes()
    {
        var result = await _service.GetSizesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("colors")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ClothingLookupItemDto>>>> GetColors()
    {
        var result = await _service.GetColorsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("conditions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ClothingLookupItemDto>>>> GetConditions()
    {
        var result = await _service.GetConditionsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("selling-methods")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ClothingLookupItemDto>>>> GetSellingMethods()
    {
        var result = await _service.GetSellingMethodsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ClothingLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
