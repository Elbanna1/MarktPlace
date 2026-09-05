using Shared.DTOs.Admin;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IAdminPermissionService
{
    Task<AdminAccessResult> AuthorizeAsync(
        string? userId, string pageKey, AdminPermission permission,
        CancellationToken cancellationToken = default);

    Task<AdminAccessResult> AuthorizeSuperAdminAsync(
        string? userId, CancellationToken cancellationToken = default);

    Task<AdminAccessResult> AuthorizeAdminAsync(
        string? userId, CancellationToken cancellationToken = default);

    Task<AdminPermissionsDto> GetMyPermissionsAsync(
        string userId, CancellationToken cancellationToken = default);

    AdminPermissionCatalogDto GetCatalog();
}

public readonly record struct AdminAccessResult(bool Allowed, int StatusCode, string? Message)
{
    public static AdminAccessResult Allow() => new(true, 200, null);

    public static AdminAccessResult Unauthenticated() =>
        new(false, 401, "لازم تسجل دخولك عشان توصل للمحتوى ده.");

    public static AdminAccessResult Denied(string? message = null) =>
        new(false, 403, message ?? "مالكش صلاحية توصل للمحتوى ده.");
}
