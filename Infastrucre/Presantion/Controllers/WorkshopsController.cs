using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Lookups;
using Shared.DTOs.Workshops;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/workshops")]
[Authorize]
[Produces("application/json")]
public class WorkshopsController : ControllerBase
{
    private readonly IWorkshopService _workshopService;
    private readonly ILookupService _lookupService;

    public WorkshopsController(IWorkshopService workshopService, ILookupService lookupService)
    {
        _workshopService = workshopService;
        _lookupService = lookupService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    public async Task<ActionResult<ApiResponse<WorkshopDetailsDto>>> Create(
        [FromForm] CreateWorkshopRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _workshopService.CreateAsync(CurrentUserId, request, images);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<WorkshopDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PaginatedResult<WorkshopListItemDto>>>> GetList(
        [FromQuery] WorkshopFilterParams filter)
    {
        var result = await _workshopService.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<WorkshopListItemDto>>.Ok(result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<WorkshopDetailsDto>>> GetById(Guid id)
    {
        var result = await _workshopService.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<WorkshopDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    public async Task<ActionResult<ApiResponse<WorkshopDetailsDto>>> Update(
        Guid id, [FromForm] UpdateWorkshopRequest request)
    {
        var newImages = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _workshopService.UpdateAsync(CurrentUserId, IsAdmin, id, request, newImages);
        return Ok(ApiResponse<WorkshopDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _workshopService.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("types")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WorkshopTypeDto>>>> GetWorkshopTypes()
    {
        var result = await _lookupService.GetWorkshopTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<WorkshopTypeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }
}
