using Shared.Enums;

namespace Shared.Constants;

public static class BannerBookingCatalog
{
    public const int DefaultDurationDays = 30;

    public const int MinDurationDays = 1;

    public const int MaxDurationDays = 365;

    public static string FormatDuration(int days) => $"{days} يوم";

    public static int NormalizeDuration(int days) =>
        days < MinDurationDays ? DefaultDurationDays :
        days > MaxDurationDays ? MaxDurationDays : days;

    public const int MaxTitleLength = 200;
    public const int MaxDescriptionLength = 1000;
    public const int MaxButtonTextLength = 50;
    public const int MaxTargetUrlLength = 2000;
    public const int MaxAdvertiserNameLength = 150;
    public const int MaxPhoneLength = 20;
    public const int MaxEmailLength = 256;
    public const int MaxRejectionNotesLength = 1000;

    public const decimal MinPrice = 0m;

    public const decimal MaxPrice = 1_000_000m;

    public const string DefaultCurrency = "EGP";

    public const int MinSlots = 1;

    public const int MaxSlots = 20;

    public const int MinImageEdge = 50;

    public const int MaxImageEdge = 10_000;

    public const double AspectRatioTolerance = 0.02;

    public static readonly string[] DefaultAllowedFormats = [".jpg", ".jpeg", ".png", ".webp", ".gif"];

    public const string AllowedFormatNames = "JPG, JPEG, PNG, WebP, GIF";

    public static readonly IReadOnlyList<ImageFormatDescriptor> SupportedFormats =
        [ImageFormatCatalog.Jpeg, ImageFormatCatalog.Png, ImageFormatCatalog.Webp, ImageFormatCatalog.Gif];

    public const long DefaultMaxImageSizeBytes = ImageConstants.MaxFileSizeBytes;

    public const long MinConfigurableImageSizeBytes = 1 * 1024 * 1024;

    public const long MaxConfigurableImageSizeBytes = ImageConstants.MaxFileSizeBytes;

    public static readonly IReadOnlyDictionary<BannerLocation, string> LocationNames =
        new Dictionary<BannerLocation, string>
        {
            [BannerLocation.HomeSlider1] = "السلايدر الرئيسي الأول",
            [BannerLocation.HomeSlider2] = "السلايدر الرئيسي الثاني",
            [BannerLocation.SubCategoryBanner] = "بانر القسم الفرعي"
        };

    public static readonly IReadOnlyDictionary<BannerBookingStatus, string> StatusNames =
        new Dictionary<BannerBookingStatus, string>
        {
            [BannerBookingStatus.PendingReview] = "قيد المراجعة",
            [BannerBookingStatus.PaymentApproved] = "تم تأكيد الدفع",
            [BannerBookingStatus.Approved] = "تمت الموافقة",
            [BannerBookingStatus.Published] = "منشور",
            [BannerBookingStatus.Rejected] = "مرفوض",
            [BannerBookingStatus.Expired] = "منتهي",
            [BannerBookingStatus.Cancelled] = "ملغي"
        };

    public static readonly IReadOnlyDictionary<BannerPaymentStatus, string> PaymentStatusNames =
        new Dictionary<BannerPaymentStatus, string>
        {
            [BannerPaymentStatus.Pending] = "قيد المراجعة",
            [BannerPaymentStatus.Paid] = "مدفوع",
            [BannerPaymentStatus.Rejected] = "مرفوض"
        };

    public static readonly IReadOnlyDictionary<BannerRejectionReason, string> RejectionReasonNames =
        new Dictionary<BannerRejectionReason, string>
        {
            [BannerRejectionReason.UnsuitableImage] = "صورة غير مناسبة",
            [BannerRejectionReason.UnclearPaymentProof] = "إثبات دفع غير واضح",
            [BannerRejectionReason.InvalidUrl] = "الرابط غير صحيح",
            [BannerRejectionReason.MissingInformation] = "بيانات ناقصة",
            [BannerRejectionReason.PolicyViolation] = "محتوى الإعلان مخالف",
            [BannerRejectionReason.Other] = "سبب آخر"
        };

    public static readonly IReadOnlyDictionary<BannerImageKind, string> ImageKindNames =
        new Dictionary<BannerImageKind, string>
        {
            [BannerImageKind.Desktop] = "صورة البانر — Desktop",
            [BannerImageKind.Mobile] = "صورة البانر — Mobile"
        };

    public static string GetLocationName(BannerLocation location) =>
        LocationNames.TryGetValue(location, out var name) ? name : location.ToString();

    public static string GetStatusName(BannerBookingStatus status) =>
        StatusNames.TryGetValue(status, out var name) ? name : status.ToString();

    public static string GetPaymentStatusName(BannerPaymentStatus status) =>
        PaymentStatusNames.TryGetValue(status, out var name) ? name : status.ToString();

    public static string GetRejectionReasonName(BannerRejectionReason reason) =>
        RejectionReasonNames.TryGetValue(reason, out var name) ? name : reason.ToString();

    public static string GetImageKindName(BannerImageKind kind) =>
        ImageKindNames.TryGetValue(kind, out var name) ? name : kind.ToString();

    public static string GetSlotName(int slotNumber) => $"Slot {slotNumber}";

    public const string DesktopSafeAreaNote =
        "يفضل أن تكون العناصر المهمة مثل اللوجو والنصوص في منتصف الصورة، بعيدًا عن الأطراف، " +
        "حتى لا يتم قصها على الشاشات المختلفة.";

    public const string ImageUsageNote =
        "يتم استخدام صورة Desktop على الشاشات الكبيرة، وصورة Mobile على الهواتف.";

    public const string PaymentTransferNote =
        "قم بتحويل قيمة الإعلان كاملة إلى بيانات الدفع الموضحة أعلاه، ثم ارفع صورة إثبات التحويل.";

    public const string ConfirmationStatement =
        "أقر بأن بيانات الإعلان صحيحة، وأوافق على مراجعة الإعلان وإثبات الدفع من إدارة المنصة قبل النشر.";

    public const string UnavailableMessage = "المساحة الإعلانية محجوزة حاليًا.";

    public const string AvailableMessage = "المساحة الإعلانية متاحة.";

    public static readonly DateTime SeedTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static class SeedIds
    {
        public const int HomeSlider1 = 1;
        public const int HomeSlider2 = 2;
        public const int SubCategoryBanner = 3;
    }

    public const int DefaultDesktopWidth = 1600;
    public const int DefaultDesktopHeight = 533;

    public const int DefaultMobileWidth = 1080;
    public const int DefaultMobileHeight = 720;

    public static readonly IReadOnlyList<SeedPlacement> SeedPlacements = new List<SeedPlacement>
    {
        new()
        {
            Id = SeedIds.HomeSlider1,
            Location = BannerLocation.HomeSlider1,
            Price = 1500m,
            MaxSlots = 3,
            DisplayOrder = 1
        },
        new()
        {
            Id = SeedIds.HomeSlider2,
            Location = BannerLocation.HomeSlider2,
            Price = 1000m,
            MaxSlots = 3,
            DisplayOrder = 2
        },
        new()
        {
            Id = SeedIds.SubCategoryBanner,
            Location = BannerLocation.SubCategoryBanner,
            Price = 500m,
            MaxSlots = 1,
            DisplayOrder = 3
        }
    };

    public sealed class SeedPlacement
    {
        public int Id { get; init; }
        public BannerLocation Location { get; init; }
        public decimal Price { get; init; }
        public int MaxSlots { get; init; }
        public int DisplayOrder { get; init; }

        public int DesktopWidth { get; init; } = DefaultDesktopWidth;
        public int DesktopHeight { get; init; } = DefaultDesktopHeight;
        public int MobileWidth { get; init; } = DefaultMobileWidth;
        public int MobileHeight { get; init; } = DefaultMobileHeight;

        public long MaxImageSizeBytes { get; init; } = DefaultMaxImageSizeBytes;

        public int DurationDays { get; init; } = DefaultDurationDays;

        public string AllowedFormats { get; init; } = string.Join(",", DefaultAllowedFormats);
    }

    public static string[] ParseFormats(string? stored) =>
        string.IsNullOrWhiteSpace(stored)
            ? DefaultAllowedFormats
            : stored
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(NormalizeFormat)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

    public static string JoinFormats(IEnumerable<string>? formats)
    {
        var normalised = (formats ?? DefaultAllowedFormats)
            .Where(format => !string.IsNullOrWhiteSpace(format))
            .Select(NormalizeFormat)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return normalised.Length == 0
            ? string.Join(",", DefaultAllowedFormats)
            : string.Join(",", normalised);
    }

    public static string NormalizeFormat(string format)
    {
        var trimmed = format.Trim().ToLowerInvariant();
        return trimmed.StartsWith('.') ? trimmed : $".{trimmed}";
    }
}
