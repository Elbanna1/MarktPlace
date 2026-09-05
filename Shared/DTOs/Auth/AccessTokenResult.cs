namespace Shared.DTOs.Auth;

public record AccessTokenResult(string Token, DateTime ExpiresAt);
