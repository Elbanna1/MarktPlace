using Shared.DTOs.Advertisements;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IAdvertisementService
{
    Task<AdvertisementDetailsDto> CreateAsync(
        string ownerId,
        CreateAdvertisementRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video = null);

    Task<AdvertisementDetailsDto> UpdateAsync(string ownerId, Guid id, UpdateAdvertisementRequest request);

    Task DeleteAsync(string ownerId, Guid id);

    Task<PaginatedResult<AdvertisementListItemDto>> GetAllAsync(
        AdvertisementFilterParams filter, string? viewerUserId = null,
        CancellationToken cancellationToken = default);

    Task<AdvertisementDetailsDto> GetByIdAsync(
        Guid id, string? viewerUserId, string? ipAddress, CancellationToken cancellationToken = default);

    Task<AdvertisementStatisticsDto> GetMyStatisticsAsync(
        string ownerId, CancellationToken cancellationToken = default);

    Task<PaginatedResult<AdvertisementListItemDto>> GetMyAdsAsync(
        string ownerId, AdvertisementFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdvertisementDetailsDto> RepublishAsync(
        string ownerId, Guid id, UpdateAdvertisementRequest? edits = null);

    Task<IReadOnlyList<AdvertisementImageDto>> AddImagesAsync(
        string ownerId, Guid id, IReadOnlyList<UploadImageModel> images);

    Task DeleteImageAsync(string ownerId, Guid imageId);
}
