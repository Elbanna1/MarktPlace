namespace Shared.DTOs.Auth;

public class RegisterRequest
{
    public string FirstName { get; set; } = default!;
    public string SecondName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;

    public string? ReferralCode { get; set; }
}
