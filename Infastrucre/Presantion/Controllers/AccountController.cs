using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Account;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/account")]
[Authorize]
[Produces("application/json")]
[EnableRateLimiting(RateLimitPolicies.Auth)]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpDelete]
    [ProducesResponseType(typeof(ApiResponse<DeactivatedAccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<DeactivatedAccountDto>>> Deactivate(
        [FromBody] DeactivateAccountRequest request)
    {
        var result = await _accountService.DeactivateAsync(
            CurrentUserId, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<DeactivatedAccountDto>.Ok(result, UserMessages.Account.Deactivated));
    }
}
