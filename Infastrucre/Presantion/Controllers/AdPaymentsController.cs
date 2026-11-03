using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.AdPayments;
using Shared.Enums;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v2/public/ad-payment")]
[AllowAnonymous]
[Tags("Ad Payments")]
[Produces("application/json")]
public class PublicAdPaymentController : ControllerBase
{
    private readonly IAdPaymentService _payments;

    public PublicAdPaymentController(IAdPaymentService payments)
    {
        _payments = payments;
    }

    [HttpGet("config")]
    [ProducesResponseType(typeof(ApiResponse<PublicAdPaymentConfigDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PublicAdPaymentConfigDto>>> GetConfig()
    {
        var result = await _payments.GetPublicConfigAsync(HttpContext.RequestAborted);

        Response.Headers.CacheControl = "no-store";

        return Ok(ApiResponse<PublicAdPaymentConfigDto>.Ok(result, UserMessages.AdPayments.ConfigLoaded));
    }
}

[ApiController]
[Route("api/v2/ads/{type}/{id:guid}/payment")]
[Authorize]
[Tags("Ad Payments")]
[Produces("application/json")]
public class AdPaymentsController : ControllerBase
{
    private readonly IAdPaymentService _payments;

    public AdPaymentsController(IAdPaymentService payments)
    {
        _payments = payments;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<AdPaymentSubmissionResultDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdPaymentSubmissionResultDto>>> Submit(
        ListingModuleType type, Guid id, [FromForm] SubmitAdPaymentRequest request)
    {
        var screenshot = await request.Screenshot.ToUploadModelAsync(HttpContext.RequestAborted)
            ?? await Request.FormFilesOrEmpty().FirstOrDefault().ToUploadModelAsync(HttpContext.RequestAborted);

        var result = await _payments.SubmitAsync(
            type, id, CurrentUserId, request, screenshot, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<AdPaymentSubmissionResultDto>.Ok(result, UserMessages.AdPayments.Submitted));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<AdPaymentStatusDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdPaymentStatusDto>>> GetStatus(ListingModuleType type, Guid id)
    {
        var result = await _payments.GetStatusAsync(type, id, CurrentUserId, HttpContext.RequestAborted);

        Response.Headers.CacheControl = "no-store";

        return Ok(ApiResponse<AdPaymentStatusDto>.Ok(result, UserMessages.AdPayments.StatusLoaded));
    }
}
