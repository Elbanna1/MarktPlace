using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/users")]
[Tags("Admin Users")]
[AdminPage(AdminPageCatalog.Keys.Users)]
public class AdminUsersController : AdminApiController
{
    private readonly IAdminUserService _users;

    public AdminUsersController(IAdminUserService users)
    {
        _users = users;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminUserListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminUserListItemDto>>>> GetUsers(
        [FromQuery] AdminUserFilterParams filter)
    {
        var result = await _users.GetUsersAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminUserListItemDto>>.Ok(result, "تم استرجاع المستخدمين."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AdminUserDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminUserDetailsDto>>> GetUser(string id)
    {
        var result = await _users.GetUserAsync(id, RequestAborted);

        return Ok(ApiResponse<AdminUserDetailsDto>.Ok(result, "تم استرجاع بيانات المستخدم."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(ApiResponse<AdminUserDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminUserDetailsDto>>> UpdateStatus(
        string id, [FromBody] UpdateUserStatusRequest request)
    {
        var result = await _users.UpdateStatusAsync(id, CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<AdminUserDetailsDto>.Ok(result, "تم تحديث حالة المستخدم."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("statuses")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminOptionDto>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<AdminOptionDto>>> GetStatuses()
    {
        var statuses = UserAccountCatalog.Options
            .Select(option => new AdminOptionDto { Id = (int)option.Status, Name = option.Name })
            .ToList();

        return Ok(ApiResponse<IReadOnlyList<AdminOptionDto>>.Ok(statuses, "تم استرجاع حالات المستخدمين."));
    }
}
