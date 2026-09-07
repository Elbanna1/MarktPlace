namespace Shared.DTOs.Auth;

public class GoogleSignInRequest
{
    public string? IdToken { get; set; }

    public string? Code { get; set; }

    public string? RedirectUri { get; set; }

    public string? ReferralCode { get; set; }

    public string? Center { get; set; }
}
