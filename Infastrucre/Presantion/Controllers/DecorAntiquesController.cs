using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Antiques;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/decor-antiques")]
[Authorize]
[Produces("application/json")]
public class DecorAntiquesController : ControllerBase
{
    private readonly IDecorAntiqueService _service;

    public DecorAntiquesController(IDecorAntiqueService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxAntiqueRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<DecorAntiqueDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<DecorAntiqueDetailsDto>>> Create(
        [FromForm] CreateDecorAntiqueRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(CurrentUserId, request, images, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<DecorAntiqueDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<DecorAntiqueListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<DecorAntiqueListItemDto>>>> GetList(
        [FromQuery] DecorAntiqueFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<DecorAntiqueListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<DecorAntiqueDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<DecorAntiqueDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<DecorAntiqueDetailsDto>.Ok(
            result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxAntiqueRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<DecorAntiqueDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<DecorAntiqueDetailsDto>>> Update(
        Guid id, [FromForm] UpdateDecorAntiqueRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, video, cancellationToken);

        return Ok(ApiResponse<DecorAntiqueDetailsDto>.Ok(
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

    [HttpGet("item-types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>>> GetItemTypes()
    {
        var result = await _service.GetItemTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("materials")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>>> GetMaterials()
    {
        var result = await _service.GetMaterialsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("conditions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>>> GetConditions()
    {
        var result = await _service.GetConditionsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("originality")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>>> GetOriginalities()
    {
        var result = await _service.GetOriginalitiesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AntiqueLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
