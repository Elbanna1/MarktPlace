using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Charity;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
public abstract class CharityControllerBase : ControllerBase
{
    protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    protected string? OptionalUserId =>
        User.Identity?.IsAuthenticated == true ? CurrentUserId : null;

    protected bool IsAdmin => User.IsInRole(AppRoles.Admin);
}

[Route("api/rescues")]
[Tags("Charity - Rescues")]
public class RescuesController : CharityControllerBase
{
    private readonly IRescueService _service;

    public RescuesController(IRescueService service)
    {
        _service = service;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<RescueDetailsDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<RescueDetailsDto>>> Create(
        [FromForm] CreateRescueRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.CreateAsync(CurrentUserId, request, images, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<RescueDetailsDto>.Ok(result, "تم إرسال الاستغاثة وهي الآن قيد المراجعة."));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<RescueListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<RescueListItemDto>>>> GetList(
        [FromQuery] RescueFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<RescueListItemDto>>.Ok(result, "تم استرجاع الاستغاثات."));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<RescueDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<RescueDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, HttpContext.RequestAborted);

        return Ok(ApiResponse<RescueDetailsDto>.Ok(result, "تم استرجاع بيانات الاستغاثة."));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<RescueDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RescueDetailsDto>>> Update(
        Guid id, [FromForm] UpdateRescueRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, images, HttpContext.RequestAborted);

        return Ok(ApiResponse<RescueDetailsDto>.Ok(result, "تم تحديث الاستغاثة."));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin, HttpContext.RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف الاستغاثة."));
    }
}

[Route("api/blood-requests")]
[Tags("Charity - Blood Requests")]
public class BloodRequestsController : CharityControllerBase
{
    private readonly IBloodRequestService _service;

    public BloodRequestsController(IBloodRequestService service)
    {
        _service = service;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<BloodRequestDetailsDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<BloodRequestDetailsDto>>> Create(
        [FromForm] CreateBloodRequestRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.CreateAsync(CurrentUserId, request, images, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<BloodRequestDetailsDto>.Ok(result, "تم إرسال طلب فصيلة الدم وهو الآن قيد المراجعة."));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<BloodRequestListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<BloodRequestListItemDto>>>> GetList(
        [FromQuery] BloodRequestFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<BloodRequestListItemDto>>.Ok(result, "تم استرجاع طلبات فصائل الدم."));
    }

    [HttpGet("blood-groups")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<object>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<object>>> GetBloodGroups()
    {
        var groups = CharityCatalog.BloodGroups
            .Select(entry => (object)new { id = (int)entry.Id, name = entry.Name, nameEn = entry.NameEn })
            .ToList();

        return Ok(ApiResponse<IReadOnlyList<object>>.Ok(groups, "تم استرجاع فصائل الدم."));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<BloodRequestDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BloodRequestDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, HttpContext.RequestAborted);

        return Ok(ApiResponse<BloodRequestDetailsDto>.Ok(result, "تم استرجاع بيانات الطلب."));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<BloodRequestDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BloodRequestDetailsDto>>> Update(
        Guid id, [FromForm] UpdateBloodRequestRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, images, HttpContext.RequestAborted);

        return Ok(ApiResponse<BloodRequestDetailsDto>.Ok(result, "تم تحديث الطلب."));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin, HttpContext.RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف الطلب."));
    }
}

[Route("api/ask-consults")]
[Tags("Charity - Ask & Consult")]
public class AskConsultsController : CharityControllerBase
{
    private readonly IAskConsultService _service;

    public AskConsultsController(IAskConsultService service)
    {
        _service = service;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<AskConsultDetailsDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AskConsultDetailsDto>>> Create(
        [FromForm] CreateAskConsultRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.CreateAsync(CurrentUserId, request, images, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<AskConsultDetailsDto>.Ok(result, "تم إرسال السؤال وهو الآن قيد المراجعة."));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AskConsultListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AskConsultListItemDto>>>> GetList(
        [FromQuery] AskConsultFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, OptionalUserId, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AskConsultListItemDto>>.Ok(result, "تم استرجاع الأسئلة."));
    }

    [HttpGet("categories")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<object>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<object>>> GetCategories()
    {
        var categories = CharityCatalog.AskConsultCategories
            .Select(entry => (object)new { id = (int)entry.Id, name = entry.Name, nameEn = entry.NameEn })
            .ToList();

        return Ok(ApiResponse<IReadOnlyList<object>>.Ok(categories, "تم استرجاع مجالات الأسئلة."));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AskConsultDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AskConsultDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, OptionalUserId, HttpContext.RequestAborted);

        return Ok(ApiResponse<AskConsultDetailsDto>.Ok(result, "تم استرجاع بيانات السؤال."));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<AskConsultDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AskConsultDetailsDto>>> Update(
        Guid id, [FromForm] UpdateAskConsultRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, images, HttpContext.RequestAborted);

        return Ok(ApiResponse<AskConsultDetailsDto>.Ok(result, "تم تحديث السؤال."));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin, HttpContext.RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف السؤال."));
    }

    [HttpPost("{id:guid}/like")]
    [ProducesResponseType(typeof(ApiResponse<AskConsultLikeResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AskConsultLikeResultDto>>> ToggleLike(Guid id)
    {
        var result = await _service.ToggleLikeAsync(CurrentUserId, id, HttpContext.RequestAborted);

        return Ok(ApiResponse<AskConsultLikeResultDto>.Ok(
            result, result.Liked ? "تم تسجيل الإعجاب." : "تم إلغاء الإعجاب."));
    }

    [HttpPost("{id:guid}/comments")]
    [ProducesResponseType(typeof(ApiResponse<AskConsultCommentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AskConsultCommentDto>>> AddComment(
        Guid id, [FromBody] CreateAskConsultCommentRequest request)
    {
        var result = await _service.AddCommentAsync(CurrentUserId, id, request, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<AskConsultCommentDto>.Ok(result, "تم إضافة التعليق."));
    }

    [HttpPut("{id:guid}/comments/{commentId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AskConsultCommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AskConsultCommentDto>>> UpdateComment(
        Guid id, Guid commentId, [FromBody] CreateAskConsultCommentRequest request)
    {
        var result = await _service.UpdateCommentAsync(
            CurrentUserId, id, commentId, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<AskConsultCommentDto>.Ok(result, "تم تعديل التعليق."));
    }

    [HttpGet("{id:guid}/comments")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AskConsultCommentDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AskConsultCommentDto>>>> GetComments(
        Guid id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _service.GetCommentsAsync(
            id, pageIndex < 1 ? 1 : pageIndex, pageSize is < 1 or > 50 ? 20 : pageSize,
            User.Identity?.IsAuthenticated == true ? CurrentUserId : null, IsAdmin,
            HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AskConsultCommentDto>>.Ok(result, "تم استرجاع التعليقات."));
    }

    [HttpDelete("{id:guid}/comments/{commentId:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> DeleteComment(Guid id, Guid commentId)
    {
        await _service.DeleteCommentAsync(
            CurrentUserId, IsAdmin, id, commentId, HttpContext.RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف التعليق."));
    }
}
