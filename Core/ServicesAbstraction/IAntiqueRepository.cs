using Domain.Entities;
using Shared.DTOs.Antiques;

namespace ServicesAbstraction;

public interface IAntiqueRepository
{
    Task<Antique?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Antique?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Antique> Items, int TotalCount)> GetPagedAsync(
        AntiqueFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<AntiqueTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<List<AntiqueMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<AntiqueConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<List<AntiqueWorkingStatusLookup>> GetWorkingStatusesAsync(CancellationToken cancellationToken = default);

    Task<List<AntiqueOriginalityLookup>> GetOriginalitiesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Antique entity);

    void Update(Antique entity);

    void RemoveImage(AntiqueImage image);

    Task AddImagesAsync(IEnumerable<AntiqueImage> images);

    void RemoveVideo(AntiqueVideo video);

    Task AddVideoAsync(AntiqueVideo video);

    Task<int> SaveChangesAsync();
}
