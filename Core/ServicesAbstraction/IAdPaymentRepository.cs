using Domain.Entities;
using Shared.DTOs.AdPayments;
using Shared.Enums;

namespace ServicesAbstraction;

public record AdPaymentListingSummary(string? Title, int? CategoryId, int? SubCategoryId);

public interface IAdPaymentRepository
{
    Task<AdPaymentSetting?> GetSettingsAsync(bool asNoTracking = true, CancellationToken cancellationToken = default);

    void AddSettings(AdPaymentSetting settings);

    Task<IReadOnlyList<PaymentMethod>> GetActiveMethodsAsync(CancellationToken cancellationToken = default);

    Task<Payment?> GetLatestForListingAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    Task<bool> HasPendingAsync(ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    Task<bool> HasCreditAsync(ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default);

    Task<int> ConsumeCreditAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow, DateTime? windowEnd,
        CancellationToken cancellationToken = default);

    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Payment> Items, int TotalCount)> GetPagedAsync(
        AdminAdPaymentFilterParams filter, CancellationToken cancellationToken = default);

    Task<int> DecideAsync(
        Guid id, PaymentStatus decision, string reviewerId, string? rejectionReason, DateTime utcNow,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<(ListingModuleType Type, Guid Id), AdPaymentListingSummary>> GetListingSummariesAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid Id)> listings, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, string>> GetUserNamesAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default);

    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
