using Domain.Entities;
using Shared.DTOs.Animals;

namespace ServicesAbstraction;

public interface IHorseRepository
{
    Task<Horse?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Horse?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Horse> Items, int TotalCount)> GetPagedAsync(
        HorseFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<HorseBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default);

    Task<List<HorsePurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<List<HorseAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<List<HorseGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<List<HorseHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<List<HorseTrainingLevelLookup>> GetTrainingLevelsAsync(CancellationToken cancellationToken = default);

    Task<List<HorseVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Horse entity);

    void Update(Horse entity);

    void RemoveImage(HorseImage image);

    Task AddImagesAsync(IEnumerable<HorseImage> images);

    Task<int> SaveChangesAsync();
}
