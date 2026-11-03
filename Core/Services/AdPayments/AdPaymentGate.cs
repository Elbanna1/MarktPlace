using ServicesAbstraction;
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
        var settings = await _repository.GetSettingsAsync(asNoTracking: true, cancellationToken);

        if (settings is not { IsEnabled: true })
            return;

        if (settings.EnabledAt is { } enabledAt && listingCreatedAt is { } createdAt && createdAt < enabledAt)
            return;

        if (await _repository.HasCreditAsync(type, listingId, cancellationToken))
            return;

        throw new PaymentRequiredException(
            await _repository.HasPendingAsync(type, listingId, cancellationToken)
                ? "صاحب الإعلان رفع إثبات دفع لسه قيد المراجعة. راجع الدفع وأكده الأول، وبعدها وافق على الإعلان."
                : "الدفع للإعلانات مفعل، والإعلان ده لسه مدفعش رسوم النشر. لازم يتم تأكيد الدفع قبل الموافقة على الإعلان.");
    }

    public Task ConsumeAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow, DateTime? windowEnd,
        CancellationToken cancellationToken = default) =>
        _repository.ConsumeCreditAsync(type, listingId, utcNow, windowEnd, cancellationToken);
}
