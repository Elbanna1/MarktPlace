using ServicesAbstraction;
using Shared.Catalog;
using Shared.Constants;
using Shared.DTOs.AdPayments;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.AdPayments;

public class AdPaymentGate : IAdPaymentGate
{
    private readonly IAdPaymentRepository _repository;
    private readonly ICatalogConfigurationProvider? _catalog;

    public AdPaymentGate(IAdPaymentRepository repository, ICatalogConfigurationProvider? catalog = null)
    {
        _repository = repository;
        _catalog = catalog;
    }

    public async Task EnsurePaidAsync(
        ListingModuleType type, Guid listingId, DateTime? listingCreatedAt,
        CancellationToken cancellationToken = default)
    {
        if (!AdPaymentCatalog.IsSubjectToPayment(type))
            return;

        var subCategoryId = await SubCategoryOfAsync(type, listingId, cancellationToken);
        var effective = await ResolveAsync(type, subCategoryId, cancellationToken);

        if (!effective.IsRequired)
            return;

        if (AdPaymentRules.IsGrandfathered(effective.RequiredSince, ModerationStatus.Pending, listingCreatedAt))
            return;

        if (await _repository.HasCreditAsync(type, listingId, cancellationToken))
            return;

        throw new PaymentRequiredException(
            await _repository.HasPendingAsync(type, listingId, cancellationToken)
                ? "صاحب الإعلان رفع إثبات دفع لسه قيد المراجعة. راجع الدفع وأكده الأول، وبعدها وافق على الإعلان."
                : "الدفع للإعلانات مفعل، والإعلان ده لسه مدفعش رسوم النشر. لازم يتم تأكيد الدفع قبل الموافقة على الإعلان.");
    }

    public async Task<bool> IsRequiredForNewListingAsync(
        ListingModuleType type, CancellationToken cancellationToken = default, int? subCategoryId = null) =>
        AdPaymentCatalog.IsSubjectToPayment(type) &&
        (await ResolveAsync(type, subCategoryId, cancellationToken)).IsRequired;

    public async Task<EffectivePaymentConfiguration> ResolveAsync(
        ListingModuleType type, int? subCategoryId, CancellationToken cancellationToken = default)
    {
        var snapshot = await SnapshotAsync(cancellationToken);

        return snapshot.ResolvePayment(CatalogContext.ForModule(type, subCategoryId), DateTime.UtcNow);
    }

    public async Task<IReadOnlyDictionary<(ListingModuleType Type, Guid Id), AdPaymentListingStateDto>> DescribeAsync(
        IReadOnlyCollection<AdPaymentListingSnapshot> listings, DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<(ListingModuleType, Guid), AdPaymentListingStateDto>();

        if (listings.Count == 0)
            return result;

        var snapshot = await SnapshotAsync(cancellationToken);

        var facts = await _repository.GetListingFactsAsync(
            listings.Select(listing => (listing.Type, listing.Id)).Distinct().ToList(), cancellationToken);

        foreach (var listing in listings)
        {
            var fact = facts.GetValueOrDefault((listing.Type, listing.Id));
            var hasCredit = fact?.HasCredit ?? false;

            var effective = snapshot.ResolvePayment(
                CatalogContext.ForModule(listing.Type, listing.SubCategoryId), utcNow);

            var required = effective.IsRequired &&
                           listing.OpensNewWindow &&
                           !AdPaymentRules.IsGrandfathered(effective.RequiredSince, listing.ModerationStatus, listing.CreatedAt) &&
                           !hasCredit;

            var stage = AdPaymentCatalog.ResolveStage(required, fact?.LatestStatus, hasCredit);

            result[(listing.Type, listing.Id)] = new AdPaymentListingStateDto
            {
                PaymentsEnabled = effective.IsRequired,
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

        var snapshot = await SnapshotAsync(cancellationToken);
        var rules = BuildQueueRules(snapshot, DateTime.UtcNow);

        if (rules.Count == 0 && stage == AdminAdPaymentStages.ReadyForReview)
            return null;

        return new AdPaymentQueueFilter(stage, rules);
    }

    public static IReadOnlyDictionary<ListingModuleType, IReadOnlyList<AdPaymentQueueRule>> BuildQueueRules(
        CatalogConfigurationSnapshot snapshot, DateTime utcNow)
    {
        var rules = new Dictionary<ListingModuleType, IReadOnlyList<AdPaymentQueueRule>>();

        foreach (var module in AdPaymentCatalog.SubjectModules)
        {
            if (ListingModuleCatalog.SubCategoryOf(module) is not null)
            {
                var effective = snapshot.ResolvePayment(CatalogContext.ForModule(module), utcNow);

                if (effective.IsRequired)
                    rules[module] = new[] { new AdPaymentQueueRule(null, effective.RequiredSince) };

                continue;
            }

            var grouped = snapshot.KnownSubCategoryIds
                .Concat(Enum.GetValues<SubCategoryType>()
                    .Where(subCategory => ListingModuleCatalog.ModuleOf(subCategory) == module)
                    .Select(subCategory => (int)subCategory))
                .Distinct()
                .Select(subCategoryId => (
                    SubCategoryId: subCategoryId,
                    Effective: snapshot.ResolvePayment(CatalogContext.ForModule(module, subCategoryId), utcNow)))
                .Where(candidate => candidate.Effective.IsRequired)
                .GroupBy(candidate => candidate.Effective.RequiredSince)
                .Select(group => new AdPaymentQueueRule(
                    group.Select(candidate => (int?)candidate.SubCategoryId).ToList(), group.Key))
                .ToList();

            if (grouped.Count > 0)
                rules[module] = grouped;
        }

        return rules;
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
                    AdPaymentRules.AwaitsFirstWindow(row.ModerationStatus, row.ExpireAt, utcNow),
                    row.SubCategoryId)
            },
            utcNow, cancellationToken);

        return states.TryGetValue((type, listingId), out var state) && state.IsPaymentRequired;
    }

    public Task ConsumeAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow, DateTime? windowEnd,
        CancellationToken cancellationToken = default) =>
        _repository.ConsumeCreditAsync(type, listingId, utcNow, windowEnd, cancellationToken);

    private async Task<int?> SubCategoryOfAsync(ListingModuleType type, Guid listingId, CancellationToken cancellationToken)
    {
        if (ListingModuleCatalog.SubCategoryOf(type) is { } fixedSubCategory)
            return (int)fixedSubCategory;

        return (await _repository.GetListingRowAsync(type, listingId, cancellationToken))?.SubCategoryId;
    }

    public Task<CatalogConfigurationSnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default) =>
        SnapshotAsync(cancellationToken);

    private async Task<CatalogConfigurationSnapshot> SnapshotAsync(CancellationToken cancellationToken)
    {
        if (_catalog is not null)
            return await _catalog.GetSnapshotAsync(cancellationToken);

        var settings = await _repository.GetSettingsAsync(asNoTracking: true, cancellationToken);

        return new CatalogConfigurationSnapshot(
            Array.Empty<CatalogConfigurationEntry>(),
            new CatalogPaymentRoot(
                settings?.IsEnabled ?? false,
                settings?.Price ?? 0m,
                settings?.Currency ?? PaymentCatalog.DefaultCurrency,
                settings?.EnabledAt),
            new Dictionary<int, int>());
    }
}
