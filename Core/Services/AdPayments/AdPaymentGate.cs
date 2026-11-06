using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.AdPayments;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.AdPayments;

public class AdPaymentGate : IAdPaymentGate
{
    private readonly IAdPaymentRepository _repository;

    public AdPaymentGate(IAdPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task EnsurePaidAsync(
        ListingModuleType type, Guid listingId, DateTime? listingCreatedAt,
        CancellationToken cancellationToken = default)
    {
        if (!AdPaymentCatalog.IsSubjectToPayment(type))
            return;

        var settings = await _repository.GetSettingsAsync(asNoTracking: true, cancellationToken);

        if (settings is not { IsEnabled: true })
            return;

        if (AdPaymentRules.IsGrandfathered(settings.EnabledAt, ModerationStatus.Pending, listingCreatedAt))
            return;

        if (await _repository.HasCreditAsync(type, listingId, cancellationToken))
            return;

        throw new PaymentRequiredException(
            await _repository.HasPendingAsync(type, listingId, cancellationToken)
                ? "صاحب الإعلان رفع إثبات دفع لسه قيد المراجعة. راجع الدفع وأكده الأول، وبعدها وافق على الإعلان."
                : "الدفع للإعلانات مفعل، والإعلان ده لسه مدفعش رسوم النشر. لازم يتم تأكيد الدفع قبل الموافقة على الإعلان.");
    }

    public async Task<bool> IsRequiredForNewListingAsync(
        ListingModuleType type, CancellationToken cancellationToken = default) =>
        AdPaymentCatalog.IsSubjectToPayment(type) &&
        await _repository.GetSettingsAsync(asNoTracking: true, cancellationToken) is { IsEnabled: true };

    public async Task<IReadOnlyDictionary<(ListingModuleType Type, Guid Id), AdPaymentListingStateDto>> DescribeAsync(
        IReadOnlyCollection<AdPaymentListingSnapshot> listings, DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<(ListingModuleType, Guid), AdPaymentListingStateDto>();

        if (listings.Count == 0)
            return result;

        var settings = await _repository.GetSettingsAsync(asNoTracking: true, cancellationToken);
        var enabled = settings is { IsEnabled: true };

        var facts = await _repository.GetListingFactsAsync(
            listings.Select(listing => (listing.Type, listing.Id)).Distinct().ToList(), cancellationToken);

        foreach (var listing in listings)
        {
            var fact = facts.GetValueOrDefault((listing.Type, listing.Id));
            var hasCredit = fact?.HasCredit ?? false;

            var required = enabled &&
                           AdPaymentCatalog.IsSubjectToPayment(listing.Type) &&
                           listing.OpensNewWindow &&
                           !AdPaymentRules.IsGrandfathered(settings!.EnabledAt, listing.ModerationStatus, listing.CreatedAt) &&
                           !hasCredit;

            var stage = AdPaymentCatalog.ResolveStage(required, fact?.LatestStatus, hasCredit);

            result[(listing.Type, listing.Id)] = new AdPaymentListingStateDto
            {
                PaymentsEnabled = enabled,
                IsPaymentRequired = required,
                Stage = stage,
                StageName = AdPaymentCatalog.Stages.NameOf(stage),
                PaymentSubmissionId = fact?.LatestPaymentId,
                PaymentStatus = fact?.LatestStatus
            };
        }

        return result;
    }

    public async Task<AdPaymentQueueFilter?> GetQueueFilterAsync(
        string? requestedStage, ModerationStatus? moderationStatus, CancellationToken cancellationToken = default)
    {
        var stage = requestedStage is null
            ? moderationStatus == ModerationStatus.Pending ? AdminAdPaymentStages.ReadyForReview : AdminAdPaymentStages.All
            : AdminAdPaymentStages.Parse(requestedStage)
              ?? throw new BadRequestException(
                  $"قيمة مرحلة الدفع غير صحيحة. القيم المسموحة: {string.Join("، ", AdminAdPaymentStages.Values)}.");

        if (stage == AdminAdPaymentStages.All)
            return null;

        var settings = await _repository.GetSettingsAsync(asNoTracking: true, cancellationToken);

        if (settings is not { IsEnabled: true })
            return stage == AdminAdPaymentStages.AwaitingPayment
                ? new AdPaymentQueueFilter(DateTime.MaxValue, AdminAdPaymentStages.AwaitingPayment)
                : null;

        return new AdPaymentQueueFilter(settings.EnabledAt, stage);
    }

    public async Task<bool> IsAwaitingPaymentAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default)
    {
        if (!AdPaymentCatalog.IsSubjectToPayment(type))
            return false;

        var row = await _repository.GetListingRowAsync(type, listingId, cancellationToken);

        if (row is null)
            return false;

        var utcNow = DateTime.UtcNow;

        var states = await DescribeAsync(
            new[]
            {
                new AdPaymentListingSnapshot(
                    type, listingId, row.CreatedAt, row.ModerationStatus,
                    AdPaymentRules.AwaitsFirstWindow(row.ModerationStatus, row.ExpireAt, utcNow))
            },
            utcNow, cancellationToken);

        return states.TryGetValue((type, listingId), out var state) && state.IsPaymentRequired;
    }

    public Task ConsumeAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow, DateTime? windowEnd,
        CancellationToken cancellationToken = default) =>
        _repository.ConsumeCreditAsync(type, listingId, utcNow, windowEnd, cancellationToken);
}
