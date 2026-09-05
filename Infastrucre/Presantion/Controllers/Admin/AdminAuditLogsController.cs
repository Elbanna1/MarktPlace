using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/audit-logs")]
[Tags("Admin Audit")]
[AdminPage(AdminPageCatalog.Keys.AuditLogs)]
public class AdminAuditLogsController : AdminApiController
{
    private readonly IAdminAuditService _audit;

    public AdminAuditLogsController(IAdminAuditService audit)
    {
        _audit = audit;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminAuditLogDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminAuditLogDto>>>> GetLogs(
        [FromQuery] AdminAuditLogFilterParams filter)
    {
        var result = await _audit.GetAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminAuditLogDto>>.Ok(result, "تم استرجاع سجل الإجراءات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdminAuditLogDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminAuditLogDto>>> GetLog(Guid id)
    {
        var result = await _audit.GetByIdAsync(id, RequestAborted);

        return Ok(ApiResponse<AdminAuditLogDto>.Ok(result, "تم استرجاع سجل الإجراء."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("metadata")]
    [ProducesResponseType(typeof(ApiResponse<AdminAuditMetadataDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AdminAuditMetadataDto>>> GetMetadata()
    {
        var result = await _audit.GetMetadataAsync(RequestAborted);

        return Ok(ApiResponse<AdminAuditMetadataDto>.Ok(result, "تم استرجاع بيانات سجل الإجراءات."));
    }
}
