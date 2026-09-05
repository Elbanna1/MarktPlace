using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Banners;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/banners")]
[Tags("Admin Banners")]
[AdminPage(AdminPageCatalog.Keys.Banners)]
public class AdminSliderBannersController : AdminApiController
{
    private readonly IBannerService _banners;

    public AdminSliderBannersController(IBannerService banners)
    {
        _banners = banners;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerDto>>>> GetAll(
        [FromQuery] BannerFilterParams filter)
    {
        var result = await _banners.GetAllAsync(filter, RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<BannerDto>>.Ok(result, "تم استرجاع البانرات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BannerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerDto>>> GetById(Guid id)
    {
        var result = await _banners.GetByIdAsync(id, RequestAborted);

        return Ok(ApiResponse<BannerDto>.Ok(result, "تم استرجاع البانر."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxFileSizeBytes + (1 * 1024 * 1024))]
    [ProducesResponseType(typeof(ApiResponse<BannerDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<BannerDto>>> Create([FromForm] CreateBannerRequest request)
    {
        var image = await request.Image.ToUploadModelAsync(RequestAborted);
        var result = await _banners.CreateAsync(request, image, RequestAborted);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<BannerDto>.Ok(result, "تم إنشاء البانر."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxFileSizeBytes + (1 * 1024 * 1024))]
    [ProducesResponseType(typeof(ApiResponse<BannerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerDto>>> Update(
        Guid id, [FromForm] UpdateBannerRequest request)
    {
        var image = await request.Image.ToUploadModelAsync(RequestAborted);
        var result = await _banners.UpdateAsync(id, request, image, RequestAborted);

        return Ok(ApiResponse<BannerDto>.Ok(result, "تم تحديث البانر."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _banners.DeleteAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف البانر."));
    }
}
