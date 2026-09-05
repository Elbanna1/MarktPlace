using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Feedback;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/admin/feedback")]
[Authorize(Roles = AppRoles.Admin)]
[Produces("application/json")]
[AdminPage(AdminPageCatalog.Keys.Feedback)]
public class AdminFeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedback;

    public AdminFeedbackController(IFeedbackService feedback)
    {
        _feedback = feedback;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<FeedbackListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<FeedbackListItemDto>>>> GetAll(
        [FromQuery] FeedbackFilterParams filter)
    {
        var result = await _feedback.GetAllAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<FeedbackListItemDto>>.Ok(result, "تم استرجاع الملاحظات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FeedbackDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FeedbackDetailsDto>>> GetById(Guid id)
    {
        var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var result = await _feedback.GetByIdAsync(
            id, adminUserId, isAdmin: true, HttpContext.RequestAborted);

        return Ok(ApiResponse<FeedbackDetailsDto>.Ok(result, "تم استرجاع الملاحظة."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpPut("{id:guid}/status")]
    [ProducesResponseType(typeof(ApiResponse<FeedbackDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FeedbackDetailsDto>>> UpdateStatus(
        Guid id, [FromBody] UpdateFeedbackStatusRequest request)
    {
        var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var result = await _feedback.UpdateStatusAsync(
            id, adminUserId, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<FeedbackDetailsDto>.Ok(result, "تم تحديث حالة الملاحظة."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("metadata")]
    [ProducesResponseType(typeof(ApiResponse<FeedbackMetadataDto>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<FeedbackMetadataDto>> GetMetadata() =>
        Ok(ApiResponse<FeedbackMetadataDto>.Ok(_feedback.GetMetadata(), "تم استرجاع بيانات الملاحظات."));
}
