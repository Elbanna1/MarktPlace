using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Notifications;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/notifications")]
[Tags("Admin Notifications")]
[AdminSelfService]
public class AdminNotificationsController : AdminApiController
{
    private readonly INotificationService _notifications;

    public AdminNotificationsController(INotificationService notifications)
    {
        _notifications = notifications;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<NotificationDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<NotificationDto>>>> GetNotifications(
        [FromQuery] NotificationFilterParams filter)
    {
        var result = await _notifications.GetMyNotificationsAsync(CurrentAdminId, filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<NotificationDto>>.Ok(result, "تم استرجاع التنبيهات."));
    }

    [HttpGet("unread-count")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<int>>> GetUnreadCount()
    {
        var count = await _notifications.GetUnreadCountAsync(CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<int>.Ok(count, "تم استرجاع عدد التنبيهات غير المقروءة."));
    }

    [HttpPatch("{id:guid}/read")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> MarkAsRead(Guid id)
    {
        await _notifications.MarkAsReadAsync(CurrentAdminId, id);

        return Ok(ApiResponse.Ok("تم تعليم التنبيه كمقروء."));
    }

    [HttpPatch("read-all")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<int>>> MarkAllAsRead()
    {
        var updated = await _notifications.MarkAllAsReadAsync(CurrentAdminId);

        return Ok(ApiResponse<int>.Ok(updated, "تم تعليم جميع التنبيهات كمقروءة."));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _notifications.DeleteAsync(CurrentAdminId, id);

        return Ok(ApiResponse.Ok("تم حذف التنبيه."));
    }
}
