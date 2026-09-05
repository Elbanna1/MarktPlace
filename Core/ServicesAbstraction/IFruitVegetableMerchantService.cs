using Shared.DTOs.Advertisements;
using Shared.DTOs.FruitVegetableMerchants;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IFruitVegetableMerchantService
{
    Task<FruitVegetableMerchantDetailsDto> CreateAsync(
        string userId,
        CreateFruitVegetableMerchantRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<FruitVegetableMerchantDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateFruitVegetableMerchantRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<FruitVegetableMerchantListItemDto>> GetListAsync(
        FruitVegetableMerchantFilterParams filter, CancellationToken cancellationToken = default);

    Task<FruitVegetableMerchantDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MerchantSaleTypeDto>> GetSaleTypesAsync(CancellationToken cancellationToken = default);
}
