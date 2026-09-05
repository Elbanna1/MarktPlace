using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs.Notifications;
using Shared.Responses;

using Shared.Constants;
namespace Presentation.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
[Produces("application/json")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly INotificationInterestService _interests;

    public NotificationsController(
        INotificationService notificationService, INotificationInterestService interests)
    {
        _notificationService = notificationService;
        _interests = interests;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResult<NotificationDto>>>> GetMine(
        [FromQuery] NotificationFilterParams filter)
    {
        var result = await _notificationService.GetMyNotificationsAsync(
            CurrentUserId, filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<NotificationDto>>.Ok(result, UserMessages.Notifications.Loaded));
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<ApiResponse<int>>> GetUnreadCount()
    {
        var count = await _notificationService.GetUnreadCountAsync(CurrentUserId, HttpContext.RequestAborted);
        return Ok(ApiResponse<int>.Ok(count, UserMessages.Notifications.UnreadCountLoaded));
    }

    [HttpPatch("{id:guid}/read")]
    public async Task<ActionResult<ApiResponse>> MarkAsRead(Guid id)
    {
        await _notificationService.MarkAsReadAsync(CurrentUserId, id);
        return Ok(ApiResponse.Ok(UserMessages.Notifications.MarkedRead));
    }

    [HttpPatch("read-all")]
    public async Task<ActionResult<ApiResponse<int>>> MarkAllAsRead()
    {
        var updated = await _notificationService.MarkAllAsReadAsync(CurrentUserId);
        return Ok(ApiResponse<int>.Ok(updated, UserMessages.Notifications.AllMarkedRead));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _notificationService.DeleteAsync(CurrentUserId, id);
        return Ok(ApiResponse.Ok(UserMessages.Notifications.Deleted));
    }

    [HttpDelete]
    public async Task<ActionResult<ApiResponse<int>>> DeleteAll()
    {
        var removed = await _notificationService.DeleteAllAsync(CurrentUserId);
        return Ok(ApiResponse<int>.Ok(removed, UserMessages.Notifications.AllDeleted));
    }

    [HttpGet("preferences")]
    public async Task<ActionResult<ApiResponse<NotificationPreferencesDto>>> GetPreferences()
    {
        var result = await _interests.GetPreferencesAsync(CurrentUserId, HttpContext.RequestAborted);
        return Ok(ApiResponse<NotificationPreferencesDto>.Ok(result, UserMessages.Notifications.PreferencesLoaded));
    }

    [HttpPut("preferences")]
    public async Task<ActionResult<ApiResponse<NotificationPreferencesDto>>> SetPreferences(
        [FromBody] UpdateNotificationPreferencesRequest request)
    {
        var result = await _interests.SetPreferencesAsync(
            CurrentUserId, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<NotificationPreferencesDto>.Ok(result, UserMessages.Notifications.PreferencesUpdated));
    }
}
