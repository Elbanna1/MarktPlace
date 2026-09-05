using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Responses;
using Shared.Authorization;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/ads")]
[Tags("Admin Ads")]
[AdminPage(AdminPageCatalog.Keys.Ads)]
public class AdminAdsV2Controller : AdminApiController
{
    private readonly IAdminAdService _ads;

    public AdminAdsV2Controller(IAdminAdService ads)
    {
        _ads = ads;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminAdListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminAdListItemDto>>>> GetAds(
        [FromQuery] AdminAdFilterParams filter)
    {
        var result = await _ads.GetAdsAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminAdListItemDto>>.Ok(result, "تم استرجاع الإعلانات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("pending")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminAdListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminAdListItemDto>>>> GetPendingAds(
        [FromQuery] AdminAdFilterParams filter)
    {
        filter.ModerationStatus = ModerationStatus.Pending;

        var result = await _ads.GetAdsAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminAdListItemDto>>.Ok(
            result, "تم استرجاع الإعلانات قيد المراجعة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("pending/count")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<int>>> GetPendingCount()
    {
        var count = await _ads.GetPendingCountAsync(RequestAborted);

        return Ok(ApiResponse<int>.Ok(count, "تم استرجاع عدد الإعلانات قيد المراجعة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("metadata")]
    [ProducesResponseType(typeof(ApiResponse<AdminAdMetadataDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AdminAdMetadataDto>>> GetMetadata()
    {
        var metadata = await _ads.GetMetadataAsync(RequestAborted);

        return Ok(ApiResponse<AdminAdMetadataDto>.Ok(metadata, "تم استرجاع بيانات المراجعة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{type}/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdminAdDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminAdDetailsDto>>> GetAd(
        ListingModuleType type, Guid id)
    {
        var result = await _ads.GetAdDetailsAsync(type, id, RequestAborted);

        return Ok(ApiResponse<AdminAdDetailsDto>.Ok(result, "تم استرجاع تفاصيل الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Approve)]
    [HttpPatch("{type}/{id:guid}/approve")]
    [ProducesResponseType(typeof(ApiResponse<AdminModerationResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminModerationResultDto>>> Approve(
        ListingModuleType type, Guid id, [FromBody] ApproveListingRequest? request)
    {
        var result = await _ads.ApproveAsync(
            type, id, CurrentAdminId, request ?? new ApproveListingRequest(), RequestAborted);

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
        var result = await _ads.RejectAsync(type, id, CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<AdminModerationResultDto>.Ok(result, "تم رفض الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPatch("{type}/{id:guid}/suspend")]
    [ProducesResponseType(typeof(ApiResponse<AdminModerationResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminModerationResultDto>>> Suspend(
        ListingModuleType type, Guid id, [FromBody] SuspendListingRequest? request)
    {
        var result = await _ads.SuspendAsync(
            type, id, CurrentAdminId, request ?? new SuspendListingRequest(), RequestAborted);

        return Ok(ApiResponse<AdminModerationResultDto>.Ok(result, "تم إيقاف الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{type}/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(ListingModuleType type, Guid id)
    {
        await _ads.DeleteAsync(type, id, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف الإعلان."));
    }
}
