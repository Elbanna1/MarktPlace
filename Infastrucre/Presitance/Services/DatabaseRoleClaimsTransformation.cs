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
    private readonly IUserAccessStateCache _cache;
    private readonly IHttpContextAccessor _accessor;

    public DatabaseRoleClaimsTransformation(
        AppDbContext db, IUserAccessStateCache cache, IHttpContextAccessor accessor)
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

        var state = await ResolveAsync(userId);

        Rewrite(identity, state);

        return principal;
    }

    private async Task<UserAccessState> ResolveAsync(string userId)
    {
        var administration = IsAdministrationRequest();

        if (!administration && _cache.TryGet(userId, out var cached))
            return cached;

        var cancellationToken = _accessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        var row = await _db.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new
            {
                user.Status,
                Roles = _db.UserRoles
                    .Where(link => link.UserId == user.Id)
                    .Join(_db.Roles, link => link.RoleId, role => role.Id, (_, role) => role.Name)
                    .Where(name => name != null)
                    .Select(name => name!)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        var state = row is null
            ? UserAccessState.Missing
            : new UserAccessState(true, row.Status, row.Roles);

        _cache.Set(userId, state);

        return state;
    }

    private bool IsAdministrationRequest()
    {
        var path = _accessor.HttpContext?.Request.Path;

        if (path is not { HasValue: true } value)
            return false;

        return value.StartsWithSegments(AdminRoutePrefix, StringComparison.OrdinalIgnoreCase) ||
               value.StartsWithSegments(LegacyAdminRoutePrefix, StringComparison.OrdinalIgnoreCase);
    }

    private static void Rewrite(ClaimsIdentity identity, UserAccessState state)
    {
        var stale = identity.Claims
            .Where(claim =>
                claim.Type == identity.RoleClaimType ||
                claim.Type == ClaimTypes.Role ||
                claim.Type == "role" ||
                claim.Type == "roles" ||
                claim.Type == AuthConstants.AccountStatusClaimType)
            .ToList();

        foreach (var claim in stale)
            identity.TryRemoveClaim(claim);

        identity.AddClaim(new Claim(
            AuthConstants.AccountStatusClaimType,
            state.Exists
                ? ((int)state.Status).ToString()
                : AuthConstants.AccountMissingClaimValue));

        foreach (var role in state.Roles)
            identity.AddClaim(new Claim(identity.RoleClaimType, role));
    }
}
