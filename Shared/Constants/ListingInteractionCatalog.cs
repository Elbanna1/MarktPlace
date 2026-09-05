using Shared.Enums;

namespace Shared.Constants;

public static class ListingInteractionCatalog
{
    public const int MinRating = 1;

    public const int MaxRating = 5;

    public static readonly string RatingRangeMessage =
        $"التقييم يجب أن يكون من {MinRating} إلى {MaxRating} نجوم.";

    public static bool IsValidRating(int rating) => rating is >= MinRating and <= MaxRating;

    public const int DefaultViewDedupeWindowMinutes = 360;

    public const int DefaultRecentlyViewedRetentionCount = 100;

    public const string AnonymousViewerKeyPrefix = "ip:";

    public const int ViewerKeyMaxLength = 100;

    public static readonly IReadOnlyDictionary<ListingStatus, string> ListingStatusNames =
        new Dictionary<ListingStatus, string>
        {
            [ListingStatus.Pending] = "قيد الانتظار",
            [ListingStatus.Active] = "نشط",
            [ListingStatus.Expired] = "منتهي",
            [ListingStatus.Rejected] = "مرفوض"
        };

    public static string NameOf(ListingStatus status) =>
        ListingStatusNames.TryGetValue(status, out var name) ? name : status.ToString();

    public static readonly IReadOnlyDictionary<ListingReportReason, string> ReportReasonNames =
        new Dictionary<ListingReportReason, string>
        {
            [ListingReportReason.Violating] = "إعلان مخالف",
            [ListingReportReason.Fraud] = "احتيال",
            [ListingReportReason.IncorrectData] = "بيانات غير صحيحة",
            [ListingReportReason.InappropriateContent] = "محتوى غير لائق",
            [ListingReportReason.Duplicate] = "إعلان مكرر",
            [ListingReportReason.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<ListingReportStatus, string> ReportStatusNames =
        new Dictionary<ListingReportStatus, string>
        {
            [ListingReportStatus.Pending] = "قيد المراجعة",
            [ListingReportStatus.UnderReview] = "جاري الفحص",
            [ListingReportStatus.ActionTaken] = "تم اتخاذ إجراء",
            [ListingReportStatus.Dismissed] = "مرفوض"
        };

    public const int ReportDetailsMaxLength = 1000;

    public const int ReportAdminNoteMaxLength = 1000;

    public const string ReportClosedOnListingRemovedNote =
        "أُغلق البلاغ تلقائيًا لأن الإعلان لم يعد موجودًا.";

    public static string NameOf(ListingReportReason reason) =>
        ReportReasonNames.TryGetValue(reason, out var name) ? name : reason.ToString();

    public static string NameOf(ListingReportStatus status) =>
        ReportStatusNames.TryGetValue(status, out var name) ? name : status.ToString();

    public static string ElapsedText(int days) => days switch
    {
        <= 0 => "اليوم",
        1 => "منذ يوم",
        2 => "منذ يومين",
        <= 10 => $"منذ {days} أيام",
        _ => $"منذ {days} يومًا"
    };
}
