using System.Security.Claims;
using Domain.Entities;
using Shared.DTOs.Auth;

namespace ServicesAbstraction;

public interface ITokenService
{
    AccessTokenResult GenerateAccessToken(ApplicationUser user, IEnumerable<string>? roles = null);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
