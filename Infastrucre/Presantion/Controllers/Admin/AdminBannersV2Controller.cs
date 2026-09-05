using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.BannerBookings;
using Shared.Enums;
using Shared.Responses;
using Shared.Authorization;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/banner-requests")]
[Tags("Admin Banners")]
[AdminPage(AdminPageCatalog.Keys.BannerRequests)]
public class AdminBannerRequestsController : AdminApiController
{
    private readonly IBannerBookingService _bookings;

    public AdminBannerRequestsController(IBannerBookingService bookings)
    {
        _bookings = bookings;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<BannerBookingListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<BannerBookingListItemDto>>>> GetAll(
        [FromQuery] BannerBookingFilterParams filter)
    {
        var result = await _bookings.GetAllAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<BannerBookingListItemDto>>.Ok(
            result, "تم استرجاع طلبات البانر."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PublishedBannerDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublishedBannerDto>>>> GetActive(
        [FromQuery] BannerLocation? location,
        [FromQuery] int? categoryId,
        [FromQuery] int? subCategoryId)
    {
        var result = await _bookings.GetPublishedAsync(
            location, categoryId, subCategoryId, RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<PublishedBannerDto>>.Ok(result, "تم استرجاع البانرات النشطة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> GetById(Guid id)
    {
        var result = await _bookings.GetByIdAsync(id, CurrentAdminId, isAdmin: true, RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم استرجاع طلب البانر."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("availability")]
    [ProducesResponseType(typeof(ApiResponse<BannerAvailabilityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<BannerAvailabilityDto>>> GetAvailability(
        [FromQuery] BannerAvailabilityQuery query)
    {
        var result = await _bookings.GetAvailabilityAsync(query, RequestAborted);

        return Ok(ApiResponse<BannerAvailabilityDto>.Ok(result, "تم استرجاع الأماكن المتاحة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("statuses")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerBookingStatusDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<ApiResponse<IReadOnlyList<BannerBookingStatusDto>>> GetStatuses()
    {
        var result = _bookings.GetStatuses();

        return Ok(ApiResponse<IReadOnlyList<BannerBookingStatusDto>>.Ok(result, "تم استرجاع حالات طلبات البانر."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("rejection-reasons")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerRejectionReasonDto>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<BannerRejectionReasonDto>>> GetRejectionReasons()
    {
        var result = _bookings.GetRejectionReasons();

        return Ok(ApiResponse<IReadOnlyList<BannerRejectionReasonDto>>.Ok(result, "تم استرجاع أسباب الرفض."));
    }

    [RequireAdminPermission(AdminPermission.Approve)]
    [HttpPatch("{id:guid}/approve-payment")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> ApprovePayment(Guid id)
    {
        var result = await _bookings.ApprovePaymentAsync(id, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم تأكيد الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Reject)]
    [HttpPatch("{id:guid}/reject-payment")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> RejectPayment(
        Guid id, [FromBody] RejectBannerPaymentRequest request)
    {
        var result = await _bookings.RejectPaymentAsync(id, CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم رفض إثبات الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Approve)]
    [HttpPatch("{id:guid}/approve")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> Approve(Guid id)
    {
        var result = await _bookings.ApproveAsync(id, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تمت الموافقة على البانر."));
    }

    [RequireAdminPermission(AdminPermission.Reject)]
    [HttpPatch("{id:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> Reject(
        Guid id, [FromBody] RejectBannerBookingRequest request)
    {
        var result = await _bookings.RejectAsync(id, CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم رفض طلب البانر."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPatch("{id:guid}/expire")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> Expire(Guid id)
    {
        var result = await _bookings.ExpireAsync(id, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم إنهاء البانر."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _bookings.DeleteAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف طلب البانر."));
    }
}

[Route(ApiVersions.AdminRoutePrefix + "/banner-prices")]
[Tags("Admin Banners")]
[AdminPage(AdminPageCatalog.Keys.BannerRequests)]
public class AdminBannerPricesController : AdminApiController
{
    private readonly IBannerSettingsService _settings;

    public AdminBannerPricesController(IBannerSettingsService settings)
    {
        _settings = settings;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerPlacementDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerPlacementDto>>>> GetAll()
    {
        var result = await _settings.GetAllAsync(RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<BannerPlacementDto>>.Ok(result, "تم استرجاع أسعار البانرات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{location}")]
    [ProducesResponseType(typeof(ApiResponse<BannerPlacementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerPlacementDto>>> Get(BannerLocation location)
    {
        var result = await _settings.GetAsync(location, RequestAborted);

        return Ok(ApiResponse<BannerPlacementDto>.Ok(result, "تم استرجاع سعر المكان الإعلاني."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPut("{location}")]
    [ProducesResponseType(typeof(ApiResponse<BannerPlacementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerPlacementDto>>> Update(
        BannerLocation location, [FromBody] UpdateBannerPlacementRequest request)
    {
        var result = await _settings.UpdateAsync(location, request, RequestAborted);

        return Ok(ApiResponse<BannerPlacementDto>.Ok(result, "تم تحديث سعر المكان الإعلاني."));
    }
}
