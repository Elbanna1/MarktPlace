using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/governorates")]
[Tags("Admin Locations")]
[AdminPage(AdminPageCatalog.Keys.Locations)]
public class AdminGovernoratesController : AdminApiController
{
    private readonly IAdminLocationService _locations;

    public AdminGovernoratesController(IAdminLocationService locations)
    {
        _locations = locations;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminGovernorateDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdminGovernorateDto>>>> GetAll()
    {
        var result = await _locations.GetGovernoratesAsync(RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<AdminGovernorateDto>>.Ok(result, "تم استرجاع المحافظات."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AdminGovernorateDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminGovernorateDto>>> Create(
        [FromBody] SaveLocationRequest request)
    {
        var result = await _locations.CreateGovernorateAsync(request, RequestAborted);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<AdminGovernorateDto>.Ok(result, "تم إضافة المحافظة."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminGovernorateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminGovernorateDto>>> Update(
        int id, [FromBody] SaveLocationRequest request)
    {
        var result = await _locations.UpdateGovernorateAsync(id, request, RequestAborted);

        return Ok(ApiResponse<AdminGovernorateDto>.Ok(result, "تم تحديث المحافظة."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        await _locations.DeleteGovernorateAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف المحافظة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{governorateId:int}/centers")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminCenterDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdminCenterDto>>>> GetCenters(
        int governorateId)
    {
        var result = await _locations.GetCentersAsync(governorateId, RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<AdminCenterDto>>.Ok(result, "تم استرجاع المراكز."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost("{governorateId:int}/centers")]
    [ProducesResponseType(typeof(ApiResponse<AdminCenterDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminCenterDto>>> CreateCenter(
        int governorateId, [FromBody] SaveLocationRequest request)
    {
        var result = await _locations.CreateCenterAsync(governorateId, request, RequestAborted);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<AdminCenterDto>.Ok(result, "تم إضافة المركز."));
    }
}

[Route(ApiVersions.AdminRoutePrefix + "/centers")]
[Tags("Admin Locations")]
[AdminPage(AdminPageCatalog.Keys.Locations)]
public class AdminCentersController : AdminApiController
{
    private readonly IAdminLocationService _locations;

    public AdminCentersController(IAdminLocationService locations)
    {
        _locations = locations;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminCenterDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdminCenterDto>>>> GetAll(
        [FromQuery] int? governorateId)
    {
        var result = governorateId is { } id
            ? await _locations.GetCentersAsync(id, RequestAborted)
            : (await _locations.GetGovernoratesAsync(RequestAborted))
                .SelectMany(governorate => governorate.Centers)
                .ToList();

        return Ok(ApiResponse<IReadOnlyList<AdminCenterDto>>.Ok(result, "تم استرجاع المراكز."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminCenterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminCenterDto>>> Update(
        int id, [FromBody] SaveLocationRequest request)
    {
        var result = await _locations.UpdateCenterAsync(id, request, RequestAborted);

        return Ok(ApiResponse<AdminCenterDto>.Ok(result, "تم تحديث المركز."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        await _locations.DeleteCenterAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف المركز."));
    }
}

[Route(ApiVersions.AdminRoutePrefix + "/projects")]
[Tags("Admin Locations")]
[AdminPage(AdminPageCatalog.Keys.Locations)]
public class AdminProjectsController : AdminApiController
{
    private readonly IAdminLocationService _locations;

    public AdminProjectsController(IAdminLocationService locations)
    {
        _locations = locations;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminProjectDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdminProjectDto>>>> GetAll(
        [FromQuery] int? centerId)
    {
        var result = await _locations.GetProjectsAsync(centerId, RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<AdminProjectDto>>.Ok(result, "تم استرجاع المشاريع."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AdminProjectDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminProjectDto>>> Create(
        [FromBody] CreateProjectRequest request)
    {
        var result = await _locations.CreateProjectAsync(request, RequestAborted);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<AdminProjectDto>.Ok(result, "تم إضافة المشروع."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminProjectDto>>> Update(
        int id, [FromBody] UpdateProjectRequest request)
    {
        var result = await _locations.UpdateProjectAsync(id, request, RequestAborted);

        return Ok(ApiResponse<AdminProjectDto>.Ok(result, "تم تحديث المشروع."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        await _locations.DeleteProjectAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف المشروع."));
    }
}
