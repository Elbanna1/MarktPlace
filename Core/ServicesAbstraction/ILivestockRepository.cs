using Domain.Entities;
using Shared.DTOs.Animals;

namespace ServicesAbstraction;

public interface ILivestockRepository
{
    Task<Livestock?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Livestock?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Livestock> Items, int TotalCount)> GetPagedAsync(
        LivestockFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<LivestockBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default);

    Task<List<LivestockPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<List<LivestockAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<List<LivestockGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<List<LivestockHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<List<LivestockVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default);

    Task<List<LivestockProductionLookup>> GetProductionsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Livestock entity);

    void Update(Livestock entity);

    void RemoveImage(LivestockImage image);

    Task AddImagesAsync(IEnumerable<LivestockImage> images);

    Task<int> SaveChangesAsync();
}
