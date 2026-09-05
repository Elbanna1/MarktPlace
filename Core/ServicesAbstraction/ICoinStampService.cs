using Shared.DTOs.Advertisements;
using Shared.DTOs.Antiques;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ICoinStampService
{
    Task<CoinStampDetailsDto> CreateAsync(
        string userId,
        CreateCoinStampRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<CoinStampDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateCoinStampRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<CoinStampListItemDto>> GetListAsync(
        CoinStampFilterParams filter, CancellationToken cancellationToken = default);

    Task<CoinStampDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetItemTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetMetalsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetConditionsAsync(CancellationToken cancellationToken = default);
}
