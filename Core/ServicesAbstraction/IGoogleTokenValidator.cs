using Shared.DTOs.Auth;

namespace ServicesAbstraction;

public interface IGoogleTokenValidator
{
    bool IsConfigured { get; }

    string? ClientId { get; }

    Task<GoogleUserProfile> ValidateAsync(
        GoogleSignInRequest request, CancellationToken cancellationToken = default);
}

public sealed record GoogleUserProfile(
    string Subject,
    string? Email,
    bool EmailVerified,
    string? Name,
    string? GivenName,
    string? FamilyName,
    string? PictureUrl);
