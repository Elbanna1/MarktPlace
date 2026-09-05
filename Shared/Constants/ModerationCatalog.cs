using Shared.Enums;

namespace Shared.Constants;

public static class ModerationCatalog
{
    private static readonly IReadOnlyDictionary<ModerationStatus, string> StatusNames =
        new Dictionary<ModerationStatus, string>
        {
            [ModerationStatus.Pending] = "قيد المراجعة",
            [ModerationStatus.Approved] = "نشط",
            [ModerationStatus.Rejected] = "مرفوض",
            [ModerationStatus.Suspended] = "موقوف"
        };

    public static string NameOf(ModerationStatus status) =>
        StatusNames.TryGetValue(status, out var name) ? name : status.ToString();

    public static IReadOnlyList<ModerationStatus> AllStatuses { get; } = new[]
    {
        ModerationStatus.Pending,
        ModerationStatus.Approved,
        ModerationStatus.Rejected,
        ModerationStatus.Suspended
    };

    private static readonly IReadOnlyDictionary<ListingRejectionReason, string> ReasonNames =
        new Dictionary<ListingRejectionReason, string>
        {
            [ListingRejectionReason.IncompleteData] = "بيانات ناقصة",
            [ListingRejectionReason.ProhibitedImages] = "صور مخالفة",
            [ListingRejectionReason.ProhibitedContent] = "محتوى مخالف",
            [ListingRejectionReason.Duplicate] = "إعلان مكرر",
            [ListingRejectionReason.IncorrectInformation] = "معلومات غير صحيحة",
            [ListingRejectionReason.Other] = "سبب آخر"
        };

    public static string NameOf(ListingRejectionReason reason) =>
        ReasonNames.TryGetValue(reason, out var name) ? name : reason.ToString();

    public static IReadOnlyList<ListingRejectionReason> AllRejectionReasons { get; } = new[]
    {
        ListingRejectionReason.IncompleteData,
        ListingRejectionReason.ProhibitedImages,
        ListingRejectionReason.ProhibitedContent,
        ListingRejectionReason.Duplicate,
        ListingRejectionReason.IncorrectInformation,
        ListingRejectionReason.Other
    };

    public static bool RequiresNotes(ListingRejectionReason reason) =>
        reason == ListingRejectionReason.Other;

    public const int MaxNotesLength = 500;
}
