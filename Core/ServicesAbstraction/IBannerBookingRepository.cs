using Domain.Entities;
using Shared.DTOs.BannerBookings;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IBannerBookingRepository
{
    Task<IReadOnlyList<BannerPlacementSetting>> GetPlacementsAsync(
        bool activeOnly = true, CancellationToken cancellationToken = default);

    Task<BannerPlacementSetting?> GetPlacementAsync(
        BannerLocation location, bool asNoTracking = true, CancellationToken cancellationToken = default);

    void UpdatePlacement(BannerPlacementSetting placement);

    Task<BannerBooking?> GetByIdAsync(
        Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<BannerBooking> Items, int TotalCount)> GetPagedAsync(
        BannerBookingFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerBooking>> GetOverlappingAsync(
        BannerLocation location,
        int? slotNumber,
        int? categoryId,
        int? subCategoryId,
        DateTime startDate,
        DateTime endDate,
        Guid? excludeBookingId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerBooking>> GetByUserAsync(
        string userId, BannerBookingStatus? status = null, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerBooking>> GetPublishedAsync(
        DateTime utcNow,
        BannerLocation? location = null,
        int? categoryId = null,
        int? subCategoryId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerBooking>> GetBookingsToPublishAsync(
        DateTime utcNow, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerBooking>> GetBookingsToExpireAsync(
        DateTime utcNow, CancellationToken cancellationToken = default);

    Task AddAsync(BannerBooking booking, CancellationToken cancellationToken = default);

    void Update(BannerBooking booking);

    void Remove(BannerBooking booking);

    Task<SubCategory?> GetSubCategoryAsync(int subCategoryId, CancellationToken cancellationToken = default);

    Task<PaymentMethod?> GetPaymentMethodAsync(int id, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
