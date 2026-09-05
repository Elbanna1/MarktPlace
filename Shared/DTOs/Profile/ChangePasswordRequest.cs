namespace Shared.DTOs.Profile;

public class ChangePasswordRequest
{
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
