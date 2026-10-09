using Domain.Entities;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Directory;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IDirectoryRepository<TListing, TImage, in TFilter>
    where TListing : DirectoryListing<TImage>
    where TImage : DirectoryListingImage
    where TFilter : DirectoryFilterParams
{
    Task<TListing?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default,
        bool includeUnmoderated = false);

    Task<TListing?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<TListing> Items, int TotalCount)> GetPagedAsync(
        TFilter filter, CancellationToken cancellationToken = default);

    Task AddAsync(TListing listing, CancellationToken cancellationToken = default);

    Task AddImagesAsync(IEnumerable<TImage> images, CancellationToken cancellationToken = default);

    void RemoveImage(TImage image);

    void Update(TListing listing);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IDirectoryListingService<in TCreate, in TUpdate, TDto, in TFilter>
    where TCreate : CreateDirectoryListingRequest
    where TUpdate : TCreate, IDirectoryGalleryEdit
    where TDto : DirectoryListingDto
    where TFilter : DirectoryFilterParams
{
    Task<TDto> CreateAsync(
        string userId, TCreate request, IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default);

    Task<TDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, TUpdate request,
        IReadOnlyList<UploadImageModel> newImages, CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin, CancellationToken cancellationToken = default);

    Task<PaginatedResult<TDto>> GetListAsync(TFilter filter, CancellationToken cancellationToken = default);

    Task<TDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
