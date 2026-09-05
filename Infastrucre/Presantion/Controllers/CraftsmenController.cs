using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Craftsmen;
using Shared.DTOs.Lookups;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/craftsmen")]
[Authorize]
[Produces("application/json")]
public class CraftsmenController : ControllerBase
{
    private readonly ICraftsmanService _craftsmanService;
    private readonly ILookupService _lookupService;

    public CraftsmenController(ICraftsmanService craftsmanService, ILookupService lookupService)
    {
        _craftsmanService = craftsmanService;
        _lookupService = lookupService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    public async Task<ActionResult<ApiResponse<CraftsmanDetailsDto>>> Create(
        [FromForm] CreateCraftsmanRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _craftsmanService.CreateAsync(CurrentUserId, request, images);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<CraftsmanDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PaginatedResult<CraftsmanListItemDto>>>> GetList(
        [FromQuery] CraftsmanFilterParams filter)
    {
        var result = await _craftsmanService.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<CraftsmanListItemDto>>.Ok(result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CraftsmanDetailsDto>>> GetById(Guid id)
    {
        var result = await _craftsmanService.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<CraftsmanDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    public async Task<ActionResult<ApiResponse<CraftsmanDetailsDto>>> Update(
        Guid id, [FromForm] UpdateCraftsmanRequest request)
    {
        var newImages = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _craftsmanService.UpdateAsync(CurrentUserId, IsAdmin, id, request, newImages);
        return Ok(ApiResponse<CraftsmanDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _craftsmanService.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("specializations")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SpecializationDto>>>> GetSpecializations()
    {
        var result = await _lookupService.GetSpecializationsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<SpecializationDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("experience-levels")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ExperienceLevelDto>>>> GetExperienceLevels()
    {
        var result = await _lookupService.GetExperienceLevelsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ExperienceLevelDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }
}
