using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.Admin;

public class AdminPermissionService : IAdminPermissionService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _users;
    private readonly IAdminAccountRepository _repository;

    private (string UserId, AdminContext Context)? _cached;

    public AdminPermissionService(
        UserManager<ApplicationUser> userManager,
        IUserRepository users,
        IAdminAccountRepository repository)
    {
        _userManager = userManager;
        _users = users;
        _repository = repository;
    }

    public async Task<AdminAccessResult> AuthorizeAsync(
        string? userId, string pageKey, AdminPermission permission,
        CancellationToken cancellationToken = default)
    {
        var (result, context) = await ResolveAsync(userId, cancellationToken);

        if (context is null)
            return result;

        if (context.IsSuperAdmin)
            return AdminAccessResult.Allow();

        if (!AdminPageCatalog.Exists(pageKey))
            return AdminAccessResult.Denied();

        var mask = (AdminPermission)await _repository.GetPermissionMaskAsync(
            context.User.Id, pageKey, cancellationToken);

        if ((mask & permission) == permission)
            return AdminAccessResult.Allow();

        var message = mask == AdminPermission.None
            ? "مالكش صلاحية توصل للصفحة دي."
            : $"مالكش صلاحية {AdminPageCatalog.ArabicNameOf(permission)} على الصفحة دي.";

        return AdminAccessResult.Denied(message);
    }

    public async Task<AdminAccessResult> AuthorizeSuperAdminAsync(
        string? userId, CancellationToken cancellationToken = default)
    {
        var (result, context) = await ResolveAsync(userId, cancellationToken);

        if (context is null)
            return result;

        return context.IsSuperAdmin
            ? AdminAccessResult.Allow()
            : AdminAccessResult.Denied("العملية دي للمسؤول الأعلى بس.");
    }

    public async Task<AdminAccessResult> AuthorizeAdminAsync(
        string? userId, CancellationToken cancellationToken = default)
    {
        var (result, context) = await ResolveAsync(userId, cancellationToken);

        return context is null ? result : AdminAccessResult.Allow();
    }

    public async Task<AdminPermissionsDto> GetMyPermissionsAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("الحساب غير موجود.");

        var isSuperAdmin = (await _users.GetRoleNamesAsync(userId, cancellationToken))
            .Contains(AppRoles.SuperAdmin, StringComparer.OrdinalIgnoreCase);

        var pages = isSuperAdmin
            ? AdminPageCatalog.All
                .Select(page => Describe(page, page.Allowed))
                .ToList()
            : await BuildGrantedPagesAsync(user.Id, cancellationToken);

        return new AdminPermissionsDto
        {
            Admin = new AdminIdentityDto
            {
                Id = user.Id,
                Name = BuildName(user),
                UserName = user.UserName ?? string.Empty,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Image = user.ProfileImageUrl,
                IsActive = user.Status == UserAccountStatus.Active,
                IsSuperAdmin = isSuperAdmin
            },
            Pages = pages
        };
    }

    public AdminPermissionCatalogDto GetCatalog() => new()
    {
        Pages = AdminPageCatalog.All
            .Select(page => new AdminPageDefinitionDto
            {
                Key = page.Key,
                Name = page.Name,
                NameAr = page.NameAr,
                Route = page.Route,
                Icon = page.Icon,
                Group = page.Group,
                Permissions = page.Permissions.Select(Describe).ToList()
            })
            .ToList(),
        Permissions = AdminPageCatalog.AllPermissions.Select(Describe).ToList()
    };

    private async Task<(AdminAccessResult Result, AdminContext? Context)> ResolveAsync(
        string? userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return (AdminAccessResult.Unauthenticated(), null);

        if (_cached is { } cached && string.Equals(cached.UserId, userId, StringComparison.Ordinal))
            return (AdminAccessResult.Allow(), cached.Context);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return (AdminAccessResult.Unauthenticated(), null);

        var roles = await _users.GetRoleNamesAsync(userId, cancellationToken);

        if (!roles.Contains(AppRoles.Admin, StringComparer.OrdinalIgnoreCase))
            return (AdminAccessResult.Denied(), null);

        if (user.Status != UserAccountStatus.Active)
        {
            var reason = string.IsNullOrWhiteSpace(user.StatusReason)
                ? string.Empty
                : $" السبب: {user.StatusReason}";

            return (AdminAccessResult.Denied($"الحساب ده متوقف.{reason}".Trim()), null);
        }

        var context = new AdminContext(
            user, roles.Contains(AppRoles.SuperAdmin, StringComparer.OrdinalIgnoreCase));

        _cached = (userId, context);

        return (AdminAccessResult.Allow(), context);
    }

    private async Task<List<AdminGrantedPageDto>> BuildGrantedPagesAsync(
        string userId, CancellationToken cancellationToken)
    {
        var grants = await _repository.GetGrantsAsync(userId, cancellationToken);

        var byKey = grants.ToDictionary(
            grant => grant.PageKey,
            grant => grant.Permissions.Aggregate(
                AdminPermission.None, (mask, permission) => mask | (AdminPermission)permission.Permission),
            StringComparer.OrdinalIgnoreCase);

        return AdminPageCatalog.All
            .Where(page => byKey.TryGetValue(page.Key, out var mask) && mask != AdminPermission.None)
            .Select(page => Describe(page, byKey[page.Key] & page.Allowed))
            .Where(page => page.Permissions.Count > 0)
            .ToList();
    }

    private static AdminGrantedPageDto Describe(
        AdminPageCatalog.AdminPageDefinition page, AdminPermission mask) => new()
    {
        Key = page.Key,
        Name = page.Name,
        NameAr = page.NameAr,
        Route = page.Route,
        Icon = page.Icon,
        Group = page.Group,
        Permissions = AdminPageCatalog.Explode(mask).Select(permission => permission.ToString()).ToList()
    };

    private static AdminPermissionOptionDto Describe(AdminPermission permission) => new()
    {
        Key = permission.ToString(),
        Value = (int)permission,
        Name = permission.ToString(),
        NameAr = AdminPageCatalog.ArabicNameOf(permission)
    };

    private static string BuildName(ApplicationUser user) =>
        $"{user.FirstName} {user.SecondName}".Trim();

    private sealed record AdminContext(ApplicationUser User, bool IsSuperAdmin);
}
