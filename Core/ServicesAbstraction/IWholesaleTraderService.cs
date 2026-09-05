using Shared.DTOs.Advertisements;
using Shared.DTOs.WholesaleTraders;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IWholesaleTraderService
{
    Task<WholesaleTraderDetailsDto> CreateAsync(
        string userId,
        CreateWholesaleTraderRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<WholesaleTraderDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateWholesaleTraderRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<WholesaleTraderListItemDto>> GetListAsync(
        WholesaleTraderFilterParams filter, CancellationToken cancellationToken = default);

    Task<WholesaleTraderDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WholesaleTradeTypeDto>> GetTradeTypesAsync(CancellationToken cancellationToken = default);

    IReadOnlyList<WholesaleSaleTypeDto> GetSaleTypes();
}
