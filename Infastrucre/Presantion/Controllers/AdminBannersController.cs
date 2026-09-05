using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Banners;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/admin/banners")]
[Authorize(Roles = AppRoles.Admin)]
[Produces("application/json")]
[AdminPage(AdminPageCatalog.Keys.Banners)]
public class AdminBannersController : ControllerBase
{
    private readonly IBannerService _bannerService;

    public AdminBannersController(IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerDto>>>> GetAll(
        [FromQuery] BannerFilterParams filter)
    {
        var result = await _bannerService.GetAllAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<BannerDto>>.Ok(result, "تم استرجاع البانرات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BannerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerDto>>> GetById(Guid id)
    {
        var result = await _bannerService.GetByIdAsync(id, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerDto>.Ok(result, "تم استرجاع البانر."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxFileSizeBytes + (1 * 1024 * 1024))]
    [ProducesResponseType(typeof(ApiResponse<BannerDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<BannerDto>>> Create(
        [FromForm] CreateBannerRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var image = await request.Image.ToUploadModelAsync(cancellationToken);
        var result = await _bannerService.CreateAsync(request, image, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<BannerDto>.Ok(result, "تم إنشاء البانر بنجاح."));
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
        var cancellationToken = HttpContext.RequestAborted;

        var image = await request.Image.ToUploadModelAsync(cancellationToken);
        var result = await _bannerService.UpdateAsync(id, request, image, cancellationToken);

        return Ok(ApiResponse<BannerDto>.Ok(result, "تم تحديث البانر بنجاح."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _bannerService.DeleteAsync(id, HttpContext.RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف البانر بنجاح."));
    }
}
