using Domain.Entities;
using Shared.DTOs.Companies;

namespace ServicesAbstraction;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Company?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Company> Items, int TotalCount)> GetPagedAsync(
        CompanyFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<CompanyFieldLookup>> GetCompanyFieldsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Company company);

    void Update(Company company);

    void RemoveImage(CompanyImage image);

    Task AddImagesAsync(IEnumerable<CompanyImage> images);

    Task<int> SaveChangesAsync();
}
