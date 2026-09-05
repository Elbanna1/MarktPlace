using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/referrals")]
[Tags("Admin Referrals")]
[AdminPage(AdminPageCatalog.Keys.Referrals)]
public class AdminReferralsController : AdminApiController
{
    private readonly IAdminReferralService _referrals;

    public AdminReferralsController(IAdminReferralService referrals)
    {
        _referrals = referrals;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminReferralDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminReferralDto>>>> GetReferrals(
        [FromQuery] AdminReferralFilterParams filter)
    {
        var result = await _referrals.GetReferralsAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminReferralDto>>.Ok(result, "تم استرجاع سجل الدعوات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(ApiResponse<AdminReferralStatisticsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminReferralStatisticsDto>>> GetStatistics(
        [FromQuery] int top = 5)
    {
        var result = await _referrals.GetStatisticsAsync(top, RequestAborted);

        return Ok(ApiResponse<AdminReferralStatisticsDto>.Ok(result, "تم استرجاع إحصائيات الدعوات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("referrers")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminReferrerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminReferrerDto>>>> GetReferrers(
        [FromQuery] AdminReferrerFilterParams filter)
    {
        var result = await _referrals.GetReferrersAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminReferrerDto>>.Ok(result, "تم استرجاع أصحاب الدعوات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("statuses")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminOptionDto>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<AdminOptionDto>>> GetStatuses()
    {
        var statuses = ReferralCatalog.Options
            .Select(option => new AdminOptionDto { Id = (int)option.Status, Name = option.Name })
            .ToList();

        return Ok(ApiResponse<IReadOnlyList<AdminOptionDto>>.Ok(statuses, "تم استرجاع حالات الدعوات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdminReferralDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminReferralDto>>> GetReferral(Guid id)
    {
        var result = await _referrals.GetReferralAsync(id, RequestAborted);

        return Ok(ApiResponse<AdminReferralDto>.Ok(result, "تم استرجاع بيانات الدعوة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("/" + ApiVersions.AdminRoutePrefix + "/users/{userId}/referrals")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminReferralDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminReferralDto>>>> GetUserReferrals(
        string userId, [FromQuery] AdminReferralFilterParams filter)
    {
        var result = await _referrals.GetForUserAsync(userId, filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminReferralDto>>.Ok(result, "تم استرجاع دعوات المستخدم."));
    }
}
