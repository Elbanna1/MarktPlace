using Shared.Enums;

namespace Shared.Constants;

public static class AdminPageCatalog
{
    public static class Keys
    {
        public const string Dashboard = "dashboard";
        public const string Ads = "ads";
        public const string Users = "users";
        public const string Categories = "categories";
        public const string Locations = "locations";
        public const string Banners = "banners";
        public const string BannerRequests = "banner-requests";
        public const string Payments = "payments";
        public const string Reports = "reports";
        public const string Feedback = "feedback";
        public const string Referrals = "referrals";
        public const string Forms = "forms";
        public const string Home = "home";
        public const string Settings = "settings";
        public const string AuditLogs = "audit-logs";
    }

    public sealed record AdminPageDefinition(
        string Key,
        string Name,
        string NameAr,
        string Route,
        string Icon,
        string Group,
        IReadOnlyList<AdminPermission> Permissions)
    {
        public AdminPermission Allowed { get; } =
            Permissions.Aggregate(AdminPermission.None, (mask, permission) => mask | permission);
    }

    private static readonly AdminPermission[] ReadOnly = [AdminPermission.View];

    private static readonly AdminPermission[] Crud =
    [
        AdminPermission.View, AdminPermission.Create, AdminPermission.Edit, AdminPermission.Delete
    ];

    private static readonly AdminPermission[] CrudManage =
    [
        AdminPermission.View, AdminPermission.Create, AdminPermission.Edit,
        AdminPermission.Delete, AdminPermission.Manage
    ];

    public static IReadOnlyList<AdminPageDefinition> All { get; } =
    [
        new(Keys.Dashboard, "Dashboard", "لوحة التحكم", "/dashboard", "layout-dashboard", "عام", ReadOnly),

        new(Keys.Ads, "Advertisements", "الإعلانات", "/ads", "megaphone", "المحتوى",
        [
            AdminPermission.View, AdminPermission.Delete,
            AdminPermission.Approve, AdminPermission.Reject, AdminPermission.Manage
        ]),

        new(Keys.Users, "Users", "المستخدمون", "/users", "users", "المحتوى",
        [
            AdminPermission.View, AdminPermission.Manage
        ]),

        new(Keys.Categories, "Categories", "الأقسام", "/categories", "layers", "المحتوى", CrudManage),

        new(Keys.Locations, "Locations", "المواقع", "/locations", "map-pin", "المحتوى", Crud),

        new(Keys.Banners, "Banners", "البانرات", "/banners", "image", "الإعلانات المدفوعة", Crud),

        new(Keys.BannerRequests, "Banner Requests", "طلبات البانرات", "/banner-requests", "calendar-check",
            "الإعلانات المدفوعة",
        [
            AdminPermission.View, AdminPermission.Delete,
            AdminPermission.Approve, AdminPermission.Reject, AdminPermission.Manage
        ]),

        new(Keys.Payments, "Payments", "المدفوعات", "/payments", "credit-card", "الإعلانات المدفوعة",
        [
            AdminPermission.View, AdminPermission.Create, AdminPermission.Edit, AdminPermission.Delete,
            AdminPermission.Approve, AdminPermission.Reject, AdminPermission.Manage
        ]),

        new(Keys.Reports, "Reports", "البلاغات", "/reports", "flag", "المراجعة",
        [
            AdminPermission.View, AdminPermission.Manage
        ]),

        new(Keys.Feedback, "Ratings", "التقييمات", "/feedback", "star", "المراجعة",
        [
            AdminPermission.View, AdminPermission.Delete
        ]),

        new(Keys.Referrals, "Referrals", "الدعوات", "/referrals", "share-2", "المراجعة", ReadOnly),

        new(Keys.Forms, "Form Builder", "نماذج الإعلانات", "/forms", "form-input", "الإعدادات", Crud),

        new(Keys.Home, "Home Page", "الصفحة الرئيسية", "/home", "home", "الإعدادات",
        [
            AdminPermission.View, AdminPermission.Edit, AdminPermission.Manage
        ]),

        new(Keys.Settings, "Settings", "الإعدادات", "/settings", "settings", "الإعدادات",
        [
            AdminPermission.View, AdminPermission.Edit, AdminPermission.Manage
        ]),

        new(Keys.AuditLogs, "Audit Log", "سجل الإجراءات", "/audit-logs", "scroll-text", "الإعدادات", ReadOnly)
    ];

    private static readonly IReadOnlyDictionary<string, AdminPageDefinition> ByKey =
        All.ToDictionary(page => page.Key, StringComparer.OrdinalIgnoreCase);

    public static AdminPageDefinition? Find(string? key) =>
        key is not null && ByKey.TryGetValue(key, out var page) ? page : null;

    public static bool Exists(string? key) => Find(key) is not null;

    public static bool Allows(string? key, AdminPermission permission) =>
        permission != AdminPermission.None &&
        Find(key) is { } page &&
        (page.Allowed & permission) == permission;

    public static readonly IReadOnlyDictionary<AdminPermission, string> PermissionNames =
        new Dictionary<AdminPermission, string>
        {
            [AdminPermission.View] = "عرض",
            [AdminPermission.Create] = "إضافة",
            [AdminPermission.Edit] = "تعديل",
            [AdminPermission.Delete] = "حذف",
            [AdminPermission.Approve] = "قبول",
            [AdminPermission.Reject] = "رفض",
            [AdminPermission.Manage] = "إدارة"
        };

    public static string ArabicNameOf(AdminPermission permission) =>
        PermissionNames.TryGetValue(permission, out var name) ? name : permission.ToString();

    public static IReadOnlyList<AdminPermission> AllPermissions { get; } =
    [
        AdminPermission.View, AdminPermission.Create, AdminPermission.Edit, AdminPermission.Delete,
        AdminPermission.Approve, AdminPermission.Reject, AdminPermission.Manage
    ];

    public static IReadOnlyList<AdminPermission> Explode(AdminPermission mask) =>
        AllPermissions.Where(permission => (mask & permission) == permission).ToList();
}
