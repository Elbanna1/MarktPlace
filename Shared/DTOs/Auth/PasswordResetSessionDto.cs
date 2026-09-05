namespace Shared.DTOs.Auth;

public class PasswordResetSessionDto
{
    public string ResetToken { get; set; } = default!;
}
