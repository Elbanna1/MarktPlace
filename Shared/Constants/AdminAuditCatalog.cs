using Shared.Enums;

namespace Shared.Constants;

public static class AdminAuditCatalog
{
    public const int MaxDescriptionLength = 1000;

    public const int MaxValueLength = 4000;

    public static class Targets
    {
        public const string Advertisement = "Advertisement";
        public const string BannerBooking = "BannerBooking";
        public const string BannerPlacement = "BannerPlacement";
        public const string Payment = "Payment";
        public const string PaymentMethod = "PaymentMethod";
        public const string Category = "Category";
        public const string SubCategory = "SubCategory";
        public const string User = "User";
        public const string PlatformSettings = "PlatformSettings";
        public const string HomeSection = "HomeSection";
        public const string Governorate = "Governorate";
        public const string Center = "Center";
        public const string Project = "Project";
        public const string Report = "Report";
        public const string FormField = "FormField";

        public const string Admin = "Admin";
    }

    public static readonly IReadOnlyDictionary<string, string> TargetNames =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [Targets.Advertisement] = "إعلان",
            [Targets.BannerBooking] = "حجز بانر",
            [Targets.BannerPlacement] = "مكان إعلاني",
            [Targets.Payment] = "عملية دفع",
            [Targets.PaymentMethod] = "وسيلة دفع",
            [Targets.Category] = "قسم",
            [Targets.SubCategory] = "قسم فرعي",
            [Targets.User] = "مستخدم",
            [Targets.PlatformSettings] = "إعدادات المنصة",
            [Targets.HomeSection] = "قسم في الصفحة الرئيسية",
            [Targets.Governorate] = "محافظة",
            [Targets.Center] = "مركز",
            [Targets.Project] = "مشروع",
            [Targets.Report] = "بلاغ",
            [Targets.FormField] = "حقل في نموذج",
            [Targets.Admin] = "مسؤول"
        };

    public static readonly IReadOnlyDictionary<AdminAuditAction, string> ActionNames =
        new Dictionary<AdminAuditAction, string>
        {
            [AdminAuditAction.ApproveAd] = "قبول إعلان",
            [AdminAuditAction.RejectAd] = "رفض إعلان",
            [AdminAuditAction.SuspendAd] = "إيقاف إعلان",
            [AdminAuditAction.DeleteAd] = "حذف إعلان",

            [AdminAuditAction.ApproveBannerPayment] = "تأكيد دفع بانر",
            [AdminAuditAction.RejectBannerPayment] = "رفض دفع بانر",
            [AdminAuditAction.ApproveBanner] = "الموافقة على بانر",
            [AdminAuditAction.RejectBanner] = "رفض بانر",
            [AdminAuditAction.ExpireBanner] = "إنهاء بانر",
            [AdminAuditAction.DeleteBanner] = "حذف بانر",
            [AdminAuditAction.UpdateBannerPrice] = "تعديل سعر بانر",

            [AdminAuditAction.ApprovePayment] = "تأكيد عملية دفع",
            [AdminAuditAction.RejectPayment] = "رفض عملية دفع",
            [AdminAuditAction.RefundPayment] = "استرداد عملية دفع",
            [AdminAuditAction.CreatePaymentMethod] = "إضافة وسيلة دفع",
            [AdminAuditAction.UpdatePaymentMethod] = "تعديل وسيلة دفع",
            [AdminAuditAction.DeletePaymentMethod] = "حذف وسيلة دفع",

            [AdminAuditAction.CreateCategory] = "إضافة قسم",
            [AdminAuditAction.UpdateCategory] = "تعديل قسم",
            [AdminAuditAction.DeleteCategory] = "حذف قسم",
            [AdminAuditAction.SetCategoryStatus] = "تغيير حالة قسم",
            [AdminAuditAction.CreateSubCategory] = "إضافة قسم فرعي",
            [AdminAuditAction.UpdateSubCategory] = "تعديل قسم فرعي",
            [AdminAuditAction.DeleteSubCategory] = "حذف قسم فرعي",
            [AdminAuditAction.SetSubCategoryStatus] = "تغيير حالة قسم فرعي",

            [AdminAuditAction.ActivateUser] = "تفعيل مستخدم",
            [AdminAuditAction.SuspendUser] = "إيقاف مستخدم",
            [AdminAuditAction.BlockUser] = "حظر مستخدم",

            [AdminAuditAction.UpdateSettings] = "تعديل إعدادات المنصة",
            [AdminAuditAction.UpdateLogo] = "تعديل شعار المنصة",
            [AdminAuditAction.UpdateHomeSection] = "تعديل قسم في الصفحة الرئيسية",
            [AdminAuditAction.ReorderHomeSections] = "تغيير ترتيب الصفحة الرئيسية",

            [AdminAuditAction.CreateGovernorate] = "إضافة محافظة",
            [AdminAuditAction.UpdateGovernorate] = "تعديل محافظة",
            [AdminAuditAction.DeleteGovernorate] = "حذف محافظة",
            [AdminAuditAction.CreateCenter] = "إضافة مركز",
            [AdminAuditAction.UpdateCenter] = "تعديل مركز",
            [AdminAuditAction.DeleteCenter] = "حذف مركز",
            [AdminAuditAction.CreateProject] = "إضافة مشروع",
            [AdminAuditAction.UpdateProject] = "تعديل مشروع",
            [AdminAuditAction.DeleteProject] = "حذف مشروع",

            [AdminAuditAction.DismissReport] = "تجاهل بلاغ",
            [AdminAuditAction.ResolveReport] = "اتخاذ إجراء على بلاغ",

            [AdminAuditAction.CreateFormField] = "إضافة حقل في نموذج",
            [AdminAuditAction.UpdateFormField] = "تعديل حقل في نموذج",
            [AdminAuditAction.DeleteFormField] = "حذف حقل من نموذج",

            [AdminAuditAction.CreateAdmin] = "إضافة مسؤول",
            [AdminAuditAction.UpdateAdmin] = "تعديل مسؤول",
            [AdminAuditAction.ActivateAdmin] = "تفعيل مسؤول",
            [AdminAuditAction.DeactivateAdmin] = "إيقاف مسؤول",
            [AdminAuditAction.UpdateAdminPermissions] = "تعديل صلاحيات مسؤول",
            [AdminAuditAction.RevokeAdmin] = "سحب صلاحيات مسؤول"
        };

    public static string NameOf(AdminAuditAction action) =>
        ActionNames.TryGetValue(action, out var name) ? name : action.ToString();

    public static string NameOfTarget(string? targetType) =>
        targetType is not null && TargetNames.TryGetValue(targetType, out var name)
            ? name
            : targetType ?? string.Empty;

    public static IReadOnlyList<AdminAuditAction> AllActions { get; } =
        Enum.GetValues<AdminAuditAction>();

    public static IReadOnlyList<string> AllTargets { get; } = TargetNames.Keys.ToList();
}
