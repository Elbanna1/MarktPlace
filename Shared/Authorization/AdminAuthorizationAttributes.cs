using Shared.Enums;

namespace Shared.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class AdminPageAttribute : Attribute
{
    public AdminPageAttribute(string pageKey) => PageKey = pageKey;

    public string PageKey { get; }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class RequireAdminPermissionAttribute : Attribute
{
    public RequireAdminPermissionAttribute(AdminPermission permission) => Permission = permission;

    public RequireAdminPermissionAttribute(string pageKey, AdminPermission permission)
    {
        PageKey = pageKey;
        Permission = permission;
    }

    public AdminPermission Permission { get; }

    public string? PageKey { get; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class SuperAdminOnlyAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class AdminSelfServiceAttribute : Attribute
{
}
