using Shared.DTOs.Advertisements;
using Shared.DTOs.Companies;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ICompanyService
{
    Task<CompanyDetailsDto> CreateAsync(
        string userId, CreateCompanyRequest request,
        IReadOnlyList<UploadImageModel> images, UploadImageModel? logo);

    Task<CompanyDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateCompanyRequest request,
        IReadOnlyList<UploadImageModel> newImages, UploadImageModel? logo);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<CompanyListItemDto>> GetListAsync(
        CompanyFilterParams filter, CancellationToken cancellationToken = default);

    Task<CompanyDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CompanyFieldOptionDto>> GetCompanyFieldsAsync(CancellationToken cancellationToken = default);
}
