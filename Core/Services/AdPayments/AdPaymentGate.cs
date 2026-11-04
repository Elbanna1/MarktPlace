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

    public Task ConsumeAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow, DateTime? windowEnd,
        CancellationToken cancellationToken = default) =>
        _repository.ConsumeCreditAsync(type, listingId, utcNow, windowEnd, cancellationToken);
}
