using Domain.Entities;
using Shared.DTOs.Animals;

namespace ServicesAbstraction;

public interface ISheepGoatRepository
{
    Task<SheepGoat?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<SheepGoat?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<SheepGoat> Items, int TotalCount)> GetPagedAsync(
        SheepGoatFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<SheepGoatBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default);

    Task<List<SheepGoatPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<List<SheepGoatAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<List<SheepGoatGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<List<SheepGoatHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<List<SheepGoatVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(SheepGoat entity);

    void Update(SheepGoat entity);

    void RemoveImage(SheepGoatImage image);

    Task AddImagesAsync(IEnumerable<SheepGoatImage> images);

    Task<int> SaveChangesAsync();
}
