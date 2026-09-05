using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Farms;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/farms")]
[Authorize]
[Produces("application/json")]
public class FarmsController : ControllerBase
{
    private readonly IFarmService _farmService;

    public FarmsController(IFarmService farmService)
    {
        _farmService = farmService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<FarmDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<FarmDetailsDto>>> Create([FromForm] CreateFarmRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _farmService.CreateAsync(CurrentUserId, request, images);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<FarmDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<FarmListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<FarmListItemDto>>>> GetList(
        [FromQuery] FarmFilterParams filter)
    {
        var result = await _farmService.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<FarmListItemDto>>.Ok(result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<FarmDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FarmDetailsDto>>> GetById(Guid id)
    {
        var result = await _farmService.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<FarmDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<FarmDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FarmDetailsDto>>> Update(
        Guid id, [FromForm] UpdateFarmRequest request)
    {
        var newImages = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _farmService.UpdateAsync(CurrentUserId, IsAdmin, id, request, newImages);
        return Ok(ApiResponse<FarmDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _farmService.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FarmTypeOptionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FarmTypeOptionDto>>>> GetFarmTypes()
    {
        var result = await _farmService.GetFarmTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<FarmTypeOptionDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("farming-methods")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FarmingMethodOptionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FarmingMethodOptionDto>>>> GetFarmingMethods()
    {
        var result = await _farmService.GetFarmingMethodsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<FarmingMethodOptionDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("availability-seasons")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AvailabilitySeasonOptionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AvailabilitySeasonOptionDto>>>> GetAvailabilitySeasons()
    {
        var result = await _farmService.GetAvailabilitySeasonsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<AvailabilitySeasonOptionDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
