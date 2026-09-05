using Domain.Entities;
using Shared.DTOs.Antiques;

namespace ServicesAbstraction;

public interface IHandmadeRepository
{
    Task<Handmade?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Handmade?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Handmade> Items, int TotalCount)> GetPagedAsync(
        HandmadeFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<HandmadeTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<List<HandmadeColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Handmade entity);

    void Update(Handmade entity);

    void RemoveImage(HandmadeImage image);

    Task AddImagesAsync(IEnumerable<HandmadeImage> images);

    void RemoveVideo(HandmadeVideo video);

    Task AddVideoAsync(HandmadeVideo video);

    void RemoveColors(IEnumerable<HandmadeColorSelection> colors);

    Task AddColorsAsync(IEnumerable<HandmadeColorSelection> colors);

    Task<int> SaveChangesAsync();
}
