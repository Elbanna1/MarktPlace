using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.JobRequests;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/job-requests")]
[Authorize]
[Produces("application/json")]
public class JobRequestsController : ControllerBase
{
    private readonly IJobRequestService _jobRequestService;

    public JobRequestsController(IJobRequestService jobRequestService)
    {
        _jobRequestService = jobRequestService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxJobRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<JobRequestDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<JobRequestDetailsDto>>> Create(
        [FromForm] CreateJobRequestRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var profileImage = await request.ProfileImage.ToUploadModelAsync(cancellationToken);
        var cvFile = await request.CvFile.ToUploadModelAsync(cancellationToken);
        var introVideo = await request.IntroVideo.ToUploadModelAsync(cancellationToken);

        var result = await _jobRequestService.CreateAsync(
            CurrentUserId, request, profileImage, cvFile, introVideo, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<JobRequestDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<JobRequestListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<JobRequestListItemDto>>>> GetList(
        [FromQuery] JobRequestFilterParams filter)
    {
        var result = await _jobRequestService.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<JobRequestListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<JobRequestDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<JobRequestDetailsDto>>> GetById(Guid id)
    {
        var result = await _jobRequestService.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<JobRequestDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxJobRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<JobRequestDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<JobRequestDetailsDto>>> Update(
        Guid id, [FromForm] UpdateJobRequestRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var profileImage = await request.ProfileImage.ToUploadModelAsync(cancellationToken);
        var cvFile = await request.CvFile.ToUploadModelAsync(cancellationToken);
        var introVideo = await request.IntroVideo.ToUploadModelAsync(cancellationToken);

        var result = await _jobRequestService.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, profileImage, cvFile, introVideo, cancellationToken);

        return Ok(ApiResponse<JobRequestDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _jobRequestService.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("job-fields")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<JobFieldDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<JobFieldDto>>>> GetJobFields()
    {
        var result = await _jobRequestService.GetJobFieldsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<JobFieldDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("experience-levels")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<JobExperienceLevelDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<JobExperienceLevelDto>>>> GetExperienceLevels()
    {
        var result = await _jobRequestService.GetExperienceLevelsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<JobExperienceLevelDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("education-levels")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EducationLevelDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<EducationLevelDto>>>> GetEducationLevels()
    {
        var result = await _jobRequestService.GetEducationLevelsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<EducationLevelDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
