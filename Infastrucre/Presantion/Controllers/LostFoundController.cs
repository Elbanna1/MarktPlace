using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.LostFound;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/lost-found")]
[Authorize]
[Produces("application/json")]
public class LostFoundController : ControllerBase
{
    private readonly ILostFoundService _lostFoundService;

    public LostFoundController(ILostFoundService lostFoundService)
    {
        _lostFoundService = lostFoundService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    private string? OptionalUserId =>
        User.Identity?.IsAuthenticated == true ? CurrentUserId : null;

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    public async Task<ActionResult<ApiResponse<LostFoundPostDetailsDto>>> Create(
        [FromForm] CreateLostFoundPostRequest request)
    {
        var images = await request.Images.ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _lostFoundService.CreateAsync(CurrentUserId, request, images);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<LostFoundPostDetailsDto>.Ok(result, UserMessages.Posts.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<LostFoundPostListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<LostFoundPostListItemDto>>>> GetFeed(
        [FromQuery] LostFoundFilterParams filter)
    {
        var result = await _lostFoundService.GetFeedAsync(
            filter, OptionalUserId, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<LostFoundPostListItemDto>>.Ok(result, UserMessages.Posts.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LostFoundPostDetailsDto>>> GetById(Guid id)
    {
        var viewerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _lostFoundService.GetByIdAsync(
            id, viewerUserId,
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            HttpContext.RequestAborted);

        return Ok(ApiResponse<LostFoundPostDetailsDto>.Ok(result, UserMessages.Posts.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    public async Task<ActionResult<ApiResponse<LostFoundPostDetailsDto>>> Update(
        Guid id, [FromForm] UpdateLostFoundPostRequest request)
    {
        var newImages = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _lostFoundService.UpdateAsync(CurrentUserId, id, request, newImages);
        return Ok(ApiResponse<LostFoundPostDetailsDto>.Ok(result, UserMessages.Posts.Updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _lostFoundService.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Posts.Deleted));
    }

    [HttpPatch("{id:guid}/returned")]
    public async Task<ActionResult<ApiResponse<LostFoundPostDetailsDto>>> MarkAsReturned(Guid id)
    {
        var result = await _lostFoundService.MarkAsReturnedAsync(CurrentUserId, id);
        return Ok(ApiResponse<LostFoundPostDetailsDto>.Ok(result, UserMessages.Posts.MarkedReturned));
    }

    [HttpPost("{id:guid}/like")]
    public async Task<ActionResult<ApiResponse<LikeResultDto>>> ToggleLike(Guid id)
    {
        var result = await _lostFoundService.ToggleLikeAsync(CurrentUserId, id);
        var message = result.Liked ? "Post liked." : "Like removed.";
        return Ok(ApiResponse<LikeResultDto>.Ok(result, message));
    }

    [HttpPost("{id:guid}/comments")]
    public async Task<ActionResult<ApiResponse<LostFoundCommentDto>>> AddComment(
        Guid id, [FromBody] CreateCommentRequest request)
    {
        var result = await _lostFoundService.AddCommentAsync(CurrentUserId, id, request);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<LostFoundCommentDto>.Ok(result, UserMessages.Comments.Added));
    }

    [HttpPut("{id:guid}/comments/{commentId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<LostFoundCommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LostFoundCommentDto>>> UpdateComment(
        Guid id, Guid commentId, [FromBody] CreateCommentRequest request)
    {
        var result = await _lostFoundService.UpdateCommentAsync(
            CurrentUserId, id, commentId, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<LostFoundCommentDto>.Ok(result, UserMessages.Comments.Updated));
    }

    [HttpDelete("{id:guid}/comments/{commentId:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> DeleteComment(Guid id, Guid commentId)
    {
        await _lostFoundService.DeleteCommentAsync(
            CurrentUserId, IsAdmin, id, commentId, HttpContext.RequestAborted);

        return Ok(ApiResponse.Ok(UserMessages.Comments.Deleted));
    }

    [HttpGet("{id:guid}/comments")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LostFoundCommentDto>>>> GetComments(Guid id)
    {
        var result = await _lostFoundService.GetCommentsAsync(
            id, OptionalUserId, IsAdmin,
            HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<LostFoundCommentDto>>.Ok(result, UserMessages.Comments.Loaded));
    }
}
