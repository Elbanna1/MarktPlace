using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Companies;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/companies")]
[Authorize]
[Produces("application/json")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<CompanyDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<CompanyDetailsDto>>> Create(
        [FromForm] CreateCompanyRequest request)
    {
        var images = await request.Images.ToUploadModelsAsync(HttpContext.RequestAborted);
        var logo = await request.Logo.ToUploadModelAsync(HttpContext.RequestAborted);
        var result = await _companyService.CreateAsync(CurrentUserId, request, images, logo);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<CompanyDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<CompanyListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<CompanyListItemDto>>>> GetList(
        [FromQuery] CompanyFilterParams filter)
    {
        var result = await _companyService.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<CompanyListItemDto>>.Ok(result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<CompanyDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CompanyDetailsDto>>> GetById(Guid id)
    {
        var result = await _companyService.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<CompanyDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<CompanyDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CompanyDetailsDto>>> Update(
        Guid id, [FromForm] UpdateCompanyRequest request)
    {
        var newImages = await request.Images.ToUploadModelsAsync(HttpContext.RequestAborted);
        var logo = await request.Logo.ToUploadModelAsync(HttpContext.RequestAborted);
        var result = await _companyService.UpdateAsync(CurrentUserId, IsAdmin, id, request, newImages, logo);
        return Ok(ApiResponse<CompanyDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _companyService.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("fields")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CompanyFieldOptionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CompanyFieldOptionDto>>>> GetCompanyFields()
    {
        var result = await _companyService.GetCompanyFieldsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<CompanyFieldOptionDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
