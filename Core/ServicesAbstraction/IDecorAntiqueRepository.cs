using Domain.Entities;
using Shared.DTOs.Antiques;

namespace ServicesAbstraction;

public interface IDecorAntiqueRepository
{
    Task<DecorAntique?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<DecorAntique?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<DecorAntique> Items, int TotalCount)> GetPagedAsync(
        DecorAntiqueFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<DecorAntiqueItemTypeLookup>> GetItemTypesAsync(CancellationToken cancellationToken = default);

    Task<List<DecorAntiqueMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<DecorAntiqueConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<List<DecorAntiqueOriginalityLookup>> GetOriginalitiesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(DecorAntique entity);

    void Update(DecorAntique entity);

    void RemoveImage(DecorAntiqueImage image);

    Task AddImagesAsync(IEnumerable<DecorAntiqueImage> images);

    void RemoveVideo(DecorAntiqueVideo video);

    Task AddVideoAsync(DecorAntiqueVideo video);

    Task<int> SaveChangesAsync();
}
