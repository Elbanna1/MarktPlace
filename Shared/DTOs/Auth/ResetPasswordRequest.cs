namespace Shared.DTOs.Auth;

public class ResetPasswordRequest
{
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
