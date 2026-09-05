using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Animals;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/horses")]
[Authorize]
[Produces("application/json")]
public class HorsesController : ControllerBase
{
    private readonly IHorseService _service;

    public HorsesController(IHorseService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<HorseDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<HorseDetailsDto>>> Create(
        [FromForm] CreateHorseRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.CreateAsync(CurrentUserId, request, images);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<HorseDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<HorseListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<HorseListItemDto>>>> GetList(
        [FromQuery] HorseFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<HorseListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<HorseDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<HorseDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<HorseDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<HorseDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<HorseDetailsDto>>> Update(
        Guid id, [FromForm] UpdateHorseRequest request)
    {
        var newImages = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.UpdateAsync(CurrentUserId, IsAdmin, id, request, newImages);
        return Ok(ApiResponse<HorseDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("breeds")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AnimalLookupItemDto>>>> GetBreeds()
    {
        var result = await _service.GetBreedsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("purposes")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AnimalLookupItemDto>>>> GetPurposes()
    {
        var result = await _service.GetPurposesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("ages")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AnimalLookupItemDto>>>> GetAges()
    {
        var result = await _service.GetAgesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("genders")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AnimalLookupItemDto>>>> GetGenders()
    {
        var result = await _service.GetGendersAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("health-status")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AnimalLookupItemDto>>>> GetHealthStatuses()
    {
        var result = await _service.GetHealthStatusesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("training-levels")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AnimalLookupItemDto>>>> GetTrainingLevels()
    {
        var result = await _service.GetTrainingLevelsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("vaccinations")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AnimalLookupItemDto>>>> GetVaccinations()
    {
        var result = await _service.GetVaccinationsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AnimalLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
