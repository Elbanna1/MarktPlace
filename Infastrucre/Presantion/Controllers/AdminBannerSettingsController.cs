using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.BannerBookings;
using Shared.Enums;
using Shared.Responses;
using Shared.Authorization;

namespace Presentation.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/admin/banner-settings")]
[Authorize(Roles = AppRoles.Admin)]
[Produces("application/json")]
[AdminPage(AdminPageCatalog.Keys.BannerRequests)]
public class AdminBannerSettingsController : ControllerBase
{
    private readonly IBannerSettingsService _settingsService;

    public AdminBannerSettingsController(IBannerSettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerPlacementDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerPlacementDto>>>> GetAll()
    {
        var result = await _settingsService.GetAllAsync(HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<BannerPlacementDto>>.Ok(result, "تم استرجاع إعدادات البانرات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{location}")]
    [ProducesResponseType(typeof(ApiResponse<BannerPlacementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BannerPlacementDto>>> Get(BannerLocation location)
    {
        var result = await _settingsService.GetAsync(location, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerPlacementDto>.Ok(result, "تم استرجاع إعدادات المكان الإعلاني."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPut("{location}")]
    [ProducesResponseType(typeof(ApiResponse<BannerPlacementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<BannerPlacementDto>>> Update(
        BannerLocation location, [FromBody] UpdateBannerPlacementRequest request)
    {
        var result = await _settingsService.UpdateAsync(location, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<BannerPlacementDto>.Ok(result, "تم تحديث إعدادات المكان الإعلاني."));
    }
}
