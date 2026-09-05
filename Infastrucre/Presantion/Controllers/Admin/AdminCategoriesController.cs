using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/categories")]
[Tags("Admin Categories")]
[AdminPage(AdminPageCatalog.Keys.Categories)]
public class AdminCategoriesController : AdminApiController
{
    private readonly IAdminCatalogService _catalog;

    public AdminCategoriesController(IAdminCatalogService catalog)
    {
        _catalog = catalog;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminCategoryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdminCategoryDto>>>> GetCategories()
    {
        var result = await _catalog.GetCategoriesAsync(RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<AdminCategoryDto>>.Ok(result, "تم استرجاع الأقسام."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminCategoryDto>>> GetCategory(int id)
    {
        var result = await _catalog.GetCategoryAsync(id, RequestAborted);

        return Ok(ApiResponse<AdminCategoryDto>.Ok(result, "تم استرجاع القسم."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AdminCategoryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminCategoryDto>>> CreateCategory(
        [FromBody] CreateCategoryRequest request)
    {
        var result = await _catalog.CreateCategoryAsync(request, RequestAborted);

        return CreatedAtAction(
            nameof(GetCategory),
            new { id = result.Id },
            ApiResponse<AdminCategoryDto>.Ok(result, "تم إنشاء القسم."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminCategoryDto>>> UpdateCategory(
        int id, [FromBody] UpdateCategoryRequest request)
    {
        var result = await _catalog.UpdateCategoryAsync(id, request, RequestAborted);

        return Ok(ApiResponse<AdminCategoryDto>.Ok(result, "تم تحديث القسم."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse>> DeleteCategory(int id)
    {
        await _catalog.DeleteCategoryAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف القسم."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(ApiResponse<AdminCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminCategoryDto>>> SetCategoryStatus(
        int id, [FromBody] UpdateActiveStatusRequest request)
    {
        var result = await _catalog.SetCategoryStatusAsync(id, request.IsActive, RequestAborted);

        return Ok(ApiResponse<AdminCategoryDto>.Ok(
            result, request.IsActive ? "تم تفعيل القسم." : "تم تعطيل القسم."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{categoryId:int}/subcategories")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminSubCategoryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdminSubCategoryDto>>>> GetSubCategories(
        int categoryId)
    {
        var result = await _catalog.GetSubCategoriesAsync(categoryId, RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<AdminSubCategoryDto>>.Ok(result, "تم استرجاع الأقسام الفرعية."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost("{categoryId:int}/subcategories")]
    [ProducesResponseType(typeof(ApiResponse<AdminSubCategoryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminSubCategoryDto>>> CreateSubCategory(
        int categoryId, [FromBody] CreateSubCategoryRequest request)
    {
        var result = await _catalog.CreateSubCategoryAsync(categoryId, request, RequestAborted);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<AdminSubCategoryDto>.Ok(result, "تم إنشاء القسم الفرعي."));
    }
}

[Route(ApiVersions.AdminRoutePrefix + "/subcategories")]
[Tags("Admin SubCategories")]
[AdminPage(AdminPageCatalog.Keys.Categories)]
public class AdminSubCategoriesController : AdminApiController
{
    private readonly IAdminCatalogService _catalog;

    public AdminSubCategoriesController(IAdminCatalogService catalog)
    {
        _catalog = catalog;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminSubCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminSubCategoryDto>>> GetSubCategory(int id)
    {
        var result = await _catalog.GetSubCategoryAsync(id, RequestAborted);

        return Ok(ApiResponse<AdminSubCategoryDto>.Ok(result, "تم استرجاع القسم الفرعي."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminSubCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminSubCategoryDto>>> UpdateSubCategory(
        int id, [FromBody] UpdateSubCategoryRequest request)
    {
        var result = await _catalog.UpdateSubCategoryAsync(id, request, RequestAborted);

        return Ok(ApiResponse<AdminSubCategoryDto>.Ok(result, "تم تحديث القسم الفرعي."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse>> DeleteSubCategory(int id)
    {
        await _catalog.DeleteSubCategoryAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف القسم الفرعي."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(ApiResponse<AdminSubCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminSubCategoryDto>>> SetSubCategoryStatus(
        int id, [FromBody] UpdateActiveStatusRequest request)
    {
        var result = await _catalog.SetSubCategoryStatusAsync(id, request.IsActive, RequestAborted);

        return Ok(ApiResponse<AdminSubCategoryDto>.Ok(
            result, request.IsActive ? "تم تفعيل القسم الفرعي." : "تم تعطيل القسم الفرعي."));
    }
}
