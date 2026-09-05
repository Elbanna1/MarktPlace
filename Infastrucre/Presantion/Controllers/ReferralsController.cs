using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs.Referrals;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/referrals")]
[Produces("application/json")]
public class ReferralsController : ControllerBase
{
    private readonly IReferralService _referrals;

    public ReferralsController(IReferralService referrals)
    {
        _referrals = referrals;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<MyReferralDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<MyReferralDto>>> GetMine()
    {
        var result = await _referrals.GetMineAsync(CurrentUserId, HttpContext.RequestAborted);

        return Ok(ApiResponse<MyReferralDto>.Ok(result, "تم استرجاع بيانات الدعوة."));
    }

    [HttpPost("me/share")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<MyReferralDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<MyReferralDto>>> RecordShare()
    {
        var result = await _referrals.RecordShareAsync(CurrentUserId, HttpContext.RequestAborted);

        return Ok(ApiResponse<MyReferralDto>.Ok(result, "تم تسجيل مشاركة الرابط."));
    }

    [HttpGet("me/statistics")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<ReferralStatisticsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<ReferralStatisticsDto>>> GetMyStatistics()
    {
        var result = await _referrals.GetMyStatisticsAsync(CurrentUserId, HttpContext.RequestAborted);

        return Ok(ApiResponse<ReferralStatisticsDto>.Ok(result, "تم استرجاع إحصائيات الدعوات."));
    }

    [HttpGet("me/users")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ReferredUserDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ReferredUserDto>>>> GetMyReferredUsers(
        [FromQuery] MyReferralFilterParams filter)
    {
        var result = await _referrals.GetMyReferredUsersAsync(
            CurrentUserId, filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ReferredUserDto>>.Ok(result, "تم استرجاع قائمة الدعوات."));
    }

    [HttpGet("resolve/{code}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ReferralResolutionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ReferralResolutionDto>>> Resolve(string code)
    {
        var callerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _referrals.ResolveAsync(code, callerId, HttpContext.RequestAborted);

        return Ok(ApiResponse<ReferralResolutionDto>.Ok(
            result, result.Valid ? "رابط الدعوة صالح." : result.Message ?? "رابط الدعوة غير صالح."));
    }
}
