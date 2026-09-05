using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/settings")]
[Tags("Admin Settings")]
[AdminPage(AdminPageCatalog.Keys.Settings)]
public class AdminSettingsController : AdminApiController
{
    private readonly IAdminSettingsService _settings;

    public AdminSettingsController(IAdminSettingsService settings)
    {
        _settings = settings;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<AdminSettingsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminSettingsDto>>> Get()
    {
        var result = await _settings.GetAsync(RequestAborted);

        return Ok(ApiResponse<AdminSettingsDto>.Ok(result, "تم استرجاع إعدادات المنصة."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<AdminSettingsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AdminSettingsDto>>> Update(
        [FromBody] UpdateSettingsRequest request)
    {
        var result = await _settings.UpdateAsync(CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<AdminSettingsDto>.Ok(result, "تم تحديث إعدادات المنصة."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("logo")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxFileSizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<AdminSettingsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AdminSettingsDto>>> UpdateLogo(IFormFile logo)
    {
        var upload = await logo.ToUploadModelAsync(RequestAborted);

        var result = await _settings.UpdateLogoAsync(CurrentAdminId, upload, RequestAborted);

        return Ok(ApiResponse<AdminSettingsDto>.Ok(result, "تم تحديث الشعار."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("favicon")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxFileSizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<AdminSettingsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AdminSettingsDto>>> UpdateFavicon(IFormFile favicon)
    {
        var upload = await favicon.ToUploadModelAsync(RequestAborted);

        var result = await _settings.UpdateFaviconAsync(CurrentAdminId, upload, RequestAborted);

        return Ok(ApiResponse<AdminSettingsDto>.Ok(result, "تم تحديث الأيقونة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("upload-limits")]
    [ProducesResponseType(typeof(ApiResponse<AdminUploadLimitsDto>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<AdminUploadLimitsDto>> GetUploadLimits() =>
        Ok(ApiResponse<AdminUploadLimitsDto>.Ok(
            _settings.GetUploadLimits(), "تم استرجاع حدود رفع الملفات."));
}

[Route(ApiVersions.AdminRoutePrefix + "/uploads")]
[Tags("Admin Settings")]
[AdminPage(AdminPageCatalog.Keys.Settings)]
public class AdminUploadsController : AdminApiController
{
    private readonly IAdminSettingsService _settings;

    public AdminUploadsController(IAdminSettingsService settings)
    {
        _settings = settings;
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxVideoSizeBytes + (5 * 1024 * 1024))]
    [ProducesResponseType(typeof(ApiResponse<AdminUploadResultDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status413PayloadTooLarge)]
    public async Task<ActionResult<ApiResponse<AdminUploadResultDto>>> Upload(
        IFormFile file,
        [FromQuery] string folder = ImageConstants.PlatformFolder,
        [FromQuery] AdminUploadKind kind = AdminUploadKind.Image)
    {
        var upload = await file.ToUploadModelAsync(RequestAborted);

        var result = await _settings.UploadAsync(upload, folder, kind, RequestAborted);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<AdminUploadResultDto>.Ok(result, "تم رفع الملف."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("folders")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<string>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<string>>> GetFolders() =>
        Ok(ApiResponse<IReadOnlyList<string>>.Ok(
            ImageConstants.AllFolders.OrderBy(folder => folder).ToList(),
            "تم استرجاع مجلدات الرفع."));
}
