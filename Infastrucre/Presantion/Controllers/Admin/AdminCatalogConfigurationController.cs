using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Authorization;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Catalog;
using Shared.Enums;
using Shared.Responses;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/catalog-configuration")]
[Tags("Catalog Configuration")]
[AdminPage(AdminPageCatalog.Keys.Categories)]
public class AdminCatalogConfigurationController : AdminApiController
{
    private readonly ICatalogConfigurationService _configuration;
    private readonly IAdminPermissionService _permissions;

    public AdminCatalogConfigurationController(
        ICatalogConfigurationService configuration, IAdminPermissionService permissions)
    {
        _configuration = configuration;
        _permissions = permissions;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [EndpointSummary("Full category, sub-category and module hierarchy with explicit and effective configuration")]
    [ProducesResponseType(typeof(ApiResponse<CatalogHierarchyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<CatalogHierarchyDto>>> GetHierarchy()
    {
        var result = await _configuration.GetHierarchyAsync(RequestAborted);

        Response.Headers.CacheControl = "no-store";

        return Ok(ApiResponse<CatalogHierarchyDto>.Ok(result, "تم استرجاع إعدادات الأقسام."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("options")]
    [EndpointSummary("Node types, application areas, statuses and limits for the dashboard")]
    [ProducesResponseType(typeof(ApiResponse<CatalogConfigurationOptionsDto>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<CatalogConfigurationOptionsDto>> GetOptions() =>
        Ok(ApiResponse<CatalogConfigurationOptionsDto>.Ok(_configuration.GetOptions(), "تم استرجاع خيارات الإعدادات."));

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("nodes/{nodeType}/{nodeId:int}")]
    [EndpointSummary("One category, sub-category or module with its explicit and effective configuration")]
    [EndpointDescription("nodeType is Category, SubCategory or Module (or 1, 2, 3). A module nodeId is its ListingModuleType number.")]
    [ProducesResponseType(typeof(ApiResponse<CatalogNodeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CatalogNodeDto>>> GetNode(string nodeType, int nodeId)
    {
        var result = await _configuration.GetNodeAsync(nodeType, nodeId, RequestAborted);

        Response.Headers.CacheControl = "no-store";

        return Ok(ApiResponse<CatalogNodeDto>.Ok(result, "تم استرجاع إعدادات القسم."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("effective")]
    [EndpointSummary("Effective payment and visibility for an ad type, category or sub-category after inheritance")]
    [ProducesResponseType(typeof(ApiResponse<CatalogEffectiveConfigurationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CatalogEffectiveConfigurationDto>>> GetEffective(
        [FromQuery] CatalogEffectiveQuery query)
    {
        var result = await _configuration.GetEffectiveAsync(query, RequestAborted);

        Response.Headers.CacheControl = "no-store";

        return Ok(ApiResponse<CatalogEffectiveConfigurationDto>.Ok(result, "تم حساب الإعدادات الفعلية."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("scheduled")]
    [EndpointSummary("Configuration rows with a future activation or expiration date")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CatalogScheduledConfigurationDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CatalogScheduledConfigurationDto>>>> GetScheduled()
    {
        var result = await _configuration.GetScheduledAsync(RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<CatalogScheduledConfigurationDto>>.Ok(result, "تم استرجاع الإعدادات المجدولة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("history")]
    [EndpointSummary("Audit history of configuration changes")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminAuditLogDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminAuditLogDto>>>> GetHistory(
        [FromQuery] AdminAuditLogFilterParams filter)
    {
        var result = await _configuration.GetHistoryAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminAuditLogDto>>.Ok(result, "تم استرجاع سجل التعديلات."));
    }

    [RequireAdminPermission(AdminPageCatalog.Keys.Payments, AdminPermission.Edit)]
    [HttpPut("payment")]
    [EndpointSummary("Enable, disable, price or schedule ad payments for one or many nodes")]
    [EndpointDescription(
        "Needs Edit on the Payments page. Every target gets the same rule and the whole request is atomic. " +
        "Enabling is refused (400) for the always-free charity category and every node under it.")]
    [ProducesResponseType(typeof(ApiResponse<CatalogBulkResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<CatalogBulkResultDto>>> UpdatePayment(
        [FromBody] UpdateCatalogPaymentRequest request)
    {
        var result = await _configuration.UpdatePaymentAsync(request, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<CatalogBulkResultDto>.Ok(result, "تم حفظ إعدادات رسوم النشر."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("visibility")]
    [EndpointSummary("Show, hide, enable, disable or schedule nodes in selected application areas")]
    [ProducesResponseType(typeof(ApiResponse<CatalogBulkResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<CatalogBulkResultDto>>> UpdateVisibility(
        [FromBody] UpdateCatalogVisibilityRequest request)
    {
        var result = await _configuration.UpdateVisibilityAsync(request, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<CatalogBulkResultDto>.Ok(result, "تم حفظ إعدادات الظهور."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpPost("reset")]
    [EndpointSummary("Remove explicit rules so the nodes inherit from their parent again")]
    [EndpointDescription(
        "kind Payment needs Edit on the Payments page; kind Visibility needs Edit on the Categories page. " +
        "areas narrows a Visibility reset; omitted means every area.")]
    [ProducesResponseType(typeof(ApiResponse<CatalogBulkResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<CatalogBulkResultDto>>> Reset(
        [FromBody] ResetCatalogConfigurationRequest request)
    {
        var kind = _configuration.ParseKind(request.Kind);

        var access = await _permissions.AuthorizeAsync(
            CurrentAdminId,
            kind == CatalogConfigurationKind.Payment ? AdminPageCatalog.Keys.Payments : AdminPageCatalog.Keys.Categories,
            AdminPermission.Edit,
            RequestAborted);

        if (!access.Allowed)
            return StatusCode(access.StatusCode, ApiResponse.Fail(access.Message ?? "مالكش صلاحية توصل للمحتوى ده."));

        var result = await _configuration.ResetAsync(request, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<CatalogBulkResultDto>.Ok(result, "تم إرجاع الإعدادات للوراثة."));
    }
}
