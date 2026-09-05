using Domain.Entities;
using Shared.DTOs.Animals;

namespace ServicesAbstraction;

public interface IOtherAnimalRepository
{
    Task<OtherAnimal?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<OtherAnimal?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<OtherAnimal> Items, int TotalCount)> GetPagedAsync(
        OtherAnimalFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<OtherAnimalTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<List<OtherAnimalPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<List<OtherAnimalAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<List<OtherAnimalGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<List<OtherAnimalHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<List<OtherAnimalVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(OtherAnimal entity);

    void Update(OtherAnimal entity);

    void RemoveImage(OtherAnimalImage image);

    Task AddImagesAsync(IEnumerable<OtherAnimalImage> images);

    Task<int> SaveChangesAsync();
}
