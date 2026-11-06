using Shared.Enums;

namespace Shared.Constants;

public static class AdPaymentCatalog
{
    public const string ConfigRoute = "/api/v2/ad-payment/config";

    public const string PublicConfigRoute = "/api/v2/public/ad-payment/config";

    public const string ModulesRoute = "/api/v2/ad-payment/modules";

    public const string SubmitRouteTemplate = "/api/v2/ads/{advertisementType}/{advertisementId}/payment";

    public const string ResubmitRouteTemplate = "/api/v2/ads/{advertisementType}/{advertisementId}/payment/resubmit";

    public const string StatusRouteTemplate = "/api/v2/ads/{advertisementType}/{advertisementId}/payment";

    public const string ContentType = "multipart/form-data";

    public const int MaxNotesLength = 500;

    public const string FormSection = "payment";

    public static class Fields
    {
        public const string PaymentMethodId = "PaymentMethodId";

        public const string TransactionReference = "TransactionReference";

        public const string Screenshot = "Screenshot";

        public const string Notes = "Notes";

        public const string ExpectedAmount = "ExpectedAmount";

        public const string ExpectedCurrency = "ExpectedCurrency";
    }

    public static class Stages
    {
        public const string NotRequired = "NotRequired";

        public const string AwaitingPayment = "AwaitingPayment";

        public const string PaymentUnderReview = "PaymentUnderReview";

        public const string PaymentRejected = "PaymentRejected";

        public const string Paid = "Paid";

        public static string NameOf(string stage) => stage switch
        {
            AwaitingPayment => "في انتظار دفع رسوم الإعلان",
            PaymentUnderReview => "إثبات الدفع قيد المراجعة",
            PaymentRejected => "تم رفض إثبات الدفع، ارفع إثبات جديد",
            Paid => "تم تأكيد الدفع",
            _ => "الإعلان مش محتاج دفع دلوقتي"
        };
    }

    private static readonly IReadOnlySet<ListingModuleType> Exempt = new HashSet<ListingModuleType>();

    public static bool IsSubjectToPayment(ListingModuleType type) =>
        Enum.IsDefined(type) && !Exempt.Contains(type);

    public static IReadOnlyList<ListingModuleType> SubjectModules { get; } =
        ListingModuleCatalog.All.Where(IsSubjectToPayment).ToList();

    public static string SubmitRouteOf(ListingModuleType type, Guid listingId) =>
        $"/api/v2/ads/{type}/{listingId}/payment";

    public static string ResubmitRouteOf(ListingModuleType type, Guid listingId) =>
        $"/api/v2/ads/{type}/{listingId}/payment/resubmit";

    public static string ResolveStage(bool required, PaymentStatus? latest, bool hasCredit)
    {
        if (hasCredit)
            return Stages.Paid;

        if (latest == PaymentStatus.Pending)
            return Stages.PaymentUnderReview;

        if (!required)
            return latest == PaymentStatus.Approved ? Stages.Paid : Stages.NotRequired;

        return latest == PaymentStatus.Rejected ? Stages.PaymentRejected : Stages.AwaitingPayment;
    }
}
