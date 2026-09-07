using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Auth;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
[EnableRateLimiting(RateLimitPolicies.Auth)]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<AuthResponse>.Ok(result, UserMessages.Auth.Registered));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponse<AuthResponse>.Ok(result, UserMessages.Auth.LoggedIn));
    }

    [HttpPost("google")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> GoogleSignIn(
        [FromBody] GoogleSignInRequest request)
    {
        var result = await _authService.GoogleSignInAsync(request, HttpContext.RequestAborted);

        return result.AccountCreated
            ? StatusCode(StatusCodes.Status201Created,
                ApiResponse<AuthResponse>.Ok(result.Auth, UserMessages.Auth.GoogleRegistered))
            : Ok(ApiResponse<AuthResponse>.Ok(result.Auth, UserMessages.Auth.GoogleSignedIn));
    }

    [HttpGet("google/config")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<GoogleAuthConfigDto>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<GoogleAuthConfigDto>> GoogleConfig() =>
        Ok(ApiResponse<GoogleAuthConfigDto>.Ok(
            _authService.GetGoogleConfig(), UserMessages.Auth.GoogleConfigLoaded));

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return Ok(ApiResponse<AuthResponse>.Ok(result, UserMessages.Auth.TokenRefreshed));
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PasswordResetSessionDto>>> ForgotPassword(
        [FromBody] ForgotPasswordRequest request)
    {
        var result = await _authService.ForgotPasswordAsync(request);
        return Ok(ApiResponse<PasswordResetSessionDto>.Ok(
            result, UserMessages.Auth.PasswordResetCodeSent));
    }

    [HttpPost("verify-reset-code")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse>> VerifyResetCode([FromBody] VerifyResetCodeRequest request)
    {
        await _authService.VerifyResetCodeAsync(ResetSessionToken, request);
        return Ok(ApiResponse.Ok(UserMessages.Auth.PasswordResetCodeValid));
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        await _authService.ResetPasswordAsync(ResetSessionToken, request);
        return Ok(ApiResponse.Ok(UserMessages.Auth.PasswordReset));
    }

    private string? ResetSessionToken =>
        Request.Headers[AuthConstants.PasswordResetTokenHeader].ToString();

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _authService.LogoutAsync(userId);
        return Ok(ApiResponse.Ok(UserMessages.Auth.LoggedOut));
    }
}
