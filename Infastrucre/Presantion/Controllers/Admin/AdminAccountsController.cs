using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Authorization;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/admins")]
[Tags("Admin Accounts (Super Admin)")]
[SuperAdminOnly]
public class AdminAccountsController : AdminApiController
{
    private readonly IAdminAccountService _admins;

    public AdminAccountsController(IAdminAccountService admins)
    {
        _admins = admins;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminAccountListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminAccountListItemDto>>>> GetAdmins(
        [FromQuery] AdminAccountFilterParams filter)
    {
        var result = await _admins.GetAdminsAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminAccountListItemDto>>.Ok(result, "تم استرجاع المسؤولين."));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AdminAccountDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminAccountDetailsDto>>> GetAdmin(string id)
    {
        var result = await _admins.GetAdminAsync(id, RequestAborted);

        return Ok(ApiResponse<AdminAccountDetailsDto>.Ok(result, "تم استرجاع بيانات المسؤول."));
    }

    [HttpGet("candidates")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminCandidateUserDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdminCandidateUserDto>>>> SearchCandidates(
        [FromQuery] AdminCandidateSearchParams filter)
    {
        var result = await _admins.SearchCandidatesAsync(filter, RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<AdminCandidateUserDto>>.Ok(
            result,
            result.Count == 0 ? "مفيش مستخدم مطابق." : "تم استرجاع المستخدمين المطابقين."));
    }

    [HttpGet("candidates/{userId}")]
    [ProducesResponseType(typeof(ApiResponse<AdminCandidateUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminCandidateUserDto>>> GetCandidate(string userId)
    {
        var result = await _admins.GetCandidateAsync(userId, RequestAborted);

        return Ok(ApiResponse<AdminCandidateUserDto>.Ok(result, "تم استرجاع بيانات المستخدم."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AdminAccountDetailsDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<AdminAccountDetailsDto>>> ConfirmAdmin(
        [FromBody] ConfirmAdminRequest request)
    {
        var result = await _admins.ConfirmAdminAsync(request, CurrentAdminId, RequestAborted);

        return CreatedAtAction(
            nameof(GetAdmin),
            new { id = result.Id },
            ApiResponse<AdminAccountDetailsDto>.Ok(result, "تم اعتماد المستخدم كمسؤول."));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AdminAccountDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminAccountDetailsDto>>> UpdateAdmin(
        string id, [FromBody] UpdateAdminRequest request)
    {
        var result = await _admins.UpdateAdminAsync(id, request, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<AdminAccountDetailsDto>.Ok(result, "تم تحديث بيانات المسؤول."));
    }

    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(ApiResponse<AdminAccountDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminAccountDetailsDto>>> SetStatus(
        string id, [FromBody] UpdateAdminStatusRequest request)
    {
        var result = await _admins.SetStatusAsync(id, request, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<AdminAccountDetailsDto>.Ok(
            result, request.IsActive ? "تم تفعيل المسؤول." : "تم إيقاف المسؤول."));
    }

    [HttpPut("{id}/permissions")]
    [ProducesResponseType(typeof(ApiResponse<AdminAccountDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<AdminAccountDetailsDto>>> UpdatePermissions(
        string id, [FromBody] UpdateAdminPermissionsRequest request)
    {
        var result = await _admins.UpdatePermissionsAsync(id, request, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<AdminAccountDetailsDto>.Ok(result, "تم تحديث صلاحيات المسؤول."));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> RevokeAdmin(string id)
    {
        await _admins.RevokeAdminAsync(id, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse.Ok("تم سحب صلاحية المسؤول."));
    }
}

[Route(ApiVersions.AdminRoutePrefix)]
[Tags("Admin Permissions")]
public class AdminPermissionsController : AdminApiController
{
    private readonly IAdminPermissionService _permissions;

    public AdminPermissionsController(IAdminPermissionService permissions)
    {
        _permissions = permissions;
    }

    [HttpGet("permissions/pages")]
    [AdminSelfService]
    [ProducesResponseType(typeof(ApiResponse<AdminPermissionCatalogDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    public ActionResult<ApiResponse<AdminPermissionCatalogDto>> GetPages()
    {
        var catalog = _permissions.GetCatalog();

        return Ok(ApiResponse<AdminPermissionCatalogDto>.Ok(catalog, "تم استرجاع صفحات لوحة التحكم."));
    }

    [HttpGet("me/permissions")]
    [AdminSelfService]
    [ProducesResponseType(typeof(ApiResponse<AdminPermissionsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminPermissionsDto>>> GetMyPermissions()
    {
        var result = await _permissions.GetMyPermissionsAsync(CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<AdminPermissionsDto>.Ok(result, "تم استرجاع الصلاحيات."));
    }
}
