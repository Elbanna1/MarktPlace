using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.DTOs.Common;

namespace Shared.DTOs.AdPayments;

public class AdPaymentMethodDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string? ArabicName { get; set; }

    public PaymentMethodType Type { get; set; }

    public string? PhoneNumber { get; set; }

    public string? InstaPayIdentifier { get; set; }

    public string? BankName { get; set; }

    public string? AccountHolderName { get; set; }

    public string? AccountNumber { get; set; }

    public string? Iban { get; set; }

    public string? PaymentInfo { get; set; }

    public string? Instructions { get; set; }
}

public class PublicAdPaymentConfigDto
{
    public bool IsEnabled { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = default!;

    public string? Instructions { get; set; }

    public IReadOnlyList<AdPaymentMethodDto> PaymentMethods { get; set; } = Array.Empty<AdPaymentMethodDto>();

    public DateTime? UpdatedAt { get; set; }
}

public class AdminAdPaymentSettingsDto : PublicAdPaymentConfigDto
{
    public IReadOnlyList<int> PaymentMethodIds { get; set; } = Array.Empty<int>();

    public bool UsesAllActiveMethods { get; set; }

    public DateTime? EnabledAt { get; set; }

    public string? UpdatedBy { get; set; }

    public string? UpdatedByName { get; set; }
}

public class UpdateAdPaymentSettingsRequest
{
    public bool IsEnabled { get; set; }

    public decimal Price { get; set; }

    public string? Currency { get; set; }

    public string? Instructions { get; set; }

    public IReadOnlyList<int>? PaymentMethodIds { get; set; }
}

public class SubmitAdPaymentRequest
{
    public int PaymentMethodId { get; set; }

    public string? TransactionReference { get; set; }

    public IFormFile? Screenshot { get; set; }

    public string? Notes { get; set; }

    public decimal? ExpectedAmount { get; set; }

    public string? ExpectedCurrency { get; set; }
}

public class AdPaymentSubmissionResultDto
{
    public Guid PaymentSubmissionId { get; set; }

    public Guid AdvertisementId { get; set; }

    public ListingModuleType AdvertisementType { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = default!;

    public PaymentStatus PaymentStatus { get; set; }

    public string PaymentStatusName { get; set; } = default!;

    public DateTime SubmittedAt { get; set; }

    public int PaymentMethodId { get; set; }

    public string? PaymentMethodName { get; set; }

    public string? TransactionReference { get; set; }

    public string? Notes { get; set; }

    public bool IsResubmission { get; set; }

    public Guid? PreviousPaymentSubmissionId { get; set; }

    public string Stage { get; set; } = default!;

    public string StageName { get; set; } = default!;

    public string StatusEndpoint { get; set; } = default!;
}

public class AdPaymentStatusDto
{
    public Guid AdvertisementId { get; set; }

    public ListingModuleType AdvertisementType { get; set; }

    public bool PaymentsEnabled { get; set; }

    public bool IsPaymentRequired { get; set; }

    public bool CanSubmit { get; set; }

    public bool HasPayment { get; set; }

    public Guid? PaymentSubmissionId { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }

    public string? PaymentStatusName { get; set; }

    public decimal? Amount { get; set; }

    public string? Currency { get; set; }

    public int? PaymentMethodId { get; set; }

    public string? PaymentMethodName { get; set; }

    public string? TransactionReference { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string? RejectionReason { get; set; }

    public decimal? CurrentPrice { get; set; }

    public string? CurrentCurrency { get; set; }

    public bool CanResubmit { get; set; }

    public string Stage { get; set; } = default!;

    public string StageName { get; set; } = default!;

    public string? AdvertisementTitle { get; set; }

    public string AdvertisementModerationStatus { get; set; } = default!;

    public string AdvertisementModerationStatusName { get; set; } = default!;

    public string? Notes { get; set; }

    public string SubmitEndpoint { get; set; } = default!;

    public string ResubmitEndpoint { get; set; } = default!;
}

public class AdPaymentModuleDto
{
    public ListingModuleType AdvertisementType { get; set; }

    public string AdvertisementTypeKey { get; set; } = default!;

    public string AdvertisementTypeName { get; set; } = default!;

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public string? ApiRoute { get; set; }

    public bool IsSubjectToPayment { get; set; }
}

public class AdPaymentListingStateDto
{
    public bool PaymentsEnabled { get; set; }

    public bool IsPaymentRequired { get; set; }

    public string Stage { get; set; } = default!;

    public string StageName { get; set; } = default!;

    public Guid? PaymentSubmissionId { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }
}

public class AdminAdPaymentFilterParams : PaginationParams
{
    public PaymentStatus? Status { get; set; }

    public ListingModuleType? Type { get; set; }

    public int? CategoryId { get; set; }

    public string? UserId { get; set; }

    public Guid? AdvertisementId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}

public class AdminAdPaymentSubmissionDto
{
    public Guid PaymentSubmissionId { get; set; }

    public Guid AdvertisementId { get; set; }

    public ListingModuleType AdvertisementType { get; set; }

    public string AdvertisementTypeName { get; set; } = default!;

    public string? AdvertisementTitle { get; set; }

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? SubCategoryId { get; set; }

    public string? SubCategoryName { get; set; }

    public string OwnerId { get; set; } = default!;

    public string? OwnerName { get; set; }

    public string? OwnerPhone { get; set; }

    public string? OwnerEmail { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = default!;

    public int PaymentMethodId { get; set; }

    public string PaymentMethodName { get; set; } = default!;

    public string? PaymentInfo { get; set; }

    public string? TransactionReference { get; set; }

    public DateTime SubmittedAt { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public string PaymentStatusName { get; set; } = default!;

    public DateTime? ReviewedAt { get; set; }

    public string? ReviewedBy { get; set; }

    public string? ReviewedByName { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime? ConsumedAt { get; set; }

    public string ScreenshotUrl { get; set; } = default!;
}

public class ReviewAdPaymentRequest
{
    public string? Decision { get; set; }

    public string? RejectionReason { get; set; }
}

public class AdPaymentReviewResultDto
{
    public AdminAdPaymentSubmissionDto Payment { get; set; } = default!;

    public string ListingAction { get; set; } = default!;

    public string ListingActionName { get; set; } = default!;

    public string? ListingModerationStatus { get; set; }
}

public static class AdPaymentListingActions
{
    public const string None = "None";

    public const string AwaitingModeration = "AwaitingModeration";

    public const string Republished = "Republished";

    public const string CreditKept = "CreditKept";
}
