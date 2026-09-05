using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix)]
[Tags("Admin Dynamic Forms")]
[AdminPage(AdminPageCatalog.Keys.Forms)]
public class AdminFormsController : AdminApiController
{
    private readonly IAdminFormService _forms;

    public AdminFormsController(IAdminFormService forms)
    {
        _forms = forms;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("forms/{categoryId:int}/{subCategoryId:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminFormSchemaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminFormSchemaDto>>> GetForm(
        int categoryId, int subCategoryId)
    {
        var result = await _forms.GetFormAsync(categoryId, subCategoryId, RequestAborted);

        return Ok(ApiResponse<AdminFormSchemaDto>.Ok(result, "تم استرجاع نموذج الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("forms/{categoryId:int}")]
    [ProducesResponseType(typeof(ApiResponse<AdminFormSchemaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AdminFormSchemaDto>>> GetForm(int categoryId)
    {
        var result = await _forms.GetFormAsync(categoryId, null, RequestAborted);

        return Ok(ApiResponse<AdminFormSchemaDto>.Ok(result, "تم استرجاع نموذج الإعلان."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost("forms/{categoryId:int}/{subCategoryId:int}/fields")]
    [ProducesResponseType(typeof(ApiResponse<AdminFormFieldDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminFormFieldDto>>> CreateField(
        int categoryId, int subCategoryId, [FromBody] CreateFormFieldRequest request)
    {
        var result = await _forms.CreateFieldAsync(
            categoryId, subCategoryId, CurrentAdminId, request, RequestAborted);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<AdminFormFieldDto>.Ok(result, "تم إضافة الحقل."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("form-fields/{fieldId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdminFormFieldDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminFormFieldDto>>> UpdateField(
        Guid fieldId, [FromBody] UpdateFormFieldRequest request)
    {
        var result = await _forms.UpdateFieldAsync(fieldId, CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<AdminFormFieldDto>.Ok(result, "تم تحديث الحقل."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("form-fields/{fieldId:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> DeleteField(Guid fieldId)
    {
        await _forms.DeleteFieldAsync(fieldId, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف الحقل."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("forms/{categoryId:int}/{subCategoryId:int}/fields/{fieldName}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> ResetField(
        int categoryId, int subCategoryId, string fieldName)
    {
        await _forms.ResetFieldAsync(categoryId, subCategoryId, fieldName, RequestAborted);

        return Ok(ApiResponse.Ok("تمت استعادة الحقل إلى إعداداته الأصلية."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost("form-fields/{fieldId:guid}/options")]
    [ProducesResponseType(typeof(ApiResponse<AdminFormOptionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AdminFormOptionDto>>> AddOption(
        Guid fieldId, [FromBody] SaveFormOptionRequest request)
    {
        var result = await _forms.AddOptionAsync(fieldId, CurrentAdminId, request, RequestAborted);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<AdminFormOptionDto>.Ok(result, "تم إضافة الخيار."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("form-options/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdminFormOptionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminFormOptionDto>>> UpdateOption(
        Guid id, [FromBody] SaveFormOptionRequest request)
    {
        var result = await _forms.UpdateOptionAsync(id, request, RequestAborted);

        return Ok(ApiResponse<AdminFormOptionDto>.Ok(result, "تم تحديث الخيار."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("form-options/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> DeleteOption(Guid id)
    {
        await _forms.DeleteOptionAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف الخيار."));
    }
}
