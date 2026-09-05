using Domain.Entities;
using Shared.DTOs.Antiques;

namespace ServicesAbstraction;

public interface IPaintingRepository
{
    Task<Painting?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Painting?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Painting> Items, int TotalCount)> GetPagedAsync(
        PaintingFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<PaintingTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<List<PaintingMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<PaintingOriginalityLookup>> GetOriginalitiesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Painting entity);

    void Update(Painting entity);

    void RemoveImage(PaintingImage image);

    Task AddImagesAsync(IEnumerable<PaintingImage> images);

    void RemoveVideo(PaintingVideo video);

    Task AddVideoAsync(PaintingVideo video);

    Task<int> SaveChangesAsync();
}
