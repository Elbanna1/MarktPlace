using Domain.Entities;
using Shared.DTOs.Animals;

namespace ServicesAbstraction;

public interface IBirdRepository
{
    Task<Bird?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Bird?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Bird> Items, int TotalCount)> GetPagedAsync(
        BirdFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<BirdTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<List<BirdPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<List<BirdAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<List<BirdGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<List<BirdHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<List<BirdVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Bird entity);

    void Update(Bird entity);

    void RemoveImage(BirdImage image);

    Task AddImagesAsync(IEnumerable<BirdImage> images);

    Task<int> SaveChangesAsync();
}
