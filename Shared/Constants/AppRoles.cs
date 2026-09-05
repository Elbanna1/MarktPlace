namespace Shared.Constants;

public static class AppRoles
{
    public const string Admin = "Admin";

    public const string SuperAdmin = "SuperAdmin";

    public static IReadOnlyList<string> All { get; } = [Admin, SuperAdmin];
}
