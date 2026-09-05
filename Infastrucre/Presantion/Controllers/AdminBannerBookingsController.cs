using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.BannerBookings;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/admin/banner-bookings")]
[Authorize(Roles = AppRoles.Admin)]
[Produces("application/json")]
[AdminPage(AdminPageCatalog.Keys.BannerRequests)]
public class AdminBannerBookingsController : ControllerBase
{
    private readonly IBannerBookingService _bookingService;

    public AdminBannerBookingsController(IBannerBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    private string CurrentAdminId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<BannerBookingListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<BannerBookingListItemDto>>>> GetAll(
        [FromQuery] BannerBookingFilterParams filter)
    {
        var result = await _bookingService.GetAllAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<BannerBookingListItemDto>>.Ok(result, "تم استرجاع حجوزات الإعلانات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> GetById(Guid id)
    {
        var result = await _bookingService.GetByIdAsync(
            id, CurrentAdminId, isAdmin: true, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم استرجاع حجز الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("rejection-reasons")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerRejectionReasonDto>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<BannerRejectionReasonDto>>> GetRejectionReasons()
    {
        var result = _bookingService.GetRejectionReasons();

        return Ok(ApiResponse<IReadOnlyList<BannerRejectionReasonDto>>.Ok(result, "تم استرجاع أسباب الرفض."));
    }

    [RequireAdminPermission(AdminPermission.Approve)]
    [HttpPost("{id:guid}/approve-payment")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> ApprovePayment(Guid id)
    {
        var result = await _bookingService.ApprovePaymentAsync(id, CurrentAdminId, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم تأكيد الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Reject)]
    [HttpPost("{id:guid}/reject-payment")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> RejectPayment(
        Guid id, [FromBody] RejectBannerPaymentRequest request)
    {
        var result = await _bookingService.RejectPaymentAsync(
            id, CurrentAdminId, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم رفض إثبات الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Approve)]
    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> Approve(Guid id)
    {
        var result = await _bookingService.ApproveAsync(id, CurrentAdminId, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تمت الموافقة على الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Reject)]
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> Reject(
        Guid id, [FromBody] RejectBannerBookingRequest request)
    {
        var result = await _bookingService.RejectAsync(id, CurrentAdminId, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم رفض حجز الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPost("{id:guid}/expire")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> Expire(Guid id)
    {
        var result = await _bookingService.ExpireAsync(id, CurrentAdminId, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم إنهاء الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _bookingService.DeleteAsync(id, HttpContext.RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف حجز الإعلان."));
    }
}
