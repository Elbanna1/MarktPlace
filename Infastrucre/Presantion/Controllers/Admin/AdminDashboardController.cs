using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/dashboard")]
[Tags("Admin Dashboard")]
[AdminPage(AdminPageCatalog.Keys.Dashboard)]
public class AdminDashboardController : AdminApiController
{
    private readonly IAdminDashboardService _dashboard;

    public AdminDashboardController(IAdminDashboardService dashboard)
    {
        _dashboard = dashboard;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<AdminDashboardDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminDashboardDto>>> Get([FromQuery] int latest = 5)
    {
        var result = await _dashboard.GetAsync(latest, RequestAborted);

        return Ok(ApiResponse<AdminDashboardDto>.Ok(result, "تم استرجاع إحصائيات لوحة التحكم."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("analytics")]
    [ProducesResponseType(typeof(ApiResponse<AdminAnalyticsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminAnalyticsDto>>> GetAnalytics(
        [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var result = await _dashboard.GetAnalyticsAsync(from, to, RequestAborted);

        return Ok(ApiResponse<AdminAnalyticsDto>.Ok(result, "تم استرجاع التحليلات."));
    }
}
