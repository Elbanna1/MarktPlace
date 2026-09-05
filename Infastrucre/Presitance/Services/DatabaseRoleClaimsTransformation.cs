using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;

namespace Persistence.Services;

public class DatabaseRoleClaimsTransformation : IClaimsTransformation
{
    private const string LegacyAdminRoutePrefix = "/api/admin";

    private static readonly string AdminRoutePrefix = "/" + ApiVersions.AdminRoutePrefix;

    private readonly AppDbContext _db;
    private readonly IUserRoleCache _cache;
    private readonly IHttpContextAccessor _accessor;

    public DatabaseRoleClaimsTransformation(
        AppDbContext db, IUserRoleCache cache, IHttpContextAccessor accessor)
    {
        _db = db;
        _cache = cache;
        _accessor = accessor;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not { IsAuthenticated: true })
            return principal;

        var identity = principal.Identities.FirstOrDefault(candidate => candidate.IsAuthenticated);

        if (identity is null)
            return principal;

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return principal;

        var roles = await ResolveRolesAsync(userId);

        Rewrite(identity, roles);

        return principal;
    }

    private async Task<IReadOnlyList<string>> ResolveRolesAsync(string userId)
    {
        var administration = IsAdministrationRequest();

        if (!administration && _cache.TryGet(userId, out var cached))
            return cached;

        var roles = await _db.UserRoles
            .Where(link => link.UserId == userId)
            .Join(_db.Roles, link => link.RoleId, role => role.Id, (_, role) => role.Name)
            .Where(name => name != null)
            .Select(name => name!)
            .ToListAsync(_accessor.HttpContext?.RequestAborted ?? CancellationToken.None);

        _cache.Set(userId, roles);

        return roles;
    }

    private bool IsAdministrationRequest()
    {
        var path = _accessor.HttpContext?.Request.Path;

        if (path is not { HasValue: true } value)
            return false;

        return value.StartsWithSegments(AdminRoutePrefix, StringComparison.OrdinalIgnoreCase) ||
               value.StartsWithSegments(LegacyAdminRoutePrefix, StringComparison.OrdinalIgnoreCase);
    }

    private static void Rewrite(ClaimsIdentity identity, IReadOnlyList<string> roles)
    {
        var stale = identity.Claims
            .Where(claim =>
                claim.Type == identity.RoleClaimType ||
                claim.Type == ClaimTypes.Role ||
                claim.Type == "role" ||
                claim.Type == "roles")
            .ToList();

        foreach (var claim in stale)
            identity.TryRemoveClaim(claim);

        foreach (var role in roles)
            identity.AddClaim(new Claim(identity.RoleClaimType, role));
    }
}
