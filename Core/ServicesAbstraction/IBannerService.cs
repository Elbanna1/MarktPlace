using Shared.DTOs.Advertisements;
using Shared.DTOs.Banners;

namespace ServicesAbstraction;

public interface IBannerService
{
    Task<IReadOnlyList<BannerDto>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerDto>> GetAllAsync(
        BannerFilterParams filter, CancellationToken cancellationToken = default);

    Task<BannerDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BannerDto> CreateAsync(
        CreateBannerRequest request, UploadImageModel? image, CancellationToken cancellationToken = default);

    Task<BannerDto> UpdateAsync(
        Guid id, UpdateBannerRequest request, UploadImageModel? image,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
