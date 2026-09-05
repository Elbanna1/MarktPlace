namespace Shared.DTOs.Auth;

public class VerifyResetCodeRequest
{
    public string Otp { get; set; } = default!;
}
