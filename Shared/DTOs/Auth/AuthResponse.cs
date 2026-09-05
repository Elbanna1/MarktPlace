using Shared.DTOs.Profile;

namespace Shared.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public DateTime Expiration { get; set; }

    public DateTime RefreshTokenExpiration { get; set; }

    public UserDto User { get; set; } = default!;
}
