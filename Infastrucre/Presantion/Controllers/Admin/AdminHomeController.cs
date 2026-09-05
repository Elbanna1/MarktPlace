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

[Route(ApiVersions.AdminRoutePrefix + "/home")]
[Tags("Admin Home")]
[AdminPage(AdminPageCatalog.Keys.Home)]
public class AdminHomeController : AdminApiController
{
    private readonly IAdminHomeService _home;

    public AdminHomeController(IAdminHomeService home)
    {
        _home = home;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<AdminHomeConfigurationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminHomeConfigurationDto>>> Get()
    {
        var result = await _home.GetAsync(RequestAborted);

        return Ok(ApiResponse<AdminHomeConfigurationDto>.Ok(result, "تم استرجاع إعدادات الصفحة الرئيسية."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("sections/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminHomeSectionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminHomeSectionDto>>> UpdateSection(
        int id, [FromBody] UpdateHomeSectionRequest request)
    {
        var result = await _home.UpdateSectionAsync(id, CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<AdminHomeSectionDto>.Ok(result, "تم تحديث القسم."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("sections/{id:int}/image")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxFileSizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<AdminHomeSectionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminHomeSectionDto>>> UpdateSectionImage(
        int id, IFormFile image)
    {
        var upload = await image.ToUploadModelAsync(RequestAborted);

        var result = await _home.UpdateSectionImageAsync(id, CurrentAdminId, upload, RequestAborted);

        return Ok(ApiResponse<AdminHomeSectionDto>.Ok(result, "تم تحديث صورة القسم."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpDelete("sections/{id:int}/image")]
    [ProducesResponseType(typeof(ApiResponse<AdminHomeSectionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminHomeSectionDto>>> DeleteSectionImage(int id)
    {
        var result = await _home.DeleteSectionImageAsync(id, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<AdminHomeSectionDto>.Ok(result, "تم حذف صورة القسم."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPut("order")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminHomeSectionDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdminHomeSectionDto>>>> Reorder(
        [FromBody] ReorderHomeSectionsRequest request)
    {
        var result = await _home.ReorderAsync(request, RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<AdminHomeSectionDto>>.Ok(result, "تم تحديث ترتيب الأقسام."));
    }
}
