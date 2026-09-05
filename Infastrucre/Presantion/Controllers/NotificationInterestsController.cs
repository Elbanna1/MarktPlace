using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs.Notifications;
using Shared.Responses;

using Shared.Constants;
namespace Presentation.Controllers;

[ApiController]
[Route("api/notifications/interests")]
[Authorize]
[Produces("application/json")]
public class NotificationInterestsController : ControllerBase
{
    private readonly INotificationInterestService _interests;

    public NotificationInterestsController(INotificationInterestService interests)
    {
        _interests = interests;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<NotificationInterestsDto>>> GetMine()
    {
        var result = await _interests.GetMineAsync(CurrentUserId, HttpContext.RequestAborted);
        return Ok(ApiResponse<NotificationInterestsDto>.Ok(result, UserMessages.Notifications.InterestsLoaded));
    }

    [HttpGet("options")]
    public async Task<ActionResult<ApiResponse<NotificationInterestOptionsDto>>> GetOptions()
    {
        var result = await _interests.GetOptionsAsync(CurrentUserId, HttpContext.RequestAborted);
        return Ok(ApiResponse<NotificationInterestOptionsDto>.Ok(result, UserMessages.Notifications.InterestOptionsLoaded));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<NotificationInterestDto>>> Add(
        [FromBody] AddNotificationInterestRequest request)
    {
        var result = await _interests.AddAsync(CurrentUserId, request, HttpContext.RequestAborted);
        return Ok(ApiResponse<NotificationInterestDto>.Ok(result, UserMessages.Notifications.InterestSaved));
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<NotificationInterestsDto>>> Replace(
        [FromBody] ReplaceNotificationInterestsRequest request)
    {
        var result = await _interests.ReplaceAsync(CurrentUserId, request, HttpContext.RequestAborted);
        return Ok(ApiResponse<NotificationInterestsDto>.Ok(result, UserMessages.Notifications.InterestsUpdated));
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<ApiResponse<NotificationInterestDto>>> SetEnabled(
        Guid id, [FromBody] UpdateNotificationInterestRequest request)
    {
        var result = await _interests.SetEnabledAsync(
            CurrentUserId, id, request.IsEnabled, HttpContext.RequestAborted);

        return Ok(ApiResponse<NotificationInterestDto>.Ok(result, UserMessages.Notifications.InterestUpdated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> Remove(Guid id)
    {
        await _interests.RemoveAsync(CurrentUserId, id, HttpContext.RequestAborted);
        return Ok(ApiResponse.Ok(UserMessages.Notifications.InterestRemoved));
    }
}
