using Shared.Enums;

namespace Shared.Constants;

public static class FeedbackCatalog
{
    public static readonly IReadOnlyDictionary<FeedbackType, string> TypeNames =
        new Dictionary<FeedbackType, string>
        {
            [FeedbackType.Suggestion] = "اقتراح",
            [FeedbackType.Complaint] = "شكوى",
            [FeedbackType.ProblemReport] = "الإبلاغ عن مشكلة",
            [FeedbackType.FeatureRequest] = "طلب ميزة جديدة",
            [FeedbackType.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<FeedbackStatus, string> StatusNames =
        new Dictionary<FeedbackStatus, string>
        {
            [FeedbackStatus.New] = "جديد",
            [FeedbackStatus.UnderReview] = "قيد المراجعة",
            [FeedbackStatus.Replied] = "تم الرد",
            [FeedbackStatus.Resolved] = "تم الحل",
            [FeedbackStatus.Closed] = "مغلق"
        };

    public static readonly IReadOnlySet<FeedbackStatus> ClosedStatuses =
        new HashSet<FeedbackStatus> { FeedbackStatus.Resolved, FeedbackStatus.Closed };

    public static string NameOf(FeedbackType type) =>
        TypeNames.TryGetValue(type, out var name) ? name : type.ToString();

    public static string NameOf(FeedbackStatus status) =>
        StatusNames.TryGetValue(status, out var name) ? name : status.ToString();

    public static bool IsClosed(FeedbackStatus status) => ClosedStatuses.Contains(status);

    public const int MaxTitleLength = 150;

    public const int MaxDescriptionLength = 4000;

    public const int MaxAdminReplyLength = 4000;

    public const int MinRating = 1;

    public const int MaxRating = 5;

    public const int MaxImages = ImageConstants.MaxImagesPerItem;
}
