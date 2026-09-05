using Domain.Entities;
using Shared.DTOs.Payments;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IPaymentRepository
{
    Task<IReadOnlyList<PaymentMethod>> GetMethodsAsync(bool activeOnly = true, CancellationToken cancellationToken = default);

    Task<PaymentMethod?> GetMethodByIdAsync(int id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<bool> MethodNameExistsAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);

    void AddMethod(PaymentMethod method);

    void UpdateMethod(PaymentMethod method);

    void RemoveMethod(PaymentMethod method);

    Task<int> CountMethodUsagesAsync(int methodId, CancellationToken cancellationToken = default);

    Task<Payment?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Payment>> GetByUserAsync(
        string userId, PaymentStatus? status = null, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Payment> Items, int TotalCount)> GetPagedAsync(
        PaymentFilterParams filter, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Payment> Items, int TotalCount)> GetPagedForAdminAsync(
        Shared.DTOs.Admin.AdminPaymentFilterParams filter, CancellationToken cancellationToken = default);

    Task<Payment?> GetForAdminAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<BannerBooking?> FindTargetBannerBookingAsync(
        Payment payment, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, string>> GetUserNamesAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default);

    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);

    void Update(Payment payment);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
