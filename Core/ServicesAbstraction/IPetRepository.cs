using Domain.Entities;
using Shared.DTOs.Animals;

namespace ServicesAbstraction;

public interface IPetRepository
{
    Task<Pet?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Pet?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Pet> Items, int TotalCount)> GetPagedAsync(
        PetFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<PetBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default);

    Task<List<PetPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<List<PetAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<List<PetGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<List<PetHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<List<PetTrainingLevelLookup>> GetTrainingLevelsAsync(CancellationToken cancellationToken = default);

    Task<List<PetVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Pet entity);

    void Update(Pet entity);

    void RemoveImage(PetImage image);

    Task AddImagesAsync(IEnumerable<PetImage> images);

    Task<int> SaveChangesAsync();
}
