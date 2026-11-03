using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Authorization;
using Shared.Constants;
using Shared.DTOs.AdPayments;
using Shared.Enums;
using Shared.Responses;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/ad-payment")]
[Tags("Admin Ad Payments")]
[AdminPage(AdminPageCatalog.Keys.Payments)]
public class AdminAdPaymentsController : AdminApiController
{
    private readonly IAdPaymentService _payments;

    public AdminAdPaymentsController(IAdPaymentService payments)
    {
        _payments = payments;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("settings")]
    [ProducesResponseType(typeof(ApiResponse<AdminAdPaymentSettingsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminAdPaymentSettingsDto>>> GetSettings()
    {
        var result = await _payments.GetSettingsAsync(RequestAborted);

        return Ok(ApiResponse<AdminAdPaymentSettingsDto>.Ok(result, "تم استرجاع إعدادات الدفع للإعلانات."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("settings")]
    [ProducesResponseType(typeof(ApiResponse<AdminAdPaymentSettingsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AdminAdPaymentSettingsDto>>> UpdateSettings(
        [FromBody] UpdateAdPaymentSettingsRequest request)
    {
        var result = await _payments.UpdateSettingsAsync(request, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<AdminAdPaymentSettingsDto>.Ok(result, "تم حفظ إعدادات الدفع للإعلانات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("submissions")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminAdPaymentSubmissionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminAdPaymentSubmissionDto>>>> GetSubmissions(
        [FromQuery] AdminAdPaymentFilterParams filter)
    {
        filter.Status ??= PaymentStatus.Pending;

        var result = await _payments.GetSubmissionsAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminAdPaymentSubmissionDto>>.Ok(
            result, "تم استرجاع طلبات الدفع للإعلانات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("submissions/{paymentSubmissionId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdminAdPaymentSubmissionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminAdPaymentSubmissionDto>>> GetSubmission(Guid paymentSubmissionId)
    {
        var result = await _payments.GetSubmissionAsync(paymentSubmissionId, RequestAborted);

        return Ok(ApiResponse<AdminAdPaymentSubmissionDto>.Ok(result, "تم استرجاع تفاصيل طلب الدفع."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("submissions/{paymentSubmissionId:guid}/screenshot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScreenshot(Guid paymentSubmissionId)
    {
        var file = await _payments.OpenScreenshotAsync(paymentSubmissionId, RequestAborted);

        Response.Headers.CacheControl = "private, no-store";
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        Response.Headers["Content-Security-Policy"] = "default-src 'none'; sandbox";

        return File(file.Content, file.ContentType);
    }

    [RequireAdminPermission(AdminPermission.Approve)]
    [HttpPatch("submissions/{paymentSubmissionId:guid}/review")]
    [ProducesResponseType(typeof(ApiResponse<AdPaymentReviewResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdPaymentReviewResultDto>>> Review(
        Guid paymentSubmissionId, [FromBody] ReviewAdPaymentRequest request)
    {
        var result = await _payments.ReviewAsync(paymentSubmissionId, request, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<AdPaymentReviewResultDto>.Ok(result, "تمت مراجعة الدفع."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("history")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminAdPaymentSubmissionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminAdPaymentSubmissionDto>>>> GetHistory(
        [FromQuery] AdminAdPaymentFilterParams filter)
    {
        var result = await _payments.GetSubmissionsAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminAdPaymentSubmissionDto>>.Ok(
            result, "تم استرجاع سجل الدفع للإعلانات."));
    }
}
