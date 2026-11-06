using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.AdPayments;
using Shared.DTOs.Advertisements;
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
    [EndpointSummary("Public ad payment configuration (anonymous, kept for backward compatibility)")]
    [EndpointDescription(
        "Same payload as GET /api/v2/ad-payment/config but without authentication. " +
        "Always served with Cache-Control: no-store, so it reflects the latest admin settings.")]
    [ProducesResponseType(typeof(ApiResponse<PublicAdPaymentConfigDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PublicAdPaymentConfigDto>>> GetConfig(
        [FromQuery] AdPaymentConfigQuery query)
    {
        var result = await _payments.GetPublicConfigAsync(HttpContext.RequestAborted, query);

        Response.Headers.CacheControl = "no-store";

        return Ok(ApiResponse<PublicAdPaymentConfigDto>.Ok(result, UserMessages.AdPayments.ConfigLoaded));
    }
}

[ApiController]
[Route("api/v2/ad-payment")]
[Authorize]
[Tags("Ad Payments")]
[Produces("application/json")]
public class AdPaymentConfigController : ControllerBase
{
    private readonly IAdPaymentService _payments;

    public AdPaymentConfigController(IAdPaymentService payments)
    {
        _payments = payments;
    }

    [HttpGet("config")]
    [EndpointSummary("Ad payment configuration for the create-ad form")]
    [EndpointDescription(
        "Authenticated. Returns the live admin settings: isEnabled, price, currency, instructions and the active " +
        "payment methods offered for ad payments (paymentMethods is empty while isEnabled is false). " +
        "Call it every time a create-ad form opens; the response is never cached (Cache-Control: no-store). " +
        "When isEnabled is true the form shows its Payment section; when false the section is hidden and nothing changes. " +
        "Pass type (ListingModuleType number or name), categoryId and/or subCategoryId to get the EFFECTIVE configuration " +
        "for that selection (category, sub-category and module overrides, schedules and the always-free charity category). " +
        "Without a selection the response is the global default and anyCategoryRequiresPayment tells whether some category overrides it.")]
    [ProducesResponseType(typeof(ApiResponse<PublicAdPaymentConfigDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<PublicAdPaymentConfigDto>>> GetConfig(
        [FromQuery] AdPaymentConfigQuery query)
    {
        var result = await _payments.GetPublicConfigAsync(HttpContext.RequestAborted, query);

        Response.Headers.CacheControl = "no-store";

        return Ok(ApiResponse<PublicAdPaymentConfigDto>.Ok(result, UserMessages.AdPayments.ConfigLoaded));
    }

    [HttpGet("modules")]
    [EndpointSummary("Which advertisement types are subject to ad payment")]
    [EndpointDescription(
        "Authenticated. One row per advertisement type (ListingModuleType): its numeric id, key, Arabic name, " +
        "category/sub-category and isSubjectToPayment. Use advertisementTypeKey or advertisementType as {type} " +
        "in the payment routes.")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdPaymentModuleDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<ApiResponse<IReadOnlyList<AdPaymentModuleDto>>> GetModules() =>
        Ok(ApiResponse<IReadOnlyList<AdPaymentModuleDto>>.Ok(_payments.GetModules(), UserMessages.AdPayments.ModulesLoaded));
}

[ApiController]
[Route("api/v2/ads/{type}/{id:guid}/payment")]
[Authorize]
[Tags("Ad Payments")]
[Produces("application/json")]
public class AdPaymentsController : ControllerBase
{
    private const string SubmitRules =
        "multipart/form-data. Fields: PaymentMethodId (int, required, must be one of the offered active methods), " +
        "Screenshot (image file, required, validated by content, max size as in create-ad-form upload limits), " +
        "TransactionReference (string, optional, max 100), Notes (string, optional, max 500), " +
        "ExpectedAmount (decimal, optional) and ExpectedCurrency (string, optional): send the price/currency the user " +
        "was shown; if the admin changed them in the meantime the request is refused with 409 instead of charging a " +
        "different amount. The charged amount and currency always come from the server settings and are frozen on the " +
        "submission; any Amount/Currency field in the request is ignored. " +
        "Only the advertisement owner may call it (anyone else gets 404). ";

    private readonly IAdPaymentService _payments;

    public AdPaymentsController(IAdPaymentService payments)
    {
        _payments = payments;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [EndpointSummary("Submit the payment proof for an advertisement")]
    [EndpointDescription(
        SubmitRules +
        "Allowed while payments are enabled, the advertisement still needs a publication window, and it has no " +
        "pending submission and no approved unused payment. Creates a Pending payment that enters the admin " +
        "review queue. Advertisement moderation is not changed. Also accepts a new proof after a rejection " +
        "(same as /resubmit).")]
    [ProducesResponseType(typeof(ApiResponse<AdPaymentSubmissionResultDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status413PayloadTooLarge)]
    public async Task<ActionResult<ApiResponse<AdPaymentSubmissionResultDto>>> Submit(
        ListingModuleType type, Guid id, [FromForm] SubmitAdPaymentRequest request)
    {
        var screenshot = await ReadScreenshotAsync(request);

        var result = await _payments.SubmitAsync(
            type, id, CurrentUserId, request, screenshot, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<AdPaymentSubmissionResultDto>.Ok(result, UserMessages.AdPayments.Submitted));
    }

    [HttpPost("resubmit")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [EndpointSummary("Resubmit a corrected payment proof after a rejection")]
    [EndpointDescription(
        SubmitRules +
        "Allowed only when the latest payment for this advertisement is Rejected (canResubmit = true in the status). " +
        "400 when there is no payment yet, 409 when the latest payment is Pending or Approved. The new submission " +
        "uses the current price and creates a fresh Pending payment; the rejected one stays in history.")]
    [ProducesResponseType(typeof(ApiResponse<AdPaymentSubmissionResultDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status413PayloadTooLarge)]
    public async Task<ActionResult<ApiResponse<AdPaymentSubmissionResultDto>>> Resubmit(
        ListingModuleType type, Guid id, [FromForm] SubmitAdPaymentRequest request)
    {
        var screenshot = await ReadScreenshotAsync(request);

        var result = await _payments.ResubmitAsync(
            type, id, CurrentUserId, request, screenshot, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<AdPaymentSubmissionResultDto>.Ok(result, UserMessages.AdPayments.Resubmitted));
    }

    [HttpGet]
    [EndpointSummary("Payment status of an advertisement (owner only)")]
    [EndpointDescription(
        "Returns the authoritative payment stage (NotRequired, AwaitingPayment, PaymentUnderReview, " +
        "PaymentRejected, Paid), the latest submission (id, amount, currency, method, status, submittedAt, " +
        "reviewedAt, rejectionReason), canSubmit / canResubmit, and the advertisement's moderation status. " +
        "Only the advertisement owner may call it (anyone else gets 404). Never cached.")]
    [ProducesResponseType(typeof(ApiResponse<AdPaymentStatusDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdPaymentStatusDto>>> GetStatus(ListingModuleType type, Guid id)
    {
        var result = await _payments.GetStatusAsync(type, id, CurrentUserId, HttpContext.RequestAborted);

        Response.Headers.CacheControl = "no-store";

        return Ok(ApiResponse<AdPaymentStatusDto>.Ok(result, UserMessages.AdPayments.StatusLoaded));
    }

    private async Task<UploadImageModel?> ReadScreenshotAsync(SubmitAdPaymentRequest request) =>
        await request.Screenshot.ToUploadModelAsync(HttpContext.RequestAborted)
        ?? await Request.FormFilesOrEmpty().FirstOrDefault().ToUploadModelAsync(HttpContext.RequestAborted);
}
