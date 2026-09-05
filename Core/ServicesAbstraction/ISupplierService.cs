using Shared.DTOs.Advertisements;
using Shared.DTOs.Suppliers;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ISupplierService
{
    Task<SupplierDetailsDto> CreateAsync(
        string userId,
        CreateSupplierRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<SupplierDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateSupplierRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<SupplierListItemDto>> GetListAsync(
        SupplierFilterParams filter, CancellationToken cancellationToken = default);

    Task<SupplierDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SupplierSpecializationDto>> GetTypesAsync(CancellationToken cancellationToken = default);
}
