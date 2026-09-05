using Shared.DTOs.BannerBookings;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IBannerSettingsService
{
    Task<IReadOnlyList<BannerPlacementDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<BannerPlacementDto> GetAsync(BannerLocation location, CancellationToken cancellationToken = default);

    Task<BannerPlacementDto> UpdateAsync(
        BannerLocation location, UpdateBannerPlacementRequest request, CancellationToken cancellationToken = default);
}
