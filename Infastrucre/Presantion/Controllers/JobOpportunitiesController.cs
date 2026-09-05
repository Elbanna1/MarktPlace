using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.JobOpportunities;
using Shared.DTOs.JobRequests;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/job-opportunities")]
[Authorize]
[Produces("application/json")]
public class JobOpportunitiesController : ControllerBase
{
    private readonly IJobOpportunityService _jobOpportunityService;

    public JobOpportunitiesController(IJobOpportunityService jobOpportunityService)
    {
        _jobOpportunityService = jobOpportunityService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<JobOpportunityDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<JobOpportunityDetailsDto>>> Create(
        [FromForm] CreateJobOpportunityRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var logo = await request.Logo.ToUploadModelAsync(cancellationToken);

        var result = await _jobOpportunityService.CreateAsync(
            CurrentUserId, request, images, logo, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<JobOpportunityDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<JobOpportunityListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<JobOpportunityListItemDto>>>> GetList(
        [FromQuery] JobOpportunityFilterParams filter)
    {
        var result = await _jobOpportunityService.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<JobOpportunityListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<JobOpportunityDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<JobOpportunityDetailsDto>>> GetById(Guid id)
    {
        var result = await _jobOpportunityService.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<JobOpportunityDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<JobOpportunityDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<JobOpportunityDetailsDto>>> Update(
        Guid id, [FromForm] UpdateJobOpportunityRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var logo = await request.Logo.ToUploadModelAsync(cancellationToken);

        var result = await _jobOpportunityService.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, logo, cancellationToken);

        return Ok(ApiResponse<JobOpportunityDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _jobOpportunityService.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("job-fields")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<JobFieldDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<JobFieldDto>>>> GetJobFields()
    {
        var result = await _jobOpportunityService.GetJobFieldsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<JobFieldDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("experience-levels")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<JobExperienceLevelDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<JobExperienceLevelDto>>>> GetExperienceLevels()
    {
        var result = await _jobOpportunityService.GetExperienceLevelsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<JobExperienceLevelDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("work-types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<WorkTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WorkTypeDto>>>> GetWorkTypes()
    {
        var result = await _jobOpportunityService.GetWorkTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<WorkTypeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("salary-types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SalaryTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SalaryTypeDto>>>> GetSalaryTypes()
    {
        var result = await _jobOpportunityService.GetSalaryTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<SalaryTypeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }
}
