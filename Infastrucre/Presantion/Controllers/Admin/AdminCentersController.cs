using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/moderation")]
[Tags("Admin Moderation")]
[AdminPage(AdminPageCatalog.Keys.Dashboard)]
public class AdminModerationController : AdminApiController
{
    private readonly IAdminDashboardService _dashboard;

    public AdminModerationController(IAdminDashboardService dashboard)
    {
        _dashboard = dashboard;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("overview")]
    [ProducesResponseType(typeof(ApiResponse<AdminModerationOverviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminModerationOverviewDto>>> GetOverview(
        [FromQuery] int recent = 5)
    {
        var result = await _dashboard.GetModerationOverviewAsync(recent, RequestAborted);

        return Ok(ApiResponse<AdminModerationOverviewDto>.Ok(result, "تم استرجاع ملخص المراجعة."));
    }
}

[Route(ApiVersions.AdminRoutePrefix + "/banner-center")]
[Tags("Admin Banners")]
[AdminPage(AdminPageCatalog.Keys.BannerRequests)]
public class AdminBannerCenterController : AdminApiController
{
    private readonly IAdminDashboardService _dashboard;

    public AdminBannerCenterController(IAdminDashboardService dashboard)
    {
        _dashboard = dashboard;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<AdminBannerCenterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminBannerCenterDto>>> Get()
    {
        var result = await _dashboard.GetBannerCenterAsync(RequestAborted);

        return Ok(ApiResponse<AdminBannerCenterDto>.Ok(result, "تم استرجاع مركز البانرات."));
    }
}
