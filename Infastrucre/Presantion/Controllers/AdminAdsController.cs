using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Responses;
using Shared.Authorization;

namespace Presentation.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/admin/ads")]
[Authorize(Roles = AppRoles.Admin)]
[Produces("application/json")]
[AdminPage(AdminPageCatalog.Keys.Ads)]
public class AdminAdsController : ControllerBase
{
    private readonly IAdminAdService _service;

    public AdminAdsController(IAdminAdService service)
    {
        _service = service;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminAdListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminAdListItemDto>>>> GetAds(
        [FromQuery] AdminAdFilterParams filter)
    {
        var result = await _service.GetAdsAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminAdListItemDto>>.Ok(result, "تم استرجاع الإعلانات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("pending")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminAdListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminAdListItemDto>>>> GetPendingAds(
        [FromQuery] AdminAdFilterParams filter)
    {
        filter.ModerationStatus = ModerationStatus.Pending;

        var result = await _service.GetAdsAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminAdListItemDto>>.Ok(
            result, "تم استرجاع الإعلانات قيد المراجعة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("pending/count")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<int>>> GetPendingCount()
    {
        var count = await _service.GetPendingCountAsync(HttpContext.RequestAborted);

        return Ok(ApiResponse<int>.Ok(count, "تم استرجاع عدد الإعلانات قيد المراجعة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("metadata")]
    [ProducesResponseType(typeof(ApiResponse<AdminAdMetadataDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AdminAdMetadataDto>>> GetMetadata()
    {
        var metadata = await _service.GetMetadataAsync(HttpContext.RequestAborted);

        return Ok(ApiResponse<AdminAdMetadataDto>.Ok(metadata, "تم استرجاع بيانات المراجعة."));
    }

    [RequireAdminPermission(AdminPermission.Approve)]
    [HttpPatch("{type}/{id:guid}/approve")]
    [ProducesResponseType(typeof(ApiResponse<AdminModerationResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminModerationResultDto>>> Approve(
        ListingModuleType type, Guid id, [FromBody] ApproveListingRequest? request)
    {
        var result = await _service.ApproveAsync(
            type, id, CurrentAdminId, request ?? new ApproveListingRequest(), HttpContext.RequestAborted);

        return Ok(ApiResponse<AdminModerationResultDto>.Ok(result, "تمت الموافقة على الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Reject)]
    [HttpPatch("{type}/{id:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<AdminModerationResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminModerationResultDto>>> Reject(
        ListingModuleType type, Guid id, [FromBody] RejectListingRequest request)
    {
        var result = await _service.RejectAsync(
            type, id, CurrentAdminId, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<AdminModerationResultDto>.Ok(result, "تم رفض الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPatch("{type}/{id:guid}/suspend")]
    [ProducesResponseType(typeof(ApiResponse<AdminModerationResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminModerationResultDto>>> Suspend(
        ListingModuleType type, Guid id, [FromBody] SuspendListingRequest? request)
    {
        var result = await _service.SuspendAsync(
            type, id, CurrentAdminId, request ?? new SuspendListingRequest(), HttpContext.RequestAborted);

        return Ok(ApiResponse<AdminModerationResultDto>.Ok(result, "تم إيقاف الإعلان."));
    }

    private string CurrentAdminId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
