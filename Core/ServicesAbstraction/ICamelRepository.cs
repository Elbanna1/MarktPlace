using Domain.Entities;
using Shared.DTOs.Animals;

namespace ServicesAbstraction;

public interface ICamelRepository
{
    Task<Camel?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Camel?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Camel> Items, int TotalCount)> GetPagedAsync(
        CamelFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<CamelBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default);

    Task<List<CamelPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<List<CamelAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<List<CamelGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<List<CamelHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<List<CamelVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Camel entity);

    void Update(Camel entity);

    void RemoveImage(CamelImage image);

    Task AddImagesAsync(IEnumerable<CamelImage> images);

    Task<int> SaveChangesAsync();
}
