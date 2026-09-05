using Shared.DTOs.Advertisements;
using Shared.DTOs.Factories;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IFactoryService
{
    Task<FactoryDetailsDto> CreateAsync(
        string userId,
        CreateFactoryRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<FactoryDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateFactoryRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<FactoryListItemDto>> GetListAsync(
        FactoryFilterParams filter, CancellationToken cancellationToken = default);

    Task<FactoryDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductionSpecialtyOptionDto>> GetProductionSpecialtiesAsync(
        CancellationToken cancellationToken = default);
}
