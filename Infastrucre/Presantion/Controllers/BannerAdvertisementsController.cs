using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs.BannerBookings;
using Shared.Enums;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/banner-advertisements")]
[AllowAnonymous]
[Produces("application/json")]
public class BannerAdvertisementsController : ControllerBase
{
    private readonly IBannerBookingService _bookingService;

    public BannerAdvertisementsController(IBannerBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("home-slider-1")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PublishedBannerDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublishedBannerDto>>>> GetHomeSlider1()
    {
        var result = await _bookingService.GetPublishedSliderAsync(
            BannerLocation.HomeSlider1, HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<PublishedBannerDto>>.Ok(result, "تم استرجاع بانرات السلايدر الأول."));
    }

    [HttpGet("home-slider-2")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PublishedBannerDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublishedBannerDto>>>> GetHomeSlider2()
    {
        var result = await _bookingService.GetPublishedSliderAsync(
            BannerLocation.HomeSlider2, HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<PublishedBannerDto>>.Ok(result, "تم استرجاع بانرات السلايدر الثاني."));
    }

    [HttpGet("sub-category")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PublishedBannerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublishedBannerDto>>>> GetSubCategory(
        [FromQuery] int categoryId, [FromQuery] int subCategoryId)
    {
        var result = await _bookingService.GetPublishedSubCategoryAsync(
            categoryId, subCategoryId, HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<PublishedBannerDto>>.Ok(result, "تم استرجاع بانرات القسم الفرعي."));
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PublishedBannerDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublishedBannerDto>>>> GetActive(
        [FromQuery] BannerLocation? location,
        [FromQuery] int? categoryId,
        [FromQuery] int? subCategoryId)
    {
        var result = await _bookingService.GetPublishedAsync(
            location, categoryId, subCategoryId, HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<PublishedBannerDto>>.Ok(result, "تم استرجاع البانرات المنشورة."));
    }
}
