using Shared.DTOs.Advertisements;
using Shared.DTOs.AdPayments;
using Shared.Enums;
using Shared.Responses;

namespace ServicesAbstraction;

public sealed record PrivateFileContent(Stream Content, string ContentType, string FileName);

public interface IAdPaymentService
{
    Task<PublicAdPaymentConfigDto> GetPublicConfigAsync(CancellationToken cancellationToken = default);

    Task<AdminAdPaymentSettingsDto> GetSettingsAsync(CancellationToken cancellationToken = default);

    Task<AdminAdPaymentSettingsDto> UpdateSettingsAsync(
        UpdateAdPaymentSettingsRequest request, string adminUserId, CancellationToken cancellationToken = default);

    Task<AdPaymentSubmissionResultDto> SubmitAsync(
        ListingModuleType type, Guid listingId, string userId, SubmitAdPaymentRequest request,
        UploadImageModel? screenshot, CancellationToken cancellationToken = default);

    Task<AdPaymentSubmissionResultDto> ResubmitAsync(
        ListingModuleType type, Guid listingId, string userId, SubmitAdPaymentRequest request,
        UploadImageModel? screenshot, CancellationToken cancellationToken = default);

    IReadOnlyList<AdPaymentModuleDto> GetModules();

    Task<AdPaymentStatusDto> GetStatusAsync(
        ListingModuleType type, Guid listingId, string userId, CancellationToken cancellationToken = default);

    Task<PaginatedResult<AdminAdPaymentSubmissionDto>> GetSubmissionsAsync(
        AdminAdPaymentFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdminAdPaymentSubmissionDto> GetSubmissionAsync(Guid id, CancellationToken cancellationToken = default);

    Task<AdPaymentReviewResultDto> ReviewAsync(
        Guid id, ReviewAdPaymentRequest request, string adminUserId, CancellationToken cancellationToken = default);

    Task<PrivateFileContent> OpenScreenshotAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> IsAdPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default);

    Task EnsureMethodCanBeWithdrawnAsync(int paymentMethodId, CancellationToken cancellationToken = default);
}

public sealed record AdPaymentListingSnapshot(
    ListingModuleType Type, Guid Id, DateTime? CreatedAt, ModerationStatus ModerationStatus, bool OpensNewWindow);

public interface IAdPaymentGate
{
    Task EnsurePaidAsync(
        ListingModuleType type, Guid listingId, DateTime? listingCreatedAt, CancellationToken cancellationToken = default);

    Task<bool> IsRequiredForNewListingAsync(ListingModuleType type, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<(ListingModuleType Type, Guid Id), AdPaymentListingStateDto>> DescribeAsync(
        IReadOnlyCollection<AdPaymentListingSnapshot> listings, DateTime utcNow,
        CancellationToken cancellationToken = default);

    Task ConsumeAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow, DateTime? windowEnd,
        CancellationToken cancellationToken = default);

    Task<AdPaymentQueueFilter?> GetQueueFilterAsync(
        string? requestedStage, ModerationStatus? moderationStatus, CancellationToken cancellationToken = default);

    Task<bool> IsAwaitingPaymentAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);
}

public interface IPrivateFileStore
{
    Task<StoredFile> SaveImageAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default);

    PrivateFileContent? Open(string relativePath);

    void Delete(string relativePath);
}
