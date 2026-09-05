using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.BannerBookings;
using Shared.Enums;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/banner-bookings")]
[Authorize]
[Produces("application/json")]
public class BannerBookingsController : ControllerBase
{
    private readonly IBannerBookingService _bookingService;

    public BannerBookingsController(IBannerBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpGet("placements")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerPlacementDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerPlacementDto>>>> GetPlacements()
    {
        var result = await _bookingService.GetPlacementsAsync(HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<BannerPlacementDto>>.Ok(result, "تم استرجاع أماكن الإعلانات."));
    }

    [HttpGet("availability")]
    [ProducesResponseType(typeof(ApiResponse<BannerAvailabilityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerAvailabilityDto>>> GetAvailability(
        [FromQuery] BannerAvailabilityQuery query)
    {
        var result = await _bookingService.GetAvailabilityAsync(query, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerAvailabilityDto>.Ok(result, result.Message));
    }

    [HttpGet("payment-methods")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerPaymentMethodDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerPaymentMethodDto>>>> GetPaymentMethods()
    {
        var result = await _bookingService.GetPaymentMethodsAsync(HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<BannerPaymentMethodDto>>.Ok(result, "تم استرجاع طرق الدفع."));
    }

    [HttpGet("quote")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<BannerBookingSummaryDto>>> GetQuote(
        [FromQuery] BannerBookingQuoteQuery query)
    {
        var result = await _bookingService.GetQuoteAsync(query, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerBookingSummaryDto>.Ok(result, "تم حساب تفاصيل الحجز."));
    }

    [HttpPost("preview")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit((2 * ImageConstants.MaxFileSizeBytes) + (1024 * 1024))]
    [ProducesResponseType(typeof(ApiResponse<BannerPreviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<BannerPreviewDto>>> Preview(
        [FromForm] BannerImagePreviewRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var desktop = await request.DesktopImage.ToUploadModelAsync(cancellationToken);
        var mobile = await request.MobileImage.ToUploadModelAsync(cancellationToken);

        var result = await _bookingService.PreviewImagesAsync(
            request.Location, desktop, mobile, cancellationToken);

        return Ok(ApiResponse<BannerPreviewDto>.Ok(result, "تم رفع الصور بنجاح."));
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxBannerBookingRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> Create(
        [FromForm] CreateBannerBookingRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var desktop = await request.DesktopImage.ToUploadModelAsync(cancellationToken);
        var mobile = await request.MobileImage.ToUploadModelAsync(cancellationToken);
        var proof = await request.PaymentProof.ToUploadModelAsync(cancellationToken);

        var result = await _bookingService.CreateAsync(
            CurrentUserId, request, desktop, mobile, proof, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<BannerBookingDto>.Ok(result, "تم إرسال حجز الإعلان وهو الآن قيد المراجعة."));
    }

    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerBookingListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerBookingListItemDto>>>> GetMyBookings(
        [FromQuery] BannerBookingStatus? status)
    {
        var result = await _bookingService.GetMyBookingsAsync(CurrentUserId, status, HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<BannerBookingListItemDto>>.Ok(result, "تم استرجاع حجوزاتك."));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> GetById(Guid id)
    {
        var result = await _bookingService.GetByIdAsync(id, CurrentUserId, IsAdmin, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم استرجاع حجز الإعلان."));
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<BannerBookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerBookingDto>>> Cancel(Guid id)
    {
        var result = await _bookingService.CancelAsync(id, CurrentUserId, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerBookingDto>.Ok(result, "تم إلغاء حجز الإعلان."));
    }

    [HttpGet("published")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PublishedBannerDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublishedBannerDto>>>> GetPublished(
        [FromQuery] BannerLocation? location,
        [FromQuery] int? categoryId,
        [FromQuery] int? subCategoryId)
    {
        var result = await _bookingService.GetPublishedAsync(
            location, categoryId, subCategoryId, HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<PublishedBannerDto>>.Ok(result, "تم استرجاع البانرات المنشورة."));
    }
}
