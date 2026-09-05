using Shared.DTOs.Auth;

namespace ServicesAbstraction;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);

    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);

    Task<PasswordResetSessionDto> ForgotPasswordAsync(ForgotPasswordRequest request);

    Task VerifyResetCodeAsync(string? resetToken, VerifyResetCodeRequest request);

    Task ResetPasswordAsync(string? resetToken, ResetPasswordRequest request);

    Task LogoutAsync(string userId);
}
